using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ClassViewModels;
using Applications.ViewModels.Response;
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
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classServices;
        private readonly IValidator<UpdateClassViewModel> _validatorUpdate;
        private readonly IValidator<CreateClassViewModel> _validatorCreate;
        private readonly IValidator<ClassFiltersViewModel> _validatorFilter;
        public ClassController(IClassService classServices,
            IValidator<UpdateClassViewModel> validatorUpdate,
            IValidator<CreateClassViewModel> validatorCreate,
            IValidator<ClassFiltersViewModel> validatorFilter)
        {
            _classServices = classServices;
            _validatorUpdate = validatorUpdate;
            _validatorCreate = validatorCreate;
            _validatorFilter = validatorFilter;
        }

        [HttpPost("CreateClass")]
        [Authorize(policy: "Admins")]
        public async Task<Response> CreateClass(CreateClassViewModel classModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult validation = _validatorCreate.Validate(classModel);
                if (validation.IsValid)
                {
                    await _classServices.CreateClass(classModel);
                    return new Response(HttpStatusCode.OK, "Create Class Succeed", classModel);
                }
            }

            return new Response(HttpStatusCode.BadRequest, "Invalid Input");
        }

        [HttpGet("GetAllClasses")]
        [Authorize(policy: "All")]
        public async Task<Pagination<ClassViewModel>> GetAllClasses(int pageIndex = 0, int pageSize = 10) => await _classServices.GetAllClasses(pageIndex, pageSize);

        [HttpGet("GetClassById/{classId}")]
        [Authorize(policy: "All")]
        public async Task<ClassViewModel> GetClassById(Guid classId) => await _classServices.GetClassById(classId);

        [HttpGet("GetClassByName/{className}")]
        [Authorize(policy: "All")]
        public async Task<IActionResult> GetClassesByName(string className, int pageIndex = 0, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
                var result = await _classServices.GetClassByName(className, pageIndex, pageSize);
                if (result.Items.Count > 0)
                {
                    return Ok(result);
                }
            }

            return NotFound("Not found Class");
        }

        [HttpGet("GetEnableClasses")]
        [Authorize(policy: "Admins")]
        public async Task<Pagination<ClassViewModel>> GetEnableClasses(int pageIndex = 0, int pageSize = 10) => await _classServices.GetEnableClasses(pageIndex, pageSize);

        [HttpGet("GetDisableClasses")]
        [Authorize(policy: "Admins")]
        public async Task<Pagination<ClassViewModel>> GetDiableClasses(int pageIndex = 0, int pageSize = 10) => await _classServices.GetDisableClasses(pageIndex, pageSize);

        [HttpPut("UpdateClass/{classId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> UpdateClass(Guid classId, UpdateClassViewModel classModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorUpdate.Validate(classModel);
                if (result.IsValid)
                {
                    await _classServices.UpdateClass(classId, classModel);
                    return Ok($"Update Class Success: \n{classModel}");
                }
            }

            return BadRequest("Update Class Fail");
        }

        [HttpPatch("UpdateStatusOnlyOfClass/{ClassId}")]
        [Authorize(policy: "Admins")]
        public async Task<Response> UpdateStatusOnlyOfClass(Guid ClassId, UpdateStatusOnlyOfClass classModel) => await _classServices.UpdateStatusOnlyOfClass(ClassId, classModel);

        [HttpPost("Class/AddTrainingProgram/{classId}/{trainingProgramId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> AddTrainingProgram(Guid classId, Guid trainingProgramId)
        {
            if (ModelState.IsValid)
            {
                var result = await _classServices.AddTrainingProgramToClass(classId, trainingProgramId);
                if (result != null)
                {
                    return Ok("Add Success");
                }
            }

            return BadRequest("Add TrainingProgram Fail");
        }

        [HttpDelete("Class/DeleteTrainingProgram/{classId}/{trainingProgramId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> DeleTrainingProgram(Guid classId, Guid trainingProgramId)
        {
            if (ModelState.IsValid)
            {
                var result = await _classServices.RemoveTrainingProgramFromClass(classId, trainingProgramId);
                if (result == null)
                {
                    return Ok("Remove Success");
                }
            }

            return BadRequest("Remove TrainingProgram Fail");
        }

        [HttpDelete("Class/DeleteUser/{classId}/{userId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> DeleteClassUser(Guid classId, Guid userId)
        {
            var result = await _classServices.RemoveUserFromClass(classId, userId);
            if (result != null)
            {
                return Ok("Remove Success");
            }

            return BadRequest("Remove UserFromClass Fail");
        }

        [HttpPost("GetClassByFilter")]
        [Authorize(policy: "All")]
        public async Task<IActionResult> GetClassByFilter(ClassFiltersViewModel filters, int pageNumber = 0, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
                var validation = _validatorFilter.Validate(filters);
                if (validation.IsValid)
                {
                    var classes = await _classServices.GetClassByFilter(filters, pageNumber = 0, pageSize = 10);
                    if (classes != null)
                    {
                        return Ok(classes);
                    }

                    return NotFound("Not found");
                }
            }

            return BadRequest("GetClassByFilter Fail");
        }

        [HttpGet("GetClassDetails/{classId}")]
        [Authorize(policy: "All")]
        public async Task<IActionResult> GetClassDetails(Guid classId)
        {
            if (ModelState.IsValid)
            {
                var classObj = await _classServices.GetClassDetails(classId);
                if (classObj != null)
                {
                    return Ok(classObj);
                }
            }

            return BadRequest("GetClassDetails Fail");
        }

        [HttpPost("AddUserToClass/{classId}/{userId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> AddUserToClass(Guid classId, Guid userId)
        {
            if (ModelState.IsValid)
            {
                var classObj = await _classServices.AddUserToClass(classId, userId);
                return Ok(classObj);
            }

            return BadRequest("Add Fail");
        }

        [HttpPut("ApprovedClass/{classId}")]
        [Authorize(policy: "Admins")]
        public async Task<IActionResult> ApprovedClass(Guid classId)
        {
            if (ModelState.IsValid)
            {
                var classObj = await _classServices.ApprovedClass(classId);
                return Ok(classObj);
            }

            return BadRequest("Approved Fail");
        }
    }
}