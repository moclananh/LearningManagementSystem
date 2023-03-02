﻿using Application.ViewModels.UnitViewModels;
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
            if (unit == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", _mapper.Map<UnitViewModel>(unit));
        }

        public async Task<Response> GetAllUnits(int pageNumber = 0, int pageSize = 10)
        {
            var unit = await _unitOfWork.UnitRepository.ToPagination(pageNumber, pageSize);
            if (unit.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<UnitViewModel>>(unit));
        }
    }
}
