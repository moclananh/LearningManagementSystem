﻿using Application.Interfaces;
using Applications.ViewModels.AuditResultViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    public class AuditResultController : ControllerBase
    {
        private readonly IAuditResultServices _service;
        private readonly IValidator<UpdateAuditResultViewModel> _updateValidator;

        public AuditResultController(IAuditResultServices service, IValidator<UpdateAuditResultViewModel> validator)
        {
            _service = service;
            _updateValidator = validator;
        }

        [HttpGet("GetByAuditPlanId")]
        [Authorize(policy: "All")]
        public async Task<AuditResultViewModel> GetByAuditPlanId(Guid id)
        {
            return await _service.GetByAudiPlanId(id);
        }

        [HttpPut("UpdateAuditResult/{AuditResultId}")]
        [Authorize(policy: "OnlySupperAdmin, Auditor, Trainer, Mentor")]
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
