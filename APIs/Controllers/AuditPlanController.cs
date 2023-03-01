using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.AuditPlanViewModel;
using Applications.ViewModels.UserAuditPlanViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditPlanController : ControllerBase
    {
        private readonly IAuditPlanService _auditPlanService;
        private readonly IValidator<AuditPlanViewModel> _validator;
        private readonly IValidator<UpdateAuditPlanViewModel> _validator1;

        public AuditPlanController(IAuditPlanService auditPlanService,
            IValidator<AuditPlanViewModel> validator,
            IValidator<UpdateAuditPlanViewModel> validator1)
        {
            _auditPlanService = auditPlanService;
            _validator = validator;
            _validator1 = validator1;
        }

        [HttpGet("GetAllAuditPlan")]
        public async Task<Pagination<AuditPlanViewModel>> GetAllAuditPlanAsync(int pageIndex = 0, int pageSize = 10) => await _auditPlanService.GetAllAuditPlanAsync(pageIndex, pageSize);

        [HttpGet("GetEnableAuditPlan")]
        public async Task<Pagination<AuditPlanViewModel>> GetEnableClasses(int pageIndex = 0, int pageSize = 10) => await _auditPlanService.GetEnableAuditPlanAsync(pageIndex, pageSize);

        [HttpGet("GetDisableAuditPlan")]
        public async Task<Pagination<AuditPlanViewModel>> GetDiableClasses(int pageIndex = 0, int pageSize = 10) => await _auditPlanService.GetDisableAuditPlanAsync(pageIndex, pageSize);

        [HttpGet("GetAuditPlanById/{AuditPlanId}")]
        public async Task<AuditPlanViewModel> GetAuditPlanByIdAsync(Guid AuditPlanId) => await _auditPlanService.GetAuditPlanByIdAsync(AuditPlanId);

        [HttpGet("GetAuditPlanByModuleId/{ModuleId}")]
        public async Task<AuditPlanViewModel> GetAuditPlanByModuleId(Guid ModuleId) => await _auditPlanService.GetAuditPlanByModuleIdAsync(ModuleId);

        [HttpGet("GetAuditPlanByClassId/{ClassId}")]
        public async Task<Pagination<AuditPlanViewModel>> GetAuditPlanByClassId(Guid ClassId, int pageIndex = 0, int pageSize = 10) => await _auditPlanService.GetAuditPlanbyClassIdAsync(ClassId, pageIndex, pageSize);

        [HttpGet("GetAuditPlanByName/{AuditPlanName}")]
        public async Task<Pagination<AuditPlanViewModel>> GetAuditPlanByName(string AuditPlanName, int pageIndex = 0, int pageSize = 10) => await _auditPlanService.GetAuditPlanByName(AuditPlanName, pageIndex, pageSize);

        [HttpPost("CreateAuditPlan")]
        public async Task<IActionResult> CreateAuditPlan(AuditPlanViewModel createAuditPlanViewModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validator.Validate(createAuditPlanViewModel);
                if (result.IsValid)
                {
                    await _auditPlanService.CreateAuditPlanAsync(createAuditPlanViewModel);
                }
                else
                {
                    return BadRequest("Fail to create new AuditPlan");
                }
            }
            return Ok("Create new AuditPlan Success");
        }

        [HttpPut("UpdateAuditPlan/{AuditPlanId}")]
        public async Task<IActionResult> UpdateAuditPlan(Guid AuditPlanId, UpdateAuditPlanViewModel updateAuditPlanView)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validator1.Validate(updateAuditPlanView);
                if (result.IsValid)
                {
                    await _auditPlanService.UpdateAuditPlanAsync(AuditPlanId, updateAuditPlanView);
                }
                else
                {
                    return BadRequest("Update AuditPlan Fail");
                }
            }
            return Ok("Update AuditPlan Success");
        }

        [HttpPost("AuditPlan/AddUser/{AuditPlanId}/{UserId}")]
        public async Task<IActionResult> AddUser(Guid AuditPlanId, Guid UserId)
        {
            if (ModelState.IsValid)
            {
                await _auditPlanService.AddUserToAuditPlan(AuditPlanId, UserId);
                return Ok("Add Success");
            }
            return BadRequest("Add User Fail");
        }

        [HttpDelete("AuditPlan/DeleteUser/{AuditPlanId}/{UserId}")]
        public async Task<IActionResult> DeleteUser(Guid AuditPlanId, Guid UserId)
        {
            if (ModelState.IsValid)
            {
                await _auditPlanService.RemoveUserToAuditPlan(AuditPlanId, UserId);
                return Ok("Remove Success");
            }
            return BadRequest("Remove User Fail");
        }
        [HttpGet("GetAllUserAuditPlan")]
        public async Task<Pagination<UserAuditPlanViewModel>> GetAllUserAuditPlan(int pageIndex = 0, int pageSize = 10)
        {
            return await _auditPlanService.GetAllUserAuditPlanAsync(pageIndex, pageSize);
        }
    }
}
