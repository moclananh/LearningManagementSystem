using Applications.Interfaces;
using Applications.ViewModels.LectureViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Cors;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    [EnableCors("AllowAll")]
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
        [Authorize(policy: "Admins")]
        public async Task<Response> CreateLecture(CreateLectureViewModel LectureModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorCreate.Validate(LectureModel);
                if (result.IsValid)
                {
                    var Lecture = await _lectureServices.CreateLecture(LectureModel);
                    return new Response(HttpStatusCode.OK, "Create Lecture Succeed", Lecture);
                }
            }
            else
            {
                return new Response(HttpStatusCode.BadRequest, "Create Failed, Invalid input");
            }
            return new Response(HttpStatusCode.BadRequest, "Invalid Input");
        }

        [HttpGet("GetAllLectures")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAllLectures(int pageIndex = 0, int pageSize = 10) => await _lectureServices.GetAllLectures(pageIndex, pageSize);

        [HttpGet("GetLectureById/{LectureId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetLectureById(Guid LectureId) => await _lectureServices.GetLectureById(LectureId);

        [HttpGet("GetLectureByUnitId/{UnitId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetLectureByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10) => await _lectureServices.GetLectureByUnitId(UnitId, pageIndex, pageSize);

        [HttpGet("GetLectureByName/{LectureName}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetLectureByName(string LectureName, int pageIndex = 0, int pageSize = 10) => await _lectureServices.GetLectureByName(LectureName, pageIndex, pageSize);

        [HttpGet("GetEnableLectures")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetEnableLectures(int pageIndex = 0, int pageSize = 10) => await _lectureServices.GetEnableLectures(pageIndex, pageSize);

        [HttpGet("GetDisableLectures")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetDisableLectures(int pageIndex = 0, int pageSize = 10) => await _lectureServices.GetDisableLectures(pageIndex, pageSize);

        [HttpPut("UpdateLecture/{LectureId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> UpdateLecture(Guid LectureId, UpdateLectureViewModel Lecture)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorUpdate.Validate(Lecture);
                if (result.IsValid)
                {
                    if (await _lectureServices.UpdateLecture(LectureId, Lecture) != null)
                    {
                        return Ok("Update Lecture Success");
                    }
                    return BadRequest("Invalid Id");
                }
            }
            return BadRequest("Update Failed,Invalid Input Information");
        }
    }
}
