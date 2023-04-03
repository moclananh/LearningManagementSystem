using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.AuditPlanViewModel;
using Applications.ViewModels.Response;
using Applications.ViewModels.UserAuditPlanViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;
using System.Net;

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

        public async Task<CreateAuditPlanViewModel?> CreateAuditPlanAsync(CreateAuditPlanViewModel AuditPlanDTO)
        {
            var auditPlan = _mapper.Map<AuditPlan>(AuditPlanDTO);
            await _unitOfWork.AuditPlanRepository.AddAsync(auditPlan);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CreateAuditPlanViewModel>(auditPlan);
            }
            return null;
        }

        public async Task<Response> GetAllAuditPlanAsync(int pageIndex = 0, int pageSize = 10)
        {
            var auditPlans = await _unitOfWork.AuditPlanRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<AuditPlanViewModel>>(auditPlans);
            var guidList = auditPlans.Items.Select(x => x.CreatedBy).ToList();
            var users = await _unitOfWork.UserRepository.GetEntitiesByIdsAsync(guidList);
            foreach (var item in result.Items)
            {
                if (string.IsNullOrEmpty(item.CreatedBy)) continue;
                var createBy = users.FirstOrDefault(x => x.Id == Guid.Parse(item.CreatedBy));
                if (createBy != null)
                {
                    item.CreatedBy = createBy.Email;
                }
            }
            if (auditPlans.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No AuditPlan Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }

        public async Task<Response> GetAuditPlanbyClassIdAsync(Guid ClassId, int pageIndex = 0, int pageSize = 10)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetAuditPlanByClassId(ClassId, pageIndex, pageSize);
            if (auditplans.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No ClassId Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<AuditPlanViewModel>>(auditplans));
        }

        public async Task<Response> GetAuditPlanByIdAsync(Guid AuditPlanId)
        {
            var auditPlans = await _unitOfWork.AuditPlanRepository.GetByIdAsync(AuditPlanId);
            var result = _mapper.Map<AuditPlanViewModel>(auditPlans);
            var createBy = await _unitOfWork.UserRepository.GetByIdAsync(auditPlans?.CreatedBy);
            if (createBy != null)
            {
                result.CreatedBy = createBy.Email;
            }
            if (auditPlans == null) return new Response(HttpStatusCode.NoContent, "No AuditPlan Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }

        public async Task<Response> GetAuditPlanByModuleIdAsync(Guid ModuleId)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetAuditPlanByModuleId(ModuleId);
            if (auditplans == null) return new Response(HttpStatusCode.NoContent, "No ModuleId found");
            else return new Response(HttpStatusCode.OK, "Search succeed", _mapper.Map<AuditPlanViewModel>(auditplans));
        }

        public async Task<Response> GetAuditPlanByName(string AuditPlanName, int pageIndex = 0, int pageSize = 10)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetAuditPlanByName(AuditPlanName, pageIndex, pageSize);
            if (auditplans.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No AuditPlan Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<AuditPlanViewModel>>(auditplans));
        }

        public async Task<Response> GetDisableAuditPlanAsync(int pageIndex = 0, int pageSize = 10)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetDisableAuditPlans(pageIndex, pageSize);
            if (auditplans.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No AuditPlan Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<AuditPlanViewModel>>(auditplans));
        }

        public async Task<Response> GetEnableAuditPlanAsync(int pageIndex = 0, int pageSize = 10)
        {
            var auditplans = await _unitOfWork.AuditPlanRepository.GetEnableAuditPlans(pageIndex, pageSize);
            if (auditplans.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No AuditPlan Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<AuditPlanViewModel>>(auditplans));
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
            if (auditOjb is object && user is object)
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
        public async Task<CreateUserAuditPlanViewModel> RemoveUserFromAuditPlan(Guid AuditPlanId, Guid UserId)
        {
            var user = await _unitOfWork.UserAuditPlanRepository.GetUserAuditPlan(AuditPlanId, UserId);
            if (user is object)
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
        public async Task<Response> GetAllUserAuditPlanAsync(int pageIndex = 0, int pageSize = 10)
        {
            var auditplans = await _unitOfWork.UserAuditPlanRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<UserAuditPlanViewModel>>(auditplans);
            var guidList = auditplans.Items.Select(x => x.CreatedBy).ToList();
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
            if (auditplans.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No User AuditPlan Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }
    }
}
