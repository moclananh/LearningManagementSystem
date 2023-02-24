using Applications.ViewModels.AssignmentViewModels;

namespace Applications.Interfaces
{
    public interface IAssignmentService
    {
        public Task<List<UpdateAssignmentViewModel>> GetEnableAssignments();
        public Task<List<UpdateAssignmentViewModel>> GetDisableAssignments();
        public Task<UpdateAssignmentViewModel?> UpdateAssignment(Guid AssignmentId, UpdateAssignmentViewModel assignmentDTO);
        public Task<UpdateAssignmentViewModel> GetAssignmentById(Guid AssignmentId);
        public Task<List<UpdateAssignmentViewModel>> GetAssignmentByUnitId(Guid UnitId);
        public Task<List<AssignmentViewModel>> ViewAllAssignmentAsync();
        public Task<CreateAssignmentViewModel> CreateAssignmentAsync(CreateAssignmentViewModel AssignmentDTO);
        public Task<List<UpdateAssignmentViewModel>> GetAssignmentByName(string Name);
    }
}
