using Application.ViewModels.TrainingProgramModels;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.TrainingProgramModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    [EnableCors("AllowAll")]
    public class TrainingProgramController : ControllerBase
    {
        private readonly ITrainingProgramService _trainingProgramService;
        private readonly IValidator<CreateTrainingProgramViewModel> _validatorCreate;
        private readonly IValidator<UpdateTrainingProgramViewModel> _validatorUpdate;

        public TrainingProgramController(ITrainingProgramService trainingProgramService, IValidator<CreateTrainingProgramViewModel> validatorCreate, IValidator<UpdateTrainingProgramViewModel> validatorUpdate)
        {
            _trainingProgramService = trainingProgramService;
            _validatorCreate = validatorCreate;
            _validatorUpdate = validatorUpdate;
        }

        [HttpPost("CreateTrainingProgram")]
        [Authorize(policy: "Admins")]
        public async Task<Response> CreateTrainingProgram(CreateTrainingProgramViewModel CreateTrainingProgram)
        {
            if (ModelState.IsValid)
            {
                ValidationResult trainingprogram = _validatorCreate.Validate(CreateTrainingProgram);
                if (trainingprogram.IsValid)
                {
                    var result = await _trainingProgramService.CreateTrainingProgramAsync(CreateTrainingProgram);
                    return new Response(HttpStatusCode.OK, "Create TrainingProgram Succeed", result);
                }
            }
            return new Response(HttpStatusCode.BadRequest, "Invalid Input");
        }

        [HttpPut("UpdateTrainingProgram/{TrainingProgramId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> UpdateTrainingProgram(Guid TrainingProgramId, UpdateTrainingProgramViewModel UpdateTrainingProgram)
        {
            if (ModelState.IsValid)
            {
                ValidationResult trainingprogram = _validatorUpdate.Validate(UpdateTrainingProgram);
                if (trainingprogram.IsValid)
                {
                    var result = await _trainingProgramService.UpdateTrainingProgramAsync(TrainingProgramId, UpdateTrainingProgram);
                    return result switch
                    {
                        not null => Ok("Update TrainingProgram Success"),
                        _ => BadRequest("Update TrainingProgram Fail")
                    };
                }
            }
            return BadRequest("Update TrainingProgram Fail");
        }

        [HttpPatch("UpdateStatusOnlyOfTrainingProgram/{TrainingProgramId}")]
        [Authorize(policy: "Admins")]
        public async Task<Response> UpdateStatusOnlyOfTrainingProgram(Guid TrainingProgramId, UpdateStatusOnlyOfTrainingProgram trainingProgramModel) => await _trainingProgramService.UpdateStatusOnlyOfTrainingProgram(TrainingProgramId, trainingProgramModel);

        [HttpGet("GetTrainingProgramDetails/{TrainingProgramId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetSyllabusDetailById(Guid TrainingProgramId) => await _trainingProgramService.GetTrainingProgramDetails(TrainingProgramId);

        [HttpGet("GetAllTrainingProgram")]
        [Authorize(policy: "All")]
        public async Task<Response> ViewAllTrainingProgram(int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.ViewAllTrainingProgramAsync(pageIndex, pageSize);

        [HttpGet("GetTrainingProgramDisable")]
        [Authorize(policy: "Admins")]
        public async Task<Response> ViewTrainingProgramDisable(int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.ViewTrainingProgramDisableAsync(pageIndex, pageSize);

        [HttpGet("GetTrainingProgramEnable")]
        [Authorize(policy: "Admins")]
        public async Task<Response> ViewTrainingProgramEnable(int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.ViewTrainingProgramEnableAsync(pageIndex, pageSize);

        [HttpGet("GetTrainingProgramById/{TrainingProgramId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetTrainingProgramById(Guid TrainingProgramId) => await _trainingProgramService.GetTrainingProgramById(TrainingProgramId);
        [HttpGet("GetTrainingProgramByClassId/{ClassId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetTrainingProgramByClassId(Guid ClassId, int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.GetTrainingProgramByClassId(ClassId, pageIndex, pageSize);

        [HttpPost("AddTrainingProgramSyllabus/{SyllabusId}/{TrainingProgramId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> AddSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId)
        {
            if (ModelState.IsValid)
            {
                var result = await _trainingProgramService.AddSyllabusToTrainingProgram(SyllabusId, TrainingProgramId);
                if (result == null)
                {
                    return BadRequest("Add Syllabus to TrainingProgram Fail");
                }
            }
            return Ok("Add Syllabus to TrainingProgram Success");
        }

        [HttpDelete("DeleteTrainingProgramSyllabus/{SyllabusId}/{TrainingProgramId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> DeleteTrainingProgram(Guid SyllabusId, Guid TrainingProgramId)
        {
            if (ModelState.IsValid)
            {
                var result = await _trainingProgramService.RemoveSyllabusToTrainingProgram(SyllabusId, TrainingProgramId);
                if (result == null)
                {
                    return BadRequest("Remove Syllabus from TrainingProgram Fail");
                }
            }
            return Ok("Remove Syllabus from TrainingProgram Success");
        }

        [HttpGet("GetTrainingProgramByName/{trainingProgramName}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetTrainingProgramByName(string trainingProgramName, int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.GetByName(trainingProgramName, pageIndex, pageSize);
    }
}
