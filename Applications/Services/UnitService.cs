using Application.ViewModels.UnitViewModels;
using Applications.Commons;
using Applications.Interfaces;
using AutoMapper;
using Domain.Entities;
using System.Drawing.Printing;

namespace Applications.Services
{
    public class UnitServices : IUnitServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UnitServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateUnitViewModel> CreateUnitAsync(CreateUnitViewModel UnitDTO)
        {
            var unitOjb = _mapper.Map<Unit>(UnitDTO);
            await _unitOfWork.UnitRepository.AddAsync(unitOjb);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CreateUnitViewModel>(unitOjb);
            }
            return null;
        }

        public async Task<Pagination<CreateUnitViewModel>> GetUnitByModuleIdAsync(Guid ModuleId, int pageIndex = 0, int pageSize = 10)
        {
            var units = await _unitOfWork.UnitRepository.ViewAllUnitByModuleIdAsync(ModuleId, pageIndex , pageSize);
            var result = _mapper.Map<Pagination<CreateUnitViewModel>>(units);
            return result;
        }

        public async Task<CreateUnitViewModel> UpdateUnitAsync(Guid UnitId, CreateUnitViewModel UnitDTO)
        {
            var unit = await _unitOfWork.UnitRepository.GetByIdAsync(UnitId);
            if (unit != null)
            {
                _mapper.Map(UnitDTO, unit);
                _unitOfWork.UnitRepository.Update(unit);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<CreateUnitViewModel>(unit);
                }
            }
            return null;
        }

        public async Task<Pagination<UnitViewModel>> GetAllUnits(int pageIndex = 0, int pageSize = 10)
        {
            var units = await _unitOfWork.UnitRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<UnitViewModel>>(units);
            return result;
        }

        public async Task<Pagination<UnitViewModel>> ViewDisableUnitsAsync(int pageIndex = 0, int pageSize = 10)
        {
            var units = await _unitOfWork.UnitRepository.GetDisableUnits(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<UnitViewModel>>(units);
            return result;
        }

        public async Task<Pagination<UnitViewModel>> ViewEnableUnitsAsync(int pageIndex = 0, int pageSize = 10)
        {
            var units = await _unitOfWork.UnitRepository.GetEnableUnits(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<UnitViewModel>>(units);
            return result;
        }

        public async Task<UnitViewModel> ViewUnitById(Guid UnitId)
        {
            var unit = await _unitOfWork.UnitRepository.GetByIdAsync(UnitId);
            return _mapper.Map<UnitViewModel>(unit);
        }

        public async Task<Pagination<UnitViewModel>> GetUnitByNameAsync(string UnitName, int pageIndex = 0, int pageSize = 10)
        {
            var units = await _unitOfWork.UnitRepository.GetUnitByNameAsync(UnitName, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<UnitViewModel>>(units);
            return result;
        }
    }
}
