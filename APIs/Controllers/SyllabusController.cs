using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusViewModels;
using Domain.Entities;
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
        [Authorize(policy: "Admins")]
        public async Task<Response> CreateSyllabus(CreateSyllabusViewModel SyllabusModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult syllabus = _validatorCreate.Validate(SyllabusModel);
                if (syllabus.IsValid)
                {
                    var result = await _syllabusServices.CreateSyllabus(SyllabusModel);
                    return new Response(HttpStatusCode.OK, "Create Succeed", result);
                }
                else
                {
                    return new Response(HttpStatusCode.BadRequest, "Create Failed, Invalid input");
                }
            }
            return new Response(HttpStatusCode.BadRequest, "Invalid Input");
        }

        [HttpPost("CreateSyllabusDetail")]
        [Authorize(policy: "Admins")]
        public async Task<Response> CreateSyllabusDetail(CreateSyllabusDetailModel SyllabusModel) => await _syllabusServices.CreateSyllabusDetail(SyllabusModel);

        [HttpPut("UpdateSyllabus/{SyllabusId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> UpdateSyllabus(Guid SyllabusId, UpdateSyllabusViewModel SyllabusModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult syllabus = _validatorUpdate.Validate(SyllabusModel);
                if (syllabus.IsValid)
                {
                    if (await _syllabusServices.UpdateSyllabus(SyllabusId, SyllabusModel) != null)
                    {
                        return Ok("Update Syllabus Success");
                    }
                    return BadRequest("Invalid Syllabus Id");
                }
            }
            return Ok("Update Fail, Invalid Input Information");
        }

        [HttpPatch("UpdateStatusOnlyOfSyllabus/{SyllabusId}")]
        [Authorize(policy: "Admins")]
        public async Task<Response> UpdateStatusOnlyOfSyllabus(Guid SyllabusId, UpdateStatusOnlyOfSyllabus SyllabusModel) => await _syllabusServices.UpdateStatusOnlyOfSyllabus(SyllabusId, SyllabusModel);

        /* [HttpGet("GetAllSyllabus")]
         public async Task<Response> GetAllSyllabus(int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetAllSyllabus(pageNumber, pageSize);*/


        [HttpGet("GetAllSyllabusDetail")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAllSyllabusDetail(int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetAllSyllabusDetail(pageNumber, pageSize);

        [HttpGet("GetSyllabusDetailById/{SyllabusId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetSyllabusDetailById(Guid SyllabusId) => await _syllabusServices.GetSyllabusDetails(SyllabusId);

        [HttpGet("GetEnableSyllabus")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetEnableSyllabus(int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetEnableSyllabus(pageNumber, pageSize);

        [HttpGet("GetDisableSyllabus")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetDisableSyllabus(int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetDisableSyllabus(pageNumber, pageSize);

        /*[HttpGet("GetSyllabusById/{SyllabusId}")]
         *[Authorize(policy: "ClassAdmin, OnlySupperAdmin")]
        public async Task<Response> GetSyllabusById(Guid SyllabusId) => await _syllabusServices.GetSyllabusById(SyllabusId);*/

        [HttpGet("GetSyllabusByName/{SyllabusName}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetSyllabusByName(string SyllabusName, int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetSyllabusByName(SyllabusName, pageNumber, pageSize);

        [HttpGet("GetSyllabusByTrainingProgramId/{TrainingProgramId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetSyllabusByTrainingProgramId(Guid TrainingProgramId, int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetSyllabusByTrainingProgramId(TrainingProgramId, pageNumber, pageSize);

        [HttpGet("GetSyllabusByOutputStandardId/{OutputStandardId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetSyllabusByOutputStandardId(Guid OutputStandardId, int pageNumber = 0, int pageSize = 10) => await _syllabusServices.GetSyllabusByOutputStandardId(OutputStandardId, pageNumber, pageSize);
        
        [HttpPost("AddSyllabusModule/{SyllabusId}/{moduleId}")]
        [Authorize(policy: "Admins")]
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
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> DeleteModuleSyllabus(Guid SyllabusId, Guid moduleId)
        {
            if (ModelState.IsValid)
            {
                var deletedSyllabusModule = await _syllabusServices.RemoveSyllabusModule(SyllabusId, moduleId);
                if (deletedSyllabusModule == null)
                {
                    return BadRequest("Module not found in syllabus");
                }
                return Ok("Remove Success");

            }
            return BadRequest("Remove ModuleSyllabus Fail");
        }

        [HttpGet("GetSyllabusByCreationDate/{startDate}/{endDate}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetSyllabusByCreationDate(DateTime startDate, DateTime endDate, int pageNumber = 0, int pageSize = 10)
        {
            return await _syllabusServices.GetSyllabusByCreationDate(startDate, endDate, pageNumber, pageSize);
        }
    }
}
