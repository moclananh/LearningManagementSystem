using Applications.Commons;
using Domain.Entities;

namespace Applications.Repositories
{
    public interface ILectureRepository : IGenericRepository<Lecture>
    {
        Task<Pagination<Lecture>> GetEnableLectures(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Lecture>> GetDisableLectures(int pageNumber = 0, int pageSize = 10);
        Task<Pagination<Lecture>> GetLectureByName(string Name, int pageIndex = 0, int pageSize = 10);
        Task<Pagination<Lecture>> GetLectureByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10);
    }
}
