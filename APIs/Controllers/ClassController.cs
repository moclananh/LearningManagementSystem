﻿using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Applications.ViewModels.ClassViewModels;
using Domain.Enum.ClassEnum;
using Domain.Enum.StatusEnum;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

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
        public async Task<IActionResult> CreateClass(CreateClassViewModel ClassModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validatorCreate.Validate(ClassModel);
                if (result.IsValid)
                {
                    await _classServices.CreateClass(ClassModel);
                }
                else
                {
                    return BadRequest("Fail to create new Class");
                }
            }
            return Ok("Create new Class Success");
        }

        [HttpGet("GetAllClasses")]
        public async Task<Pagination<ClassViewModel>> GetAllClasses(int pageIndex = 0, int pageSize = 10) => await _classServices.GetAllClasses(pageIndex, pageSize);

        [HttpGet("GetClassById/{ClassId}")]
        public async Task<ClassViewModel> GetClassById(Guid ClassId) => await _classServices.GetClassById(ClassId);

        [HttpGet("GetClassByName/{ClassName}")]
        public async Task<Pagination<ClassViewModel>> GetClassesByName(string ClassName, int pageIndex = 0, int pageSize = 10) => await _classServices.GetClassByName(ClassName, pageIndex, pageSize);

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
                }
                else
                {
                    return BadRequest("Update Class Fail");
                }
            }
            return Ok("Update Class Success");
        }

        [HttpPost("Class/AddTrainingProgram/{ClassId}/{TrainingProgramId}")]
        public async Task<IActionResult> AddTrainingProgram(Guid ClassId, Guid TrainingProgramId)
        {
            if (ModelState.IsValid)
            {
                await _classServices.AddTrainingProgramToClass(ClassId, TrainingProgramId);
                return Ok("Add Success");
            }
            return BadRequest("Add TrainingProgram Fail");
        }

        [HttpDelete("Class/DeleteTrainingProgram/{ClassId}/{TrainingProgramId}")]
        public async Task<IActionResult> DeleTrainingProgram(Guid ClassId, Guid TrainingProgramId)
        {
            if (ModelState.IsValid)
            {
                await _classServices.RemoveTrainingProgramToClass(ClassId, TrainingProgramId);
                return Ok("Remove Success");
            }
            return BadRequest("Remove TrainingProgram Fail");
        }

        [HttpDelete("Class/DeleteUser/{ClassId}/{UserId}")]
        public async Task<IActionResult> DeleteUser(Guid ClassId, Guid UserId)
        {
            if (ModelState.IsValid)
            {
                await _classServices.RemoveUserToClass(ClassId, UserId);
                return Ok("Remove Success");
            }
            return BadRequest("Remove User Fail");
        }

        [HttpGet("GetClassByFilter")]
        public async Task<IActionResult> GetClassByFilter(LocationEnum locations, ClassTimeEnum classTime, Status status, AttendeeEnum attendee, FSUEnum fsu, DateTime? startDate, DateTime? endDate, int pageNumber = 0, int pageSize = 10)
        {
            if (ModelState.IsValid)
            {
                var classes = await _classServices.GetClassByFilter(locations, classTime, status, attendee, fsu, startDate, endDate, pageNumber = 0, pageSize = 10);
                return Ok(classes);
            }
            return BadRequest("GetClassByFilter Fail");
        }
        [HttpGet("GetClassDetails/{ClassId}")]
        public async Task<IActionResult> GetClassDetails(Guid ClassId)
        {
            if (ModelState.IsValid)
            {
                var classObj = await _classServices.GetClassDetails(ClassId);
                return Ok(classObj);
            }
            return BadRequest("GetClassDetails Fail");
        }
    }
}