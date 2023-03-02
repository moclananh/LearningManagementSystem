using Application.ViewModels.TrainingProgramModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.TrainingProgramModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> CreateTrainingProgram(CreateTrainingProgramViewModel CreateTrainingProgram)
        {
            if (ModelState.IsValid)
            {
                ValidationResult trainingprogram = _validatorCreate.Validate(CreateTrainingProgram);
                if (trainingprogram.IsValid)
                {
                    await _trainingProgramService.CreateTrainingProgramAsync(CreateTrainingProgram);
                }
                else
                {
                    var error = trainingprogram.Errors.Select(x => x.ErrorMessage).ToList();
                    return BadRequest(error);
                }
            }
            return Ok("Create new TrainningProgram Success");
        }

        [HttpPut("UpdateTrainingProgram/{TrainingProgramId}")]
        public async Task<IActionResult> UpdateTrainingProgram(Guid TrainingProgramId, UpdateTrainingProgramViewModel UpdateTrainingProgram)
        {
            if (ModelState.IsValid)
            {
                ValidationResult trainingprogram = _validatorUpdate.Validate(UpdateTrainingProgram);
                if (trainingprogram.IsValid)
                {
                    await _trainingProgramService.UpdateTrainingProgramAsync(TrainingProgramId, UpdateTrainingProgram);
                }
                else
                {
                    var error = trainingprogram.Errors.Select(x => x.ErrorMessage).ToList();
                    return BadRequest(error);
                }
            }
            return Ok("Update TrainingProgram Success");
        }

        [HttpGet("GetAllTrainingProgram")]
        public async Task<Pagination<TrainingProgramViewModel>> ViewAllTrainingProgram(int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.ViewAllTrainingProgramAsync(pageIndex, pageSize);

        [HttpGet("GetTrainingProgramDisable")]
        public async Task<Pagination<TrainingProgramViewModel>> ViewTrainingProgramDisable(int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.ViewTrainingProgramDisableAsync(pageIndex, pageSize);

        [HttpGet("GetTrainingProgramEnable")]
        public async Task<Pagination<TrainingProgramViewModel>> ViewTrainingProgramEnable(int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.ViewTrainingProgramEnableAsync(pageIndex, pageSize);

        [HttpGet("GetTrainingProgramById/{TrainingProgramId}")]
        public async Task<TrainingProgramViewModel> GetTrainingProgramById(Guid TrainingProgramId) => await _trainingProgramService.GetTrainingProgramById(TrainingProgramId);
        [HttpGet("GetTrainingProgramByClassId/{ClassId}")]
        public async Task<Pagination<TrainingProgramViewModel>> GetTrainingProgramByClassId(Guid ClassId, int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.GetTrainingProgramByClassId(ClassId, pageIndex, pageSize);

        [HttpPost("AddTrainingProgramSyllabus/{SyllabusId}/{TrainingProgramId}")]
        public async Task<IActionResult> AddSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId)
        {
            if (ModelState.IsValid)
            {
                await _trainingProgramService.AddSyllabusToTrainingProgram(SyllabusId, TrainingProgramId);
                return Ok("Add Syllabus to TrainingProgram Success");
            }
            return BadRequest("Add Syllabus to TrainingProgram Fail");
        }

        [HttpDelete("DeleteTrainingProgramSyllabus/{SyllabusId}/{TrainingProgramId}")]
        public async Task<IActionResult> DeleteTrainingProgram(Guid SyllabusId, Guid TrainingProgramId)
        {
            if (ModelState.IsValid)
            {
                await _trainingProgramService.RemoveSyllabusToTrainingProgram(SyllabusId, TrainingProgramId);
                return Ok("Remove Syllabus from TrainingProgram Success");
            }
            return BadRequest("Remove Syllabus from TrainingProgram Fail");
        }
    }
}
