using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.SyllabusModuleViewModel;
using AutoMapper;

namespace Applications.Services
{
    public class SyllabusModuleService : ISyllabusModuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SyllabusModuleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Pagination<SyllabusModuleViewModel>> GetAllSyllabusModuleAsync(int pageIndex = 0, int pageSize = 10)
        {
            var syllabusmodule = await _unitOfWork.SyllabusModuleRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<SyllabusModuleViewModel>>(syllabusmodule);
            return result;
        }
    }
}
