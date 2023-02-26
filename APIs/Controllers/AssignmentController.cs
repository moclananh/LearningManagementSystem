using Application.ViewModels.QuizzViewModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.AssignmentViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        public AssignmentController(IAssignmentService assignmentServices)
        {
            _assignmentService = assignmentServices;
        }
        [HttpGet("GetAllAssignment")]
        public async Task<Pagination<AssignmentViewModel>> ViewAllAssignmentAsync(int pageIndex = 0, int pageSize = 10) => await _assignmentService.ViewAllAssignmentAsync(pageIndex, pageSize);

        [HttpPost("CreateAssignment")]
        public async Task<CreateAssignmentViewModel> CreateAssignment(CreateAssignmentViewModel AssignmentModel) => await _assignmentService.CreateAssignmentAsync(AssignmentModel);

        [HttpGet("GetEnableAssignments")]
        public async Task<Pagination<UpdateAssignmentViewModel>> GetEnableAssignments(int pageIndex = 0, int pageSize = 10) => await _assignmentService.GetEnableAssignments(pageIndex, pageSize);

        [HttpGet("GetDisableAssignments")]
        public async Task<Pagination<UpdateAssignmentViewModel>> GetDiableAssignments(int pageIndex = 0, int pageSize = 10) => await _assignmentService.GetDisableAssignments(pageIndex, pageSize);

        [HttpGet("ViewAssignmentById/{AssignmentId}")]
        public async Task<UpdateAssignmentViewModel> GetAssignmentById(Guid AssignmentId) => await _assignmentService.GetAssignmentById(AssignmentId);

        [HttpGet("ViewAssignmentsByUnitId/{UnitId}")]
        public async Task<Pagination<UpdateAssignmentViewModel>> GetAssignmentsByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10) => await _assignmentService.GetAssignmentByUnitId(UnitId, pageIndex, pageSize);

        [HttpPut("UpdateAssignment/{AssignmentId}")]
        public async Task<UpdateAssignmentViewModel?> UpdateAssignment(Guid AssignmentId, UpdateAssignmentViewModel assignmentDTO) => await _assignmentService.UpdateAssignment(AssignmentId, assignmentDTO);

        [HttpGet("GetAssignmentByName/{AssignmentName}")]
        public async Task<Pagination<UpdateAssignmentViewModel>> GetAssignmentByName(string AssignmentName, int pageIndex = 0, int pageSize = 10) => await _assignmentService.GetAssignmentByName(AssignmentName, pageIndex, pageSize);
    }
}
