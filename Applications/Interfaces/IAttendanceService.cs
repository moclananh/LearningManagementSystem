using Applications.ViewModels.AssignmentViewModels;
using Applications.ViewModels.Response;
namespace Applications.Interfaces
{
    public interface IAttendanceService 
    {
        public Task<Response> CreateAttendanceAsync(Guid ClassId);
        public Task<Response?> CheckAttendance(string ClassCode, string Email);
    }
}
