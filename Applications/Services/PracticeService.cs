using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.PracticeViewModels;
using AutoMapper;

namespace Applications.Services
{
    public class PracticeService : IPracticeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PracticeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PracticeViewModel> GetPracticeById(Guid Id)
        {
            var praObj = await _unitOfWork.PracticeRepository.GetByIdAsync(Id);
            var result = _mapper.Map<PracticeViewModel>(praObj);
            return result;
        }

        public async Task<Pagination<PracticeViewModel>> GetPracticeByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10)
        {
            var praObj = await _unitOfWork.PracticeRepository.GetPracticeByUnitId(UnitId);
            var result = _mapper.Map<Pagination<PracticeViewModel>>(praObj);
            return result;
        }
    }
}
