using Applications.Interfaces;
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

        [HttpGet("GetEnableAssignments")]
        public async Task<List<UpdateAssignmentViewModel>> GetEnableAssignments() => await _assignmentService.GetEnableAssignments();

        [HttpGet("GetDisableAssignments")]
        public async Task<List<UpdateAssignmentViewModel>> GetDiableAssignments() => await _assignmentService.GetDisableAssignments();

        [HttpGet("ViewAssignmentById/{AssignmentId}")]
        public async Task<UpdateAssignmentViewModel> GetAssignmentById(Guid AssignmentId) => await _assignmentService.GetAssignmentById(AssignmentId);

        [HttpGet("ViewAssignmentsByUnitId/{UnitId}")]
        public async Task<List<UpdateAssignmentViewModel>> GetAssignmentsByUnitId(Guid UnitId) => await _assignmentService.GetAssignmentByUnitId(UnitId);

        [HttpPut("UpdateAssignment/{AssignmentId}")]
        public async Task<UpdateAssignmentViewModel?> UpdateAssignment(Guid AssignmentId, UpdateAssignmentViewModel assignmentDTO) => await _assignmentService.UpdateAssignment(AssignmentId, assignmentDTO);
    }
}
