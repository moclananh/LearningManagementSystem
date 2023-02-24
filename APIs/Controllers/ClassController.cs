using Applications.Interfaces;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Applications.ViewModels.ClassViewModels;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassServices _classServices;
        private readonly IValidator<UpdateClassViewModel> _validatorUpdate;
        private readonly IValidator<CreateClassViewModel> _validatorCreate;
        public ClassController(IClassServices classServices,
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
        public async Task<List<ClassViewModel>> GetAllClasses() => await _classServices.GetAllClasses();

        [HttpGet("GetClassById/{ClassId}")]
        public async Task<ClassViewModel> GetClassById(Guid ClassId) => await _classServices.GetClassById(ClassId);

        [HttpGet("GetClassByName/{ClassName}")]
        public async Task<List<ClassViewModel>> GetClassesByName(string ClassName) => await _classServices.GetClassByName(ClassName);

        [HttpGet("GetEnableClasses")]
        public async Task<List<ClassViewModel>> GetEnableClasses() => await _classServices.GetEnableClasses();

        [HttpGet("GetDisableClasses")]
        public async Task<List<ClassViewModel>> GetDiableClasses() => await _classServices.GetDisableClasses();

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
    }
}
