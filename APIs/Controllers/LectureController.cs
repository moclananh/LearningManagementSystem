using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.LectureViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Applications.Commons;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly ILectureServices _lectureServices;
        private readonly IValidator<CreateLectureViewModel> _validatorCreate;
        private readonly IValidator<UpdateLectureViewModel> _validatorUpdate;
        public LectureController(ILectureServices lectureServices,
            IValidator<CreateLectureViewModel> validatorCreate,
            IValidator<UpdateLectureViewModel> validatorUpdate)
        {
            _lectureServices = lectureServices;
            _validatorCreate = validatorCreate;
            _validatorUpdate = validatorUpdate;
        }

        [HttpPost("CreateLecture")]
        public async Task<IActionResult> CreateLecture(CreateLectureViewModel LectureModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorCreate.Validate(LectureModel);
                if (result.IsValid)
                {
                    await _lectureServices.CreateLecture(LectureModel);
                }
                else
                {
                    return BadRequest("Fail to create new Lecture");
                }
            }
            return Ok("Create new Lecture Success");
        }
        [HttpGet("GetLecturePagingsion")]
        public async Task<Pagination<LectureViewModel>> GetLecturePagingsion(int pageIndex = 0, int pageSize = 10)
        {
            return await _lectureServices.GetLecturePagingsionAsync(pageIndex, pageSize);
        }

        [HttpGet("GetLectureById/{LectureId}")]
        public async Task<LectureViewModel> GetLectureById(Guid LectureId) => await _lectureServices.GetLectureById(LectureId);

        [HttpGet("GetLectureByUnitId/{UnitId}")]
        public async Task<List<LectureViewModel>> GetLectureByUnitId(Guid UnitId) => await _lectureServices.GetLectureByUnitId(UnitId);
        [HttpGet("GetLectureByName/{LectureName}")]
        public async Task<List<LectureViewModel>> GetLectureByName(string LectureName) => await _lectureServices.GetLectureByName(LectureName);

        [HttpGet("GetEnableLectures")]
        public async Task<List<LectureViewModel>> GetEnableLectures() => await _lectureServices.GetEnableLectures();

        [HttpGet("GetDisableLectures")]
        public async Task<List<LectureViewModel>> GetDisableLectures() => await _lectureServices.GetDisableLectures();

        [HttpPut("UpdateLecture/{LectureId}")]
        public async Task<IActionResult> UpdateLecture(Guid LectureId, UpdateLectureViewModel Lecture)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorUpdate.Validate(Lecture);
                if (result.IsValid)
                {
                    await _lectureServices.UpdateLecture(LectureId, Lecture);
                }
                else
                {
                    return BadRequest("Update Lecture Fail");
                }
            }
            return Ok("Update Lecture Success");
        }
    }
}
