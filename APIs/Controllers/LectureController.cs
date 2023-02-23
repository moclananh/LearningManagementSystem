using Applications.Interfaces;
using Applications.ViewModels.LectureViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly ILectureServices _lectureServices;
        public LectureController(ILectureServices lectureServices)
        {
            _lectureServices = lectureServices;
        }

        [HttpPost("CreateLecture")]
        public async Task<CreateLectureViewModel> CreateLecture(CreateLectureViewModel LectureModel) => await _lectureServices.CreateLecture(LectureModel);

        [HttpGet("GetAllLectures")]
        public async Task<List<LectureViewModel>> GetAllLectures() => await _lectureServices.GetAllLectures();

        [HttpGet("GetLectureById/{LectureId}")]
        public async Task<LectureViewModel> GetLectureById(Guid LectureId) => await _lectureServices.GetLectureById(LectureId);

        [HttpGet("GetLectureByUnitId/{UnitId}")]
        public async Task<List<LectureViewModel>> GetLectureByUnitId(Guid UnitId) => await _lectureServices.GetLectureByUnitId(UnitId);

        [HttpGet("GetEnableLectures")]
        public async Task<List<LectureViewModel>> GetEnableLectures() => await _lectureServices.GetEnableLectures();

        [HttpGet("GetDisableLectures")]
        public async Task<List<LectureViewModel>> GetDisableLectures() => await _lectureServices.GetDisableLectures();

        [HttpPut("UpdateLecture/{LectureId}")]
        public async Task<UpdateLectureViewModel> UpdateLecture(Guid LectureId, UpdateLectureViewModel Lecutre) => await _lectureServices.UpdateLecture(LectureId, Lecutre);
    }
}
