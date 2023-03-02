using Applications.Commons;
using Applications.ViewModels.LectureViewModels;
using Applications.ViewModels.Response;

namespace Applications.Interfaces
{
    public interface ILectureService
    {
        public Task<Response> GetAllLectures(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetEnableLectures(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetDisableLectures(int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetLectureById(Guid LectureId);
        public Task<Response> GetLectureByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10);
        public Task<Response> GetLectureByName(string Name, int pageIndex = 0, int pageSize = 10);
        public Task<CreateLectureViewModel?> CreateLecture(CreateLectureViewModel lectureDTO);
        public Task<UpdateLectureViewModel?> UpdateLecture(Guid LectureId, UpdateLectureViewModel lectureDTO);
    }
}
