using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.UnitModuleViewModel;
using AutoMapper;
namespace Applications.Services
{
    public class ModuleUnitService : IModuleUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ModuleUnitService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Pagination<ModuleUnitViewModel>> GetAllModuleUnitsAsync(int pageIndex = 0, int pageSize = 10)
        {
            var moduleUnit = await _unitOfWork.ModuleUnitRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ModuleUnitViewModel>>(moduleUnit);
            return result;
        }
    }
}
