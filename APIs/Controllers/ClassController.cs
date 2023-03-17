using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ClassViewModels;
using Applications.ViewModels.Response;
using Domain.Enum.ClassEnum;
using Domain.Enum.StatusEnum;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classServices;
        private readonly IValidator<UpdateClassViewModel> _validatorUpdate;
        private readonly IValidator<CreateClassViewModel> _validatorCreate;
        public ClassController(IClassService classServices,
            IValidator<UpdateClassViewModel> validatorUpdate,
            IValidator<CreateClassViewModel> validatorCreate)
        {
            _classServices = classServices;
            _validatorUpdate = validatorUpdate;
            _validatorCreate = validatorCreate;
        }

        [HttpPost("CreateClass")]
        public async Task<Response> CreateClass(CreateClassViewModel ClassModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorCreate.Validate(ClassModel);
                if (result.IsValid)
                {
                    var Class = await _classServices.CreateClass(ClassModel);
                    return new Response(HttpStatusCode.OK, "Create Class Succeed", Class);
                }
            }
            else
            {
                return new Response(HttpStatusCode.BadRequest, "Create Failed, Invalid input");
            }
            return new Response(HttpStatusCode.BadRequest, "Invalid Input");
        }

        [HttpGet("GetAllClasses")]
        public async Task<Pagination<ClassViewModel>> GetAllClasses(int pageIndex = 0, int pageSize = 10) => await _classServices.GetAllClasses(pageIndex, pageSize);

        [HttpGet("GetClassById/{ClassId}")]
        public async Task<ClassViewModel> GetClassById(Guid ClassId) => await _classServices.GetClassById(ClassId);

        [HttpGet("GetClassByName/{ClassName}")]
        public async Task<IActionResult> GetClassesByName(string ClassName, int pageIndex = 0, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
                var result = await _classServices.GetClassByName(ClassName, pageIndex, pageSize);
                return Ok(result);
            }
            return BadRequest($"Not found Class contain name: {ClassName}");
        }

        [HttpGet("GetEnableClasses")]
        public async Task<Pagination<ClassViewModel>> GetEnableClasses(int pageIndex = 0, int pageSize = 10) => await _classServices.GetEnableClasses(pageIndex, pageSize);

        [HttpGet("GetDisableClasses")]
        public async Task<Pagination<ClassViewModel>> GetDiableClasses(int pageIndex = 0, int pageSize = 10) => await _classServices.GetDisableClasses(pageIndex, pageSize);

        [HttpPut("UpdateClass/{ClassId}")]
        public async Task<IActionResult> UpdateClass(Guid ClassId, UpdateClassViewModel Class)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorUpdate.Validate(Class);
                if (result.IsValid)
                {
                    await _classServices.UpdateClass(ClassId, Class);
                    return Ok("Update Class Success");
                }
            }
            return BadRequest("Update Class Fail");
        }

        [HttpPost("Class/AddTrainingProgram/{ClassId}/{TrainingProgramId}")]
        public async Task<IActionResult> AddTrainingProgram(Guid ClassId, Guid TrainingProgramId)
        {
            if (ModelState.IsValid)
            {
                var result = await _classServices.AddTrainingProgramToClass(ClassId, TrainingProgramId);
                if (result == null)
                {
                    return BadRequest("Add TrainingProgram Fail");
                }
            }
            return Ok("Add Success");
        }

        [HttpDelete("Class/DeleteTrainingProgram/{ClassId}/{TrainingProgramId}")]
        public async Task<IActionResult> DeleTrainingProgram(Guid ClassId, Guid TrainingProgramId)
        {
            if (ModelState.IsValid)
            {
                var result = await _classServices.RemoveTrainingProgramFromClass(ClassId, TrainingProgramId);
                if (result == null)
                {
                    return BadRequest("Remove TrainingProgram Fail");
                }
            }
            return Ok("Remove Success");
        }

        [HttpDelete("Class/DeleteUser/{ClassId}/{UserId}")]
        public async Task<IActionResult> DeleteClassUser(Guid ClassId, Guid UserId)
        {
            var result = await _classServices.RemoveUserFromClass(ClassId, UserId);
            if (result == null)
            {
                return BadRequest("Remove UserFromClass Fail");
            }
            return Ok("Remove Success");
        }

        [HttpGet("GetClassByFilter")]
        public async Task<IActionResult> GetClassByFilter(LocationEnum? locations, ClassTimeEnum? classTime, Status? status, AttendeeEnum? attendee, FSUEnum? fsu, DateTime? startDate, DateTime? endDate, int pageNumber = 0, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
                var classes = await _classServices.GetClassByFilter(locations, classTime, status, attendee, fsu, startDate, endDate, pageNumber = 0, pageSize = 10);
                if (classes != null)
                {
                    return Ok(classes);
                }
            }
            return BadRequest("GetClassByFilter Fail");
        }

        [HttpGet("GetClassDetails/{ClassId}")]
        public async Task<IActionResult> GetClassDetails(Guid ClassId)
        {
            if (ModelState.IsValid)
            {
                var classObj = await _classServices.GetClassDetails(ClassId);
                if (classObj != null)
                {
                    return Ok(classObj);
                }
            }

            return BadRequest("GetClassDetails Fail");
        }

        [HttpPost("AddUserToClass/{ClassId}/{UserId}")]
        public async Task<IActionResult> AddUserToClass(Guid ClassId, Guid UserId)
        {
            if (ModelState.IsValid)
            {
                var classObj = await _classServices.AddUserToClass(ClassId, UserId);
                return Ok(classObj);
            }
            return BadRequest("Add Fail");
        }

        [HttpPut("ApprovedClass/{ClassId}")]
        public async Task<IActionResult> ApprovedClass(Guid ClassId)
        {
            if (ModelState.IsValid)
            {
                var classObj = await _classServices.ApprovedClass(ClassId);
                return Ok(classObj);
            }
            return BadRequest("Approved Fail");
        }
    }
}