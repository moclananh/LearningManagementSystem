using Application.Interfaces;
using Applications;
using Applications.ViewModels.AuditResultViewModels;
using AutoMapper;

namespace Application.Services
{
    public class AuditResultServices : IAuditResultServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuditResultServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuditResultViewModel> GetAuditResultById(Guid Id)
        {
            var classOjb = await _unitOfWork.AuditResultRepository.GetAuditResultById(Id);
            var result = _mapper.Map<AuditResultViewModel>(classOjb);
            var createBy = await _unitOfWork.UserRepository.GetByIdAsync(classOjb.CreatedBy);
            result.CreatedBy = createBy.Email;
            result.UserId = createBy.Id;
            return result;
        }

        public async Task<AuditResultViewModel> GetByAudiPlanId(Guid id)
        {
            var classOjb = await _unitOfWork.AuditResultRepository.GetByAuditPlanId(id);
            var result = _mapper.Map<AuditResultViewModel>(classOjb);
            var createBy = await _unitOfWork.UserRepository.GetByIdAsync(classOjb.CreatedBy);
            result.CreatedBy = createBy.Email;
            result.UserId = createBy.Id;
            return result;
        }

        public async Task<UpdateAuditResultViewModel> UpdateAuditResult(Guid AuditResultId, UpdateAuditResultViewModel classDTO)
        {
            var aditRsObj = await _unitOfWork.AuditResultRepository.GetByIdAsync(AuditResultId);
            if (aditRsObj != null)
            {
                _mapper.Map(classDTO, aditRsObj);
                _unitOfWork.AuditResultRepository.Update(aditRsObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateAuditResultViewModel>(aditRsObj);
                }
            }
            return null;
        }
    }
}
