using Application.Interfaces;
using Applications.ViewModels.AuditResultViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    [EnableCors("AllowAll")]
    public class AuditResultController : ControllerBase
    {
        private readonly IAuditResultServices _service;
        private readonly IValidator<UpdateAuditResultViewModel> _updateValidator;

        public AuditResultController(IAuditResultServices service, IValidator<UpdateAuditResultViewModel> validator)
        {
            _service = service;
            _updateValidator = validator;
        }

        [HttpGet("GetByAuditPlanId/{AuditPlanId}")]
        [Authorize(policy: "All")]
        public async Task<AuditResultViewModel> GetByAuditPlanId(Guid AuditPlanId)
        {
            return await _service.GetByAudiPlanId(AuditPlanId);
        }

        [HttpGet("GetAuditResultById/{auditresultId}")]
        [Authorize(policy: "All")]
        public async Task<AuditResultViewModel> GetAuditResultById(Guid auditresultId)
        {
            return await _service.GetAuditResultById(auditresultId);
        }

        [HttpPut("UpdateAuditResult/{AuditResultId}")]
        [Authorize(policy: "AuditResults")]
        public async Task<IActionResult> UpdateAuditResult(Guid AuditResultId, UpdateAuditResultViewModel assignmentDTO)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _updateValidator.Validate(assignmentDTO);
                if (result.IsValid)
                {
                    if (await _service.UpdateAuditResult(AuditResultId, assignmentDTO) != null)
                    {
                        return Ok("Update AuditResult Success");
                    }
                    return BadRequest("Invalid AuditResult Id");
                }
            }
            return Ok("Update AuditResult Success");
        }
    }
}
