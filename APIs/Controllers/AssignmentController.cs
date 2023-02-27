using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.AssignmentViewModels;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IValidator<CreateAssignmentViewModel> _validator;
        private readonly IValidator<UpdateAssignmentViewModel> _validator1;

        public AssignmentController(IAssignmentService assignmentServices,
             IValidator<CreateAssignmentViewModel> validator,
             IValidator<UpdateAssignmentViewModel> validator1)
        {
            _assignmentService = assignmentServices;
            _validator = validator;
            _validator1 = validator1;
        }
        [HttpGet("GetAllAssignment")]
        public async Task<Pagination<AssignmentViewModel>> ViewAllAssignmentAsync(int pageIndex = 0, int pageSize = 10) => await _assignmentService.ViewAllAssignmentAsync(pageIndex, pageSize);

        [HttpPost("CreateAssignment")]
        public async Task<IActionResult> CreateAssignment(CreateAssignmentViewModel AssignmentModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validator.Validate(AssignmentModel);
                if (result.IsValid)
                {
                    await _assignmentService.CreateAssignmentAsync(AssignmentModel);
                }
                else
                {
                    return BadRequest("Fail to create new Assignment");
                }
            }
            return Ok("Create new Assignment Success");
        }

        [HttpGet("GetEnableAssignments")]
        public async Task<Pagination<UpdateAssignmentViewModel>> GetEnableAssignments(int pageIndex = 0, int pageSize = 10) => await _assignmentService.GetEnableAssignments(pageIndex, pageSize);

        [HttpGet("GetDisableAssignments")]
        public async Task<Pagination<UpdateAssignmentViewModel>> GetDiableAssignments(int pageIndex = 0, int pageSize = 10) => await _assignmentService.GetDisableAssignments(pageIndex, pageSize);

        [HttpGet("ViewAssignmentById/{AssignmentId}")]
        public async Task<UpdateAssignmentViewModel> GetAssignmentById(Guid AssignmentId) => await _assignmentService.GetAssignmentById(AssignmentId);

        [HttpGet("ViewAssignmentsByUnitId/{UnitId}")]
        public async Task<Pagination<UpdateAssignmentViewModel>> GetAssignmentsByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10) => await _assignmentService.GetAssignmentByUnitId(UnitId, pageIndex, pageSize);

        [HttpPut("UpdateAssignment/{AssignmentId}")]
        public async Task<IActionResult?> UpdateAssignment(Guid AssignmentId, UpdateAssignmentViewModel assignmentDTO)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validator1.Validate(assignmentDTO);
                if (result.IsValid)
                {
                    await _assignmentService.UpdateAssignment(AssignmentId, assignmentDTO);
                }
                else
                {
                    return BadRequest("Update Assignment Fail");
                }
            }
            return Ok("Update Assignment Success");
        }

        [HttpGet("GetAssignmentByName/{AssignmentName}")]
        public async Task<Pagination<UpdateAssignmentViewModel>> GetAssignmentByName(string AssignmentName, int pageIndex = 0, int pageSize = 10) => await _assignmentService.GetAssignmentByName(AssignmentName, pageIndex, pageSize);
    }
}
