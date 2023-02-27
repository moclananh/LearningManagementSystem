using Applications.Commons;
using Applications.ViewModels.LectureViewModels;

namespace Applications.Interfaces
{
    public interface ILectureService
    {
        public Task<Pagination<LectureViewModel>> GetAllLectures(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<LectureViewModel>> GetEnableLectures(int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<LectureViewModel>> GetDisableLectures(int pageIndex = 0, int pageSize = 10);
        public Task<LectureViewModel> GetLectureById(Guid LectureId);
        public Task<Pagination<LectureViewModel>> GetLectureByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10);
        public Task<Pagination<LectureViewModel>> GetLectureByName(string Name, int pageIndex = 0, int pageSize = 10);
        public Task<CreateLectureViewModel?> CreateLecture(CreateLectureViewModel lectureDTO);
        public Task<UpdateLectureViewModel?> UpdateLecture(Guid LectureId, UpdateLectureViewModel lectureDTO);
    }
}
