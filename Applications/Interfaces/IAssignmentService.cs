using Applications.ViewModels.AssignmentViewModels;
using Applications.ViewModels.Response;

namespace Applications.Interfaces
{
    public interface IAssignmentService
    {
        public Task<Response> GetEnableAssignments(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetDisableAssignments(int pageIndex = 0, int pageSize = 10);
        public Task<UpdateAssignmentViewModel?> UpdateAssignment(Guid AssignmentId, UpdateAssignmentViewModel assignmentDTO);
        public Task<Response> GetAssignmentById(Guid AssignmentId);
        public Task<Response> GetAssignmentByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10);
        public Task<Response> ViewAllAssignmentAsync(int pageIndex = 0, int pageSize = 10);
        public Task<CreateAssignmentViewModel> CreateAssignmentAsync(CreateAssignmentViewModel AssignmentDTO);
        public Task<Response> GetAssignmentByName(string Name, int pageIndex = 0, int pageSize = 10);
    }
}
