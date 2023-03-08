using Applications.Interfaces;
using Applications.ViewModels.Response;
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

       /* [HttpGet("GetAllSyllabus")]
        public async Task<Response> GetAllSyllabus(int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetAllSyllabus(pageNumber, pageSize);*/


        [HttpGet("GetAllSyllabusDetail")]
        public async Task<Response> GetAllSyllabusDetail(int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetAllSyllabusDetail(pageNumber, pageSize);

        [HttpGet("GetSyllabusDetailById/{SyllabusId}")]
        public async Task<Response> GetSyllabusDetailById(Guid SyllabusId) => await _syllabusServices.GetSyllabusDetails(SyllabusId);

        [HttpGet("GetEnableSyllabus")]
        public async Task<Response> GetEnableSyllabus(int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetEnableSyllabus(pageNumber, pageSize);

        [HttpGet("GetDisableSyllabus")]
        public async Task<Response> GetDisableSyllabus(int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetDisableSyllabus(pageNumber, pageSize);

        [HttpGet("GetSyllabusById/{SyllabusId}")]
        public async Task<Response> GetSyllabusById(Guid SyllabusId) => await _syllabusServices.GetSyllabusById(SyllabusId);

        [HttpGet("GetSyllabusByName/{SyllabusName}")]
        public async Task<Response> GetSyllabusByName(string SyllabusName, int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetSyllabusByName(SyllabusName, pageNumber, pageSize);

        [HttpGet("GetSyllabusByTrainingProgramId/{TrainingProgramId}")]
        public async Task<Response> GetSyllabusByTrainingProgramId(Guid TrainingProgramId, int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetSyllabusByTrainingProgramId(TrainingProgramId, pageNumber, pageSize);

        [HttpGet("GetSyllabusByOutputStandardId/{OutputStandardId}")]
        public async Task<Response> GetSyllabusByOutputStandardId(Guid OutputStandardId, int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetSyllabusByOutputStandardId(OutputStandardId, pageNumber, pageSize);
        [HttpPost("AddSyllabusModule/{SyllabusId}/{moduleId}")]
        public async Task<IActionResult> AddSyllabusModule(Guid SyllabusId, Guid moduleId)
        {
            if (ModelState.IsValid)
            {
                await _syllabusServices.AddSyllabusModule(SyllabusId, moduleId);
                return Ok("Add Success");
            }
            return BadRequest("Add Fail");
        }
        [HttpDelete("DeleteModuleSyllabus/{SyllabusId}/{moduleId}")]
        public async Task<IActionResult> DeleteUnit(Guid SyllabusId, Guid moduleId)
        {
            if (ModelState.IsValid)
            {
                await _syllabusServices.RemoveSyllabusModule(SyllabusId, moduleId);
                return Ok("Remove Success");
            }
            return BadRequest("Remove Unit Fail");
        }
    }
}
