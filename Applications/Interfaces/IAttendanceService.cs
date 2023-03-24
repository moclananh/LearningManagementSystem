using Applications.Commons;
using Applications.ViewModels.AttendanceViewModels;
using Applications.ViewModels.Response;
using Domain.Enum.AttendenceEnum;

namespace Applications.Interfaces
{
    public interface IAttendanceService 
    {
        public Task<Response> CreateAttendanceAsync(Guid ClassId);
        public Task<Response?> CheckAttendance(string ClassCode, string Email);
        Task<byte[]> ExportAttendanceByClassCodeandDate(string ClassCode, DateTime Date);
        public Task<Response?> UpdateAttendance(DateTime Date, string ClassCode, string Email , AttendenceStatus Status);
        public Task<Response> GetAttendanceByFilter(AttendanceFilterViewModel filters, int pageNumber = 0, int pageSize = 10);
    }
}
