using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class LectureRepository : GenericRepository<Lecture>, ILectureRepository
    {
        private readonly AppDBContext _dbContext;
        public LectureRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Lecture>> GetLectureByName(string Name) => await _dbContext.Lectures.Where(x => x.LectureName.Contains(Name)).ToListAsync();
        public async Task<List<Lecture>> GetLectureByUnitId(Guid UnitId) => await _dbContext.Lectures.Where(x => x.UnitId.Equals(UnitId)).ToListAsync();
        public async Task<List<Lecture>> GetDisableLectures() => await _dbContext.Lectures.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Disable).ToListAsync();
        public async Task<List<Lecture>> GetEnableLectures() => await _dbContext.Lectures.Where(x => x.Status == Domain.Enum.StatusEnum.Status.Enable).ToListAsync();
    }
}
