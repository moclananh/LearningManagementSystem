using Application.ViewModels.UnitViewModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using AutoMapper;
using Domain.Entities;
using System.Net;

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

        public async Task<Response> CreateUnitAsync(CreateUnitViewModel UnitDTO)
        {
            var unitOjb = _mapper.Map<Unit>(UnitDTO);
            await _unitOfWork.UnitRepository.AddAsync(unitOjb);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return new Response(HttpStatusCode.OK, "Created success", _mapper.Map<CreateUnitViewModel>(unitOjb));
            }
            return new Response(HttpStatusCode.BadRequest, "Created Failed");
        }

        public async Task<UnitViewModel> UpdateUnitAsync(Guid UnitId, CreateUnitViewModel UnitDTO)
        {
            var unit = await _unitOfWork.UnitRepository.GetByIdAsync(UnitId);
            if (unit is object)
            {
                _mapper.Map(UnitDTO, unit);
                _unitOfWork.UnitRepository.Update(unit);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UnitViewModel>(unit);
                }
            } 
            return null;
        }

        public async Task<Response> GetUnitByModuleIdAsync(Guid ModuleId, int pageIndex = 0, int pageSize = 10)
        {
            var units = await _unitOfWork.UnitRepository.ViewAllUnitByModuleIdAsync(ModuleId, pageIndex, pageSize);
            if (units.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<UnitViewModel>>(units));
        }

        public async Task<Response> GetUnitByNameAsync(string UnitName, int pageIndex = 0, int pageSize = 10)
        {
            var units = await _unitOfWork.UnitRepository.GetUnitByNameAsync(UnitName, pageIndex, pageSize);
            if (units.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<UnitViewModel>>(units));
        }

        public async Task<Response> GetDisableUnitsAsync(int pageIndex = 0, int pageSize = 10)
        {
            var units = await _unitOfWork.UnitRepository.GetDisableUnits(pageIndex, pageSize);
            if (units.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<UnitViewModel>>(units));
        }

        public async Task<Response> GetEnableUnitsAsync(int pageIndex = 0, int pageSize = 10)
        {
            var units = await _unitOfWork.UnitRepository.GetEnableUnits(pageIndex, pageSize);
            if (units.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<UnitViewModel>>(units));
        }

        public async Task<Response> GetUnitById(Guid UnitId)
        {
            var unit = await _unitOfWork.UnitRepository.GetByIdAsync(UnitId);
            var result = _mapper.Map<UnitViewModel>(unit);
            var createBy = await _unitOfWork.UserRepository.GetByIdAsync(unit?.CreatedBy);
            if (createBy != null)
            {
                result.CreatedBy = createBy.Email;
            }
            if (unit == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", result);
        }

        public async Task<Response> GetAllUnits(int pageNumber = 0, int pageSize = 10)
        {
            var unit = await _unitOfWork.UnitRepository.ToPagination(pageNumber, pageSize);
            var result = _mapper.Map<Pagination<UnitViewModel>>(unit);
            var guidList = unit.Items.Select(x => x.CreatedBy).ToList();
            var users = await _unitOfWork.UserRepository.GetEntitiesByIdsAsync(guidList);

            foreach (var item in result.Items)
            {
                if (string.IsNullOrEmpty(item.CreatedBy)) continue;

                var createdBy = users.FirstOrDefault(x => x.Id == Guid.Parse(item.CreatedBy));
                if (createdBy != null)
                {
                    item.CreatedBy = createdBy.Email;
                }
            }
            if (unit.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }
    }
}
