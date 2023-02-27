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
            return result;
        }

        public async Task<AuditResultViewModel> GetByAudiPlanId(Guid id)
        {
            var classOjb = await _unitOfWork.AuditResultRepository.GetByAuditPlanId(id);
            var result = _mapper.Map<AuditResultViewModel>(classOjb);
            return result;
        }
    }
}
