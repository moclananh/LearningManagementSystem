using Applications.Commons;
using Applications.ViewModels.LectureViewModels;

namespace Applications.Interfaces
{
    public interface ILectureService
    {
        public Task<List<LectureViewModel>> GetAllLectures();
        public Task<List<LectureViewModel>> GetEnableLectures();
        public Task<List<LectureViewModel>> GetDisableLectures();
        public Task<LectureViewModel> GetLectureById(Guid LectureId);
        public Task<List<LectureViewModel>> GetLectureByUnitId(Guid UnitId);
        public Task<List<LectureViewModel>> GetLectureByName(string Name);
        public Task<CreateLectureViewModel?> CreateLecture(CreateLectureViewModel lectureDTO);
        public Task<UpdateLectureViewModel?> UpdateLecture(Guid LectureId, UpdateLectureViewModel lectureDTO);
        public Task<Pagination<LectureViewModel>> GetLecturePagingsionAsync(int pageIndex = 0, int pageSize = 10);

    }
}
