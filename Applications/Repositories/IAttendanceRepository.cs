using Domain.Entities;

namespace Applications.Repositories
{
    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
        Task AddListAttendanceAsync(Guid ClassId, DateTime startDate, DateTime enddate);
        Task<Attendance> GetSingleAttendance(Guid ClassId, Guid UserId);
        Task<List<Attendance>> GetListAttendances(string ClassCode, DateTime Date);
        Task<Attendance> GetSingleAttendanceForUpdate(DateTime Date, Guid ClassId, Guid UserId);
        Task<List<Attendance>> GetAbsentId();
    }
}
