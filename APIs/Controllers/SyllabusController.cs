using Applications.Interfaces;
using Applications.ViewModels.SyllabusViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusController : ControllerBase
    {
        private readonly ISyllabusServices _syllabusServices;
        private readonly IValidator<CreateSyllabusViewModel> _validatorCreate;
        private readonly IValidator<UpdateSyllabusViewModel> _validatorUpdate;

        public SyllabusController(ISyllabusServices syllabusServices, IValidator<CreateSyllabusViewModel> validatorCreate, IValidator<UpdateSyllabusViewModel> validatorUpdate)
        {
            _syllabusServices = syllabusServices;
            _validatorCreate = validatorCreate;
            _validatorUpdate = validatorUpdate;
        }

        [HttpPost("CreateSyllabus")]
        public async Task<IActionResult> CreateSyllabus(CreateSyllabusViewModel SyllabusModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult syllabus = _validatorCreate.Validate(SyllabusModel);
                if (syllabus.IsValid)
                {
                    await _syllabusServices.CreateSyllabus(SyllabusModel);
                }
                else
                {
                    var error = syllabus.Errors.Select(x => x.ErrorMessage).ToList();
                    return BadRequest(error);
                }
            }
            return Ok("Create new Syllabus Success");
        }

        [HttpPut("UpdateSyllabus/{SyllabusId}")]
        public async Task<IActionResult> UpdateSyllabus(Guid SyllabusId, UpdateSyllabusViewModel SyllabusModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult syllabus = _validatorUpdate.Validate(SyllabusModel);
                if (syllabus.IsValid)
                {
                    await _syllabusServices.UpdateSyllabus(SyllabusId, SyllabusModel);
                }
                else
                {
                    var error = syllabus.Errors.Select(x => x.ErrorMessage).ToList();
                    return BadRequest(error);
                }
            }
            return Ok("Update Syllabus Success");
        }

        [HttpGet("GetAllSyllabus")]
        public async Task<List<SyllabusViewModel>> GetAllSyllabus() => await _syllabusServices.GetAllSyllabus();

        [HttpGet("GetEnableSyllabus")]
        public async Task<List<SyllabusViewModel>> GetEnableSyllabus() => await _syllabusServices.GetEnableSyllabus();

        [HttpGet("GetDisableSyllabus")]
        public async Task<List<SyllabusViewModel>> GetDisableSyllabus() => await _syllabusServices.GetDisableSyllabus();

        [HttpGet("GetSyllabusById/{SyllabusId}")]
        public async Task<SyllabusViewModel> GetSyllabusById(Guid SyllabusId) => await _syllabusServices.GetSyllabusById(SyllabusId);

        [HttpGet("GetSyllabusByName/{SyllabusName}")]
        public async Task<List<SyllabusViewModel>> GetSyllabusByName(string SyllabusName) => await _syllabusServices.GetSyllabusByName(SyllabusName);

        [HttpGet("GetSyllabusByTrainingProgramId/{TrainingProgramId}")]
        public async Task<List<SyllabusViewModel>> GetSyllabusByTrainingProgramId(Guid TrainingProgramId) => await _syllabusServices.GetSyllabusByTrainingProgramId(TrainingProgramId);

        [HttpGet("GetSyllabusByOutputStandardId/{OutputStandardId}")]
        public async Task<List<SyllabusViewModel>> GetSyllabusByOutputStandardId(Guid OutputStandardId) => await _syllabusServices.GetSyllabusByOutputStandardId(OutputStandardId);
    }
}
