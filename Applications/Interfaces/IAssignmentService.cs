using Applications.Commons;
using Applications.ViewModels.AssignmentViewModels;

namespace Applications.Interfaces
{
    public interface IAssignmentService
    {
        public Task<Pagination<UpdateAssignmentViewModel>> GetEnableAssignments(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<UpdateAssignmentViewModel>> GetDisableAssignments(int pageIndex = 0, int pageSize = 10);
        public Task<UpdateAssignmentViewModel?> UpdateAssignment(Guid AssignmentId, UpdateAssignmentViewModel assignmentDTO);
        public Task<UpdateAssignmentViewModel> GetAssignmentById(Guid AssignmentId);
        public Task<Pagination<UpdateAssignmentViewModel>> GetAssignmentByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<AssignmentViewModel>> ViewAllAssignmentAsync(int pageIndex = 0, int pageSize = 10);
        public Task<CreateAssignmentViewModel> CreateAssignmentAsync(CreateAssignmentViewModel AssignmentDTO);
        public Task<Pagination<UpdateAssignmentViewModel>> GetAssignmentByName(string Name, int pageIndex = 0, int pageSize = 10);
    }
}
