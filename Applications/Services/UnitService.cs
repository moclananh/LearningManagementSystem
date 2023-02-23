using Application.ViewModels.UnitViewModels;
using Applications.Interfaces;
using AutoMapper;
using Domain.Entities;

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

        public async Task<List<UnitViewModel>> ViewAllUnitAsync()
        {
            var units = await _unitOfWork.UnitRepository.GetAllAsync();
            var result = _mapper.Map<List<UnitViewModel>>(units);
            return result;
        }

        public async Task<List<UnitViewModel>> ViewDisableUnitsAsync()
        {
            var units = await _unitOfWork.UnitRepository.GetDisableUnits();
            var result = _mapper.Map<List<UnitViewModel>>(units);
            return result;
        }

        public async Task<List<UnitViewModel>> ViewEnableUnitsAsync()
        {
            var units = await _unitOfWork.UnitRepository.GetEnableUnits();
            var result = _mapper.Map<List<UnitViewModel>>(units);
            return result;
        }

        public async Task<UnitViewModel> ViewUnitById(Guid UnitId)
        {
            var unit = await _unitOfWork.UnitRepository.GetByIdAsync(UnitId);
            return _mapper.Map<UnitViewModel>(unit);
        }
    }
}
