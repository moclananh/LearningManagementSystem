using Domain.Entities;

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
