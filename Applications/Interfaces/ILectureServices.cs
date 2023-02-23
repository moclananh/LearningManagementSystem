using Applications.ViewModels.LectureViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces
{
    public interface ILectureServices
    {
        public Task<List<LectureViewModel>> GetAllLectures();
        public Task<List<LectureViewModel>> GetEnableLectures();
        public Task<List<LectureViewModel>> GetDisableLectures();
        public Task<LectureViewModel> GetLectureById(Guid LectureId);
        public Task<List<LectureViewModel>> GetLectureByUnitId(Guid UnitId);
        public Task<List<LectureViewModel>> GetLectureByName(string Name);
        public Task<CreateLectureViewModel?> CreateLecture(CreateLectureViewModel lectureDTO);
        public Task<UpdateLectureViewModel?> UpdateLecture(Guid LectureId, UpdateLectureViewModel lectureDTO);
    }
}
