using Applications.Interfaces;
using Applications.ViewModels.AssignmentViewModels;
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
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly IValidator<CreateAssignmentViewModel> _validatorCreate;
        private readonly IValidator<UpdateAssignmentViewModel> _validatorUpdate;

        public AssignmentController(IAssignmentService assignmentServices,
             IValidator<CreateAssignmentViewModel> validatorCreate,
             IValidator<UpdateAssignmentViewModel> validatorUpdate)
        {
            _assignmentService = assignmentServices;
            _validatorCreate = validatorCreate;
            _validatorUpdate = validatorUpdate;
        }

        [HttpGet("GetAllAssignment")]
        [Authorize(policy: "All")]
        public async Task<Response> ViewAllAssignmentAsync(int pageIndex = 0, int pageSize = 10) => await _assignmentService.ViewAllAssignmentAsync(pageIndex, pageSize);

        [HttpPost("CreateAssignment")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> CreateAssignment(CreateAssignmentViewModel AssignmentModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorCreate.Validate(AssignmentModel);
                if (result.IsValid)
                {
                    await _assignmentService.CreateAssignmentAsync(AssignmentModel);
                }
                else
                {
                    var error = result.Errors.Select(x => x.ErrorMessage).ToList();
                    return BadRequest(error);
                }
            }
            return Ok("Create new Assignment Success");
        }

        [HttpGet("GetEnableAssignments")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetEnableAssignments(int pageIndex = 0, int pageSize = 10) => await _assignmentService.GetEnableAssignments(pageIndex, pageSize);

        [HttpGet("GetDisableAssignments")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetDiableAssignments(int pageIndex = 0, int pageSize = 10) => await _assignmentService.GetDisableAssignments(pageIndex, pageSize);

        [HttpGet("ViewAssignmentById/{AssignmentId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAssignmentById(Guid AssignmentId) => await _assignmentService.GetAssignmentById(AssignmentId);

        [HttpGet("ViewAssignmentsByUnitId/{UnitId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAssignmentsByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10) => await _assignmentService.GetAssignmentByUnitId(UnitId, pageIndex, pageSize);

        [HttpPut("UpdateAssignment/{AssignmentId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult?> UpdateAssignment(Guid AssignmentId, UpdateAssignmentViewModel assignmentDTO)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorUpdate.Validate(assignmentDTO);
                if (result.IsValid)
                {
                    if (await _assignmentService.UpdateAssignment(AssignmentId, assignmentDTO) is object)
                    {
                        return Ok("Update Assignment Success");
                    }   
                    return BadRequest("Invalid Id");
                }
            }
            return BadRequest("Update Failed,Invalid Input Information");
        }

        [HttpGet("GetAssignmentByName/{AssignmentName}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAssignmentByName(string AssignmentName, int pageIndex = 0, int pageSize = 10) => await _assignmentService.GetAssignmentByName(AssignmentName, pageIndex, pageSize);
    }
}
