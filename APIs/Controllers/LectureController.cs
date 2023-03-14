using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.LectureViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Applications.Commons;
using System.Drawing.Printing;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    public class LectureController : ControllerBase
    {
        private readonly ILectureService _lectureServices;
        private readonly IValidator<CreateLectureViewModel> _validatorCreate;
        private readonly IValidator<UpdateLectureViewModel> _validatorUpdate;
        public LectureController(ILectureService lectureServices,
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
        [HttpGet("GetAllLectures")]
        public async Task<Response> GetAllLectures(int pageIndex = 0, int pageSize = 10) => await _lectureServices.GetAllLectures(pageIndex, pageSize);

        [HttpGet("GetLectureById/{LectureId}")]
        public async Task<Response> GetLectureById(Guid LectureId) => await _lectureServices.GetLectureById(LectureId);

        [HttpGet("GetLectureByUnitId/{UnitId}")]
        public async Task<Response> GetLectureByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10) => await _lectureServices.GetLectureByUnitId(UnitId, pageIndex, pageSize);
        [HttpGet("GetLectureByName/{LectureName}")]
        public async Task<Response> GetLectureByName(string LectureName, int pageIndex = 0, int pageSize = 10) => await _lectureServices.GetLectureByName(LectureName, pageIndex, pageSize);

        [HttpGet("GetEnableLectures")]
        public async Task<Response> GetEnableLectures(int pageIndex = 0, int pageSize = 10) => await _lectureServices.GetEnableLectures(pageIndex, pageSize);

        [HttpGet("GetDisableLectures")]
        public async Task<Response> GetDisableLectures(int pageIndex = 0, int pageSize = 10) => await _lectureServices.GetDisableLectures(pageIndex, pageSize);

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
