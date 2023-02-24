using Applications.Interfaces;
using Applications.ViewModels.AuditPlanViewModel;
using AutoMapper;
using Domain.Entities;

namespace Applications.Services
{
    public class AuditPlanService : IAuditPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuditPlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuditPlanViewModel?> CreateAuditPlanAsync(AuditPlanViewModel AuditPlanDTO)
        {
            var auditPlan = _mapper.Map<AuditPlan>(AuditPlanDTO);
            await _unitOfWork.AuditPlanRepository.AddAsync(auditPlan);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<AuditPlanViewModel>(auditPlan);
            }
            return null;
        }

        public async Task<List<AuditPlanViewModel>> GetAllAuditPlanAsync()
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetAllAsync();
            var result = _mapper.Map<List<AuditPlanViewModel>>(auditplans);
            return result;
        }

        public async Task<List<AuditPlanViewModel>> GetAuditPlanbyClassIdAsync(Guid ClassId)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetAuditPlanByClassId(ClassId);
            var result = _mapper.Map<List<AuditPlanViewModel>>(auditplans);
            return result;
        }

        public async Task<AuditPlanViewModel> GetAuditPlanByIdAsync(Guid AuditPlanId)
        {
            var AuditPlans = await _unitOfWork.AuditPlanRepository.GetByIdAsync(AuditPlanId);
            var result = _mapper.Map<AuditPlanViewModel>(AuditPlans);
            return result;
        }

        public async Task<List<AuditPlanViewModel>> GetAuditPlanByModuleIdAsync(Guid ModuleId)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetAuditPlanByModuleId(ModuleId);
            var result = _mapper.Map<List<AuditPlanViewModel>>(auditplans);
            return result;
        }

        public async Task<List<AuditPlanViewModel>> GetDisableAuditPlanAsync()
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetDisableAuditPlans();
            var result = _mapper.Map<List<AuditPlanViewModel>>(auditplans);
            return result;
        }

        public async Task<List<AuditPlanViewModel>> GetEnableAuditPlanAsync()
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetEnableAuditPlans();
            var result = _mapper.Map<List<AuditPlanViewModel>>(auditplans);
            return result;
        }

        public async Task<UpdateAuditPlanViewModel?> UpdateAuditPlanAsync(Guid AuditPlanId, UpdateAuditPlanViewModel AuditPlanDTO)
        {
            var auditplanObj = await _unitOfWork.AuditPlanRepository.GetByIdAsync(AuditPlanId);
            if (auditplanObj != null)
            {
                _mapper.Map(AuditPlanDTO, auditplanObj);
                _unitOfWork.AuditPlanRepository.Update(auditplanObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateAuditPlanViewModel>(auditplanObj);
                }
            }
            return null;
        }
    }
}
