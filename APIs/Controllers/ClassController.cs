using Applications.Interfaces;
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
        private readonly IValidator<ClassViewModel> _validator;
        public ClassController(IClassServices classServices,
            IValidator<ClassViewModel> validator)
        {
            _classServices = classServices;
            _validator = validator;
        }

        [HttpPost("CreateClass")]
        public async Task<IActionResult> CreateClass(ClassViewModel ClassModel)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validator.Validate(ClassModel);
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

        [HttpGet("GetEnableClasses")]
        public async Task<List<ClassViewModel>> GetEnableClasses() => await _classServices.GetEnableClasses();

        [HttpGet("GetDisableClasses")]
        public async Task<List<ClassViewModel>> GetDiableClasses() => await _classServices.GetDisableClasses();

        [HttpPut("UpdateClass/{ClassId}")]
        public async Task<IActionResult> UpdateClass(Guid ClassId, ClassViewModel Class)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = _validator.Validate(Class);
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

    }
}
