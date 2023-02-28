using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ClassViewModels;
using Applications.ViewModels.PracticeViewModels;
using AutoMapper;
using Domain.Entities;

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
        public async Task<CreatePracticeViewModel> CreatePracticeAsync(CreatePracticeViewModel PracticeDTO)
        {
            var practiceOjb = _mapper.Map<Practice>(PracticeDTO);
            await _unitOfWork.PracticeRepository.AddAsync(practiceOjb);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CreatePracticeViewModel>(practiceOjb);
            }
            return null;
        }
        public async Task<Pagination<PracticeViewModel>> GetAllPractice(int pageIndex = 0, int pageSize = 10)
        {
            var practiceOjb = await _unitOfWork.PracticeRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<PracticeViewModel>>(practiceOjb);
            return result;
        }
        public async Task<Pagination<PracticeViewModel>> GetpracticeByName(string Name, int pageIndex = 0, int pageSize = 10)
        {
            var practiceOjb = await _unitOfWork.PracticeRepository.GetPracticeByName(Name, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<PracticeViewModel>>(practiceOjb);
            return result;
        }
        public async Task<Pagination<PracticeViewModel>> GetDisablePractice(int pageIndex = 0, int pageSize = 10)
        {
            var practices = await _unitOfWork.PracticeRepository.GetDisablePractices();
            var result = _mapper.Map<Pagination<PracticeViewModel>>(practices);
            return result;
        }
        public async Task<Pagination<PracticeViewModel>> GetEnablePractice(int pageIndex = 0, int pageSize = 10)
        {
            var practices = await _unitOfWork.PracticeRepository.GetEnablePractices();
            var result = _mapper.Map<Pagination<PracticeViewModel>>(practices);
            return result;
        }

        public async Task<UpdatePracticeViewModel> UpdatePractice(Guid UnitId, UpdatePracticeViewModel practiceDTO)
        {
            var classObj = await _unitOfWork.PracticeRepository.GetByIdAsync(UnitId);
            if (classObj != null)
            {
                _mapper.Map(practiceDTO, classObj);
                _unitOfWork.PracticeRepository.Update(classObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdatePracticeViewModel>(classObj);
                }
            }
            return null;
        }
    }
}
