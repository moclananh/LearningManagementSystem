using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.AssignmentViewModels;
using Applications.ViewModels.LectureViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
using Domain.Entities;
using System.Net;

namespace Applications.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AssignmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> GetAssignmentById(Guid AssignmentId)
        {
            var assignmentObj = await _unitOfWork.AssignmentRepository.GetByIdAsync(AssignmentId);
            var result = _mapper.Map<AssignmentViewModel>(assignmentObj);
            var createBy = await _unitOfWork.UserRepository.GetByIdAsync(assignmentObj?.CreatedBy);
            if (createBy != null)
            {
                result.CreatedBy = createBy.Email;
            }
            if (assignmentObj == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", result);
        }

        public async Task<Response> GetAssignmentByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10)
        {
            var assignmentObj = await _unitOfWork.AssignmentRepository.GetAssignmentByUnitId(UnitId, pageIndex, pageSize);
            if (assignmentObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Assignment Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<AssignmentViewModel>>(assignmentObj));
        }

        public async Task<Response> GetDisableAssignments(int pageIndex = 0, int pageSize = 10)
        {
            var assignmentObj = await _unitOfWork.AssignmentRepository.GetDisableAssignmentAsync(pageIndex, pageSize);
            if (assignmentObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Assignment Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<AssignmentViewModel>>(assignmentObj));
        }

        public async Task<Response> GetEnableAssignments(int pageIndex = 0, int pageSize = 10)
        {
            var assignmentObj = await _unitOfWork.AssignmentRepository.GetEnableAssignmentAsync(pageIndex, pageSize);
            if (assignmentObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Assignment Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<AssignmentViewModel>>(assignmentObj));
        }

        public async Task<UpdateAssignmentViewModel?> UpdateAssignment(Guid AssignmentId, UpdateAssignmentViewModel assignmentDTO)
        {
            var assignmentObj = await _unitOfWork.AssignmentRepository.GetByIdAsync(AssignmentId);
            if (assignmentObj is object)
            {
                _mapper.Map(assignmentDTO, assignmentObj);
                _unitOfWork.AssignmentRepository.Update(assignmentObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateAssignmentViewModel>(assignmentObj);
                }
            }
            return null;
        }
        public async Task<Response> ViewAllAssignmentAsync(int pageIndex = 0, int pageSize = 10)
        {
            var assignmentObj = await _unitOfWork.AssignmentRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<AssignmentViewModel>>(assignmentObj);
            var guidList = assignmentObj.Items.Select(s => s.CreatedBy).ToList();
            var users = await _unitOfWork.UserRepository.GetEntitiesByIdsAsync(guidList);

            foreach (var item in result.Items)
            {
                if (string.IsNullOrEmpty(item.CreatedBy)) continue;

                var createdBy = users.FirstOrDefault(x => x.Id == Guid.Parse(item.CreatedBy));
                if (createdBy != null)
                {
                    item.CreatedBy = createdBy.Email;
                }
            }
            if (assignmentObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Assignment Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }

        public async Task<CreateAssignmentViewModel> CreateAssignmentAsync(CreateAssignmentViewModel AssignmentDTO)
        {
            var assignmentOjb = _mapper.Map<Assignment>(AssignmentDTO);
            await _unitOfWork.AssignmentRepository.AddAsync(assignmentOjb);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CreateAssignmentViewModel>(assignmentOjb);
            }
            return null;
        }

        public async Task<Response> GetAssignmentByName(string Name, int pageIndex = 0, int pageSize = 10)
        {
            var assignmentObj = await _unitOfWork.AssignmentRepository.GetAssignmentByName(Name, pageIndex, pageSize);
            if (assignmentObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Assignment Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<AssignmentViewModel>>(assignmentObj));
        }
    }
}
