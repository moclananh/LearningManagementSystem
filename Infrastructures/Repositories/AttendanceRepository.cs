using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructures.Repositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        private readonly AppDBContext _dbContext;
        private readonly IClaimService _claimService;
        public AttendanceRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
            _claimService = claimService;
        }

        public async Task AddListAttendanceAsync(Guid ClassId, DateTime startDate, DateTime enddate)
        {
            DateTime stDay = Convert.ToDateTime(startDate);
            DateTime edDay = Convert.ToDateTime(enddate);
            TimeSpan Time = edDay - stDay;
            int TotalDays = Time.Days;
            int j = 0;
            DateTime DateATD;
            for (int i = 0; i < 6; i++)
            {
                if (j > TotalDays)
                {
                    break;
                }

                foreach (var item in _dbContext.ClassUser.Where(x => x.ClassId == ClassId))
                {
                    if (i == 5)
                    {
                        j = j + 3;
                        i = 0;
                        DateATD = startDate.AddDays(j - 1);
                    }

                    else if (startDate.DayOfWeek == DayOfWeek.Tuesday && j < 1)
                    {
                        i = i + 1;
                        j = j + 1;
                        DateATD = startDate;
                    }

                    else if (startDate.DayOfWeek == DayOfWeek.Wednesday && j < 1)
                    {
                        i = i + 2;
                        j = j + 1;
                        DateATD = startDate;
                    }

                    else if (startDate.DayOfWeek == DayOfWeek.Thursday && j < 1)
                    {
                        i = i + 3;
                        j = j + 1;
                        DateATD = startDate;
                    }


                    else if (startDate.DayOfWeek == DayOfWeek.Friday && j < 1)
                    {
                        i = i + 4;
                        j = j + 1;
                        DateATD = startDate;
                    }

                    else if (startDate.DayOfWeek == DayOfWeek.Monday && j < 1)
                    {
                        i = i;
                        j = j + 1;
                        DateATD = startDate;
                    }
                    else
                    {
                        DateATD = startDate.AddDays(j);
                        j = j + 1;

                    }
                    var Atd = new Attendance();
                    Atd.CreationDate = DateTime.UtcNow;
                    Atd.CreatedBy = _claimService.GetCurrentUserId;
                    Atd.ClassId = ClassId;
                    Atd.UserId = item.UserId;
                    Atd.Status = Domain.Enum.AttendenceEnum.AttendenceStatus.Absent;
                    Atd.Date = DateATD;
                    Atd.Note = string.Empty;
                    Atd.IsDeleted = false;
                    await _dbContext.AddAsync(Atd);
                }
            }
        }

        public async Task<List<Attendance>> GetListAttendances(string ClassCode, DateTime Date)
        {
            return await _dbContext.Attendances.Where(x => x.Date == Date && x.Class.ClassCode == ClassCode).Include(a => a.Class).Include(a => a.User).ToListAsync();
        }

        public async Task<Attendance> GetSingleAttendance(Guid ClassId, Guid UserId)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Date.Date == DateTime.Today.Date && x.ClassId == ClassId && x.UserId == UserId);
            return result;
        }

        public async Task<Attendance> GetSingleAttendanceForUpdate(DateTime Date, Guid ClassId, Guid UserId)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Date == Date && x.ClassId == ClassId && x.UserId == UserId);
            return result;
        }
        public async Task<List<Attendance>> GetAbsentId()
        {
            return await _dbContext.Attendances.Where(x => x.Date == DateTime.Today && x.Status == Domain.Enum.AttendenceEnum.AttendenceStatus.Absent).ToListAsync();
        }

    }
}