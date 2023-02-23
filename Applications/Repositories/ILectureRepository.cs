using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Repositories
{
    public interface ILectureRepository : IGenericRepository<Lecture>
    {
        Task<List<Lecture>> GetEnableLectures();
        Task<List<Lecture>> GetDisableLectures();
        Task<List<Lecture>> GetLectureByName(string Name);
        Task<List<Lecture>> GetLectureByUnitId(Guid UnitId);
    }
}
