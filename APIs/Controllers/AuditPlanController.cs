using Applications.Interfaces;
using Applications.ViewModels.AuditPlanViewModel;
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
        public async Task<List<AuditPlanViewModel>> GetAllQuizzAsync() => await _auditPlanService.GetAllAuditPlanAsync();

        [HttpGet("GetEnableAuditPlan")]
        public async Task<List<AuditPlanViewModel>> GetEnableClasses() => await _auditPlanService.GetEnableAuditPlanAsync();

        [HttpGet("GetDisableAuditPlan")]
        public async Task<List<AuditPlanViewModel>> GetDiableClasses() => await _auditPlanService.GetDisableAuditPlanAsync();

        [HttpGet("GetAuditPlanById/{AuditPlanId}")]
        public async Task<AuditPlanViewModel> GetAuditPlanByIdAsync(Guid AuditPlanId) => await _auditPlanService.GetAuditPlanByIdAsync(AuditPlanId);

        [HttpGet("GetAuditPlanByModuleId/{ModuleId}")]
        public async Task<List<AuditPlanViewModel>> GetAuditPlanByModuleId(Guid ModuleId) => await _auditPlanService.GetAuditPlanByModuleIdAsync(ModuleId);

        [HttpGet("GetAuditPlanByClassId/{ClassId}")]
        public async Task<List<AuditPlanViewModel>> GetAuditPlanByClassId(Guid ClassId) => await _auditPlanService.GetAuditPlanbyClassIdAsync(ClassId);

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
    }
}
