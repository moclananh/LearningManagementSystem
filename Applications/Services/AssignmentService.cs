using Applications.Interfaces;
using Applications.ViewModels.AssignmentViewModels;
using AutoMapper;
using Domain.Entities;

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

        public async Task<UpdateAssignmentViewModel> GetAssignmentById(Guid AssignmentId)
        {
            var asmObj = await _unitOfWork.AssignmentRepository.GetByIdAsync(AssignmentId);
            var result = _mapper.Map<UpdateAssignmentViewModel>(asmObj);
            return result;
        }

        public async Task<List<UpdateAssignmentViewModel>> GetAssignmentByUnitId(Guid UnitId)
        {
            var asmObj = await _unitOfWork.AssignmentRepository.GetAssignmentByUnitId(UnitId);
            var result = _mapper.Map<List<UpdateAssignmentViewModel>>(asmObj);
            return result;
        }

        public async Task<List<UpdateAssignmentViewModel>> GetDisableAssignments()
        {
            var asmObj = await _unitOfWork.AssignmentRepository.GetDisableAssignmentAsync();
            var result = _mapper.Map<List<UpdateAssignmentViewModel>>(asmObj);
            return result;
        }

        public async Task<List<UpdateAssignmentViewModel>> GetEnableAssignments()
        {
            var asmObj = await _unitOfWork.AssignmentRepository.GetEnableAssignmentAsync();
            var result = _mapper.Map<List<UpdateAssignmentViewModel>>(asmObj);
            return result;
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
        public async Task<List<AssignmentViewModel>> ViewAllAssignmentAsync()
        {
            var assignment = await _unitOfWork.AssignmentRepository.GetAllAsync();
            var result = _mapper.Map<List<AssignmentViewModel>>(assignment);
            return result;
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
    }
}
