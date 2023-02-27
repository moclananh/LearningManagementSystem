﻿using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.AuditPlanViewModel;
using Applications.ViewModels.UserAuditPlanViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;

namespace Applications.Services
{
    public class AuditPlanService : IAuditPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuditPlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuditPlanViewModel?> CreateAuditPlanAsync(AuditPlanViewModel AuditPlanDTO)
        {
            var auditPlan = _mapper.Map<AuditPlan>(AuditPlanDTO);
            await _unitOfWork.AuditPlanRepository.AddAsync(auditPlan);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<AuditPlanViewModel>(auditPlan);
            }
            return null;
        }

        public async Task<Pagination<AuditPlanViewModel>> GetAllAuditPlanAsync(int pageIndex = 0, int pageSize = 10)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<AuditPlanViewModel>>(auditplans);
            return result;
        }

        public async Task<Pagination<AuditPlanViewModel>> GetAuditPlanbyClassIdAsync(Guid ClassId, int pageIndex = 0, int pageSize = 10)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetAuditPlanByClassId(ClassId, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<AuditPlanViewModel>>(auditplans);
            return result;
        }

        public async Task<AuditPlanViewModel> GetAuditPlanByIdAsync(Guid AuditPlanId)
        {
            var AuditPlans = await _unitOfWork.AuditPlanRepository.GetByIdAsync(AuditPlanId);
            var result = _mapper.Map<AuditPlanViewModel>(AuditPlans);
            return result;
        }

        public async Task<AuditPlanViewModel> GetAuditPlanByModuleIdAsync(Guid ModuleId)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetAuditPlanByModuleId(ModuleId);
            var result = _mapper.Map<AuditPlanViewModel>(auditplans);
            return result;
        }

        public async Task<Pagination<AuditPlanViewModel>> GetAuditPlanByName(string AuditPlanName, int pageIndex = 0, int pageSize = 10)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetAuditPlanByName(AuditPlanName, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<AuditPlanViewModel>>(auditplans);
            return result;
        }

        public async Task<Pagination<AuditPlanViewModel>> GetDisableAuditPlanAsync(int pageIndex = 0, int pageSize = 10)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetDisableAuditPlans(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<AuditPlanViewModel>>(auditplans);
            return result;
        }

        public async Task<Pagination<AuditPlanViewModel>> GetEnableAuditPlanAsync(int pageIndex = 0, int pageSize = 10)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetEnableAuditPlans(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<AuditPlanViewModel>>(auditplans);
            return result;
        }

        public async Task<UpdateAuditPlanViewModel?> UpdateAuditPlanAsync(Guid AuditPlanId, UpdateAuditPlanViewModel AuditPlanDTO)
        {
            var auditplanObj = await _unitOfWork.AuditPlanRepository.GetByIdAsync(AuditPlanId);
            if (auditplanObj != null)
            {
                _mapper.Map(AuditPlanDTO, auditplanObj);
                _unitOfWork.AuditPlanRepository.Update(auditplanObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateAuditPlanViewModel>(auditplanObj);
                }
            }
            return null;
        }
        public async Task<CreateUserAuditPlanViewModel> AddUserToAuditPlan(Guid AuditPlanId, Guid UserId)
        {
            var auditOjb = await _unitOfWork.AuditPlanRepository.GetByIdAsync(AuditPlanId);
            var user = await _unitOfWork.UserRepository.GetByIdAsync(UserId);
            if (auditOjb != null && user != null)
            {
                var userAuditPlan = new UserAuditPlan()
                {
                    AuditPlan = auditOjb,
                    User = user
                };
                await _unitOfWork.UserAuditPlanRepository.AddAsync(userAuditPlan);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<CreateUserAuditPlanViewModel>(userAuditPlan);
                }
            }
            return null;
        }
        public async Task<CreateUserAuditPlanViewModel> RemoveUserToAuditPlan(Guid AuditPlanId, Guid UserId)
        {
            var user = await _unitOfWork.UserAuditPlanRepository.GetUserAuditPlan(AuditPlanId, UserId);
            if (user != null)
            {
                _unitOfWork.UserAuditPlanRepository.SoftRemove(user);
                var isSucces = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSucces)
                {
                    return _mapper.Map<CreateUserAuditPlanViewModel>(user);
                }
            }
            return null;
        }
    }
}
