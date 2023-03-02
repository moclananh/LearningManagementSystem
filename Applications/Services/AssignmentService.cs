using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.AssignmentViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using System.Drawing.Printing;
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
            var asmObj = await _unitOfWork.AssignmentRepository.GetByIdAsync(AssignmentId);
            if (asmObj == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", _mapper.Map<UpdateAssignmentViewModel>(asmObj));
        }

        public async Task<Response> GetAssignmentByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10)
        {
            var asmObj = await _unitOfWork.AssignmentRepository.GetAssignmentByUnitId(UnitId, pageIndex, pageSize);
            if (asmObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Assignment Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<UpdateAssignmentViewModel>>(asmObj));
        }

        public async Task<Response> GetDisableAssignments(int pageIndex = 0, int pageSize = 10)
        {
            var asmObj = await _unitOfWork.AssignmentRepository.GetDisableAssignmentAsync(pageIndex, pageSize);
            if (asmObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Assignment Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<UpdateAssignmentViewModel>>(asmObj));
        }

        public async Task<Response> GetEnableAssignments(int pageIndex = 0, int pageSize = 10)
        {
            var asmObj = await _unitOfWork.AssignmentRepository.GetEnableAssignmentAsync(pageIndex, pageSize);
            if (asmObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Assignment Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<UpdateAssignmentViewModel>>(asmObj));
        }

        public async Task<UpdateAssignmentViewModel?> UpdateAssignment(Guid AssignmentId, UpdateAssignmentViewModel assignmentDTO)
        {
            var asmObj = await _unitOfWork.AssignmentRepository.GetByIdAsync(AssignmentId);
            if (asmObj != null)
            {
                _mapper.Map(assignmentDTO, asmObj);
                _unitOfWork.AssignmentRepository.Update(asmObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateAssignmentViewModel>(asmObj);
                }
            }
            return null;
        }
        public async Task<Response> ViewAllAssignmentAsync(int pageIndex = 0, int pageSize = 10)
        {
            var asmObj = await _unitOfWork.AssignmentRepository.ToPagination(pageIndex, pageSize);
            if (asmObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Assignment Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<AssignmentViewModel>>(asmObj));
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
            var asmObj = await _unitOfWork.AssignmentRepository.GetAssignmentByName(Name, pageIndex, pageSize);
            if (asmObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Assignment Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<UpdateAssignmentViewModel>>(asmObj));
        }
    }
}
