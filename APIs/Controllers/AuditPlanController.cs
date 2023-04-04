using Applications.Interfaces;
using Applications.ViewModels.AuditPlanViewModel;
using Applications.ViewModels.Response;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    [EnableCors("AllowAll")]
    public class AuditPlanController : ControllerBase
    {
        private readonly IAuditPlanService _auditPlanService;
        private readonly IValidator<AuditPlanViewModel> _validator;
        private readonly IValidator<UpdateAuditPlanViewModel> _validatorUpdate;
        private readonly IValidator<CreateAuditPlanViewModel> _validatorCreate;

        public AuditPlanController(IAuditPlanService auditPlanService,
            IValidator<AuditPlanViewModel> validator,
            IValidator<UpdateAuditPlanViewModel> validatorUpdate,
            IValidator<CreateAuditPlanViewModel> validatorCreate)
        {
            _auditPlanService = auditPlanService;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
            _validatorCreate = validatorCreate;
        }

        [HttpGet("GetAllAuditPlan")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAllAuditPlanAsync(int pageIndex = 0, int pageSize = 10) => await _auditPlanService.GetAllAuditPlanAsync(pageIndex, pageSize);

        [HttpGet("GetEnableAuditPlan")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetEnableAuditPlans(int pageIndex = 0, int pageSize = 10) => await _auditPlanService.GetEnableAuditPlanAsync(pageIndex, pageSize);

        [HttpGet("GetDisableAuditPlan")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetDiableAuditPlans(int pageIndex = 0, int pageSize = 10) => await _auditPlanService.GetDisableAuditPlanAsync(pageIndex, pageSize);

        [HttpGet("GetAuditPlanById/{AuditPlanId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAuditPlanByIdAsync(Guid AuditPlanId) => await _auditPlanService.GetAuditPlanByIdAsync(AuditPlanId);

        [HttpGet("GetAuditPlanByModuleId/{ModuleId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAuditPlanByModuleId(Guid ModuleId) => await _auditPlanService.GetAuditPlanByModuleIdAsync(ModuleId);

        [HttpGet("GetAuditPlanByClassId/{ClassId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAuditPlanByClassId(Guid ClassId, int pageIndex = 0, int pageSize = 10) => await _auditPlanService.GetAuditPlanbyClassIdAsync(ClassId, pageIndex, pageSize);

        [HttpGet("GetAuditPlanByName/{AuditPlanName}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAuditPlanByName(string AuditPlanName, int pageIndex = 0, int pageSize = 10) => await _auditPlanService.GetAuditPlanByName(AuditPlanName, pageIndex, pageSize);
        
        [HttpPost("CreateAuditPlan")]
        [Authorize(policy: "Audits")]
        public async Task<IActionResult> CreateAuditPlan(CreateAuditPlanViewModel createAuditPlanViewModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorCreate.Validate(createAuditPlanViewModel);
                if (result.IsValid)
                {
                    if(await _auditPlanService.CreateAuditPlanAsync(createAuditPlanViewModel) != null)
                    {
                        return Ok("Update AuditPlan Success");
                    }
                    return BadRequest("Invalid Id");
                }
            }
            return BadRequest("Create Failed,Invalid Input Information");
        }

        [HttpPut("UpdateAuditPlan/{AuditPlanId}")]
        [Authorize(policy: "Audits")]
        public async Task<IActionResult> UpdateAuditPlan(Guid AuditPlanId, UpdateAuditPlanViewModel updateAuditPlanView)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorUpdate.Validate(updateAuditPlanView);
                if (result.IsValid)
                {
                    if (await _auditPlanService.UpdateAuditPlanAsync(AuditPlanId, updateAuditPlanView) != null)
                    {
                        return Ok("Update Assignment Success");
                    }
                    return BadRequest("Invalid AuditPlan Id");
                }
            }
            return BadRequest("Update Failed,Invalid Input Information");
        }

        [HttpPost("AuditPlan/AddUser/{AuditPlanId}/{UserId}")]
        [Authorize(policy: "Audits")]
        public async Task<IActionResult> AddUser(Guid AuditPlanId, Guid UserId)
        {
            var result = await _auditPlanService.AddUserToAuditPlan(AuditPlanId, UserId);
            if (result != null)
            {
                return Ok("Add Success");
            }

            return BadRequest("Add UserToAuditPlan Fail");
        }

        [HttpDelete("AuditPlan/DeleteUser/{AuditPlanId}/{UserId}")]
        [Authorize(policy: "Audits")]
        public async Task<IActionResult> DeleteUser(Guid AuditPlanId, Guid UserId)
        {
            var result = await _auditPlanService.RemoveUserFromAuditPlan(AuditPlanId, UserId);
            if (result != null)
            {
                return Ok("Remove Success");
            }

            return BadRequest("Remove UserFromAuditPlan Fail");
        }

        [HttpGet("GetAllUserAuditPlan")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetAllUserAuditPlan(int pageIndex = 0, int pageSize = 10)
        {
            return await _auditPlanService.GetAllUserAuditPlanAsync(pageIndex, pageSize);
        }
    }
}
