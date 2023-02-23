using Applications.Interfaces;
using Applications.ViewModels.ClassViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassServices _classServices;
        public ClassController(IClassServices classServices)
        {
            _classServices = classServices;
        }

        [HttpPost("CreateClass")]
        public async Task<ClassViewModel> CreateClass(ClassViewModel ClassModel) => await _classServices.CreateClass(ClassModel);

        [HttpGet("GetAllClasses")]
        public async Task<List<ClassViewModel>> GetAllClasses() => await _classServices.GetAllClasses();

        [HttpGet("GetClassById/{ClassId}")]
        public async Task<ClassViewModel> GetClassById(Guid ClassId) => await _classServices.GetClassById(ClassId);

        [HttpGet("GetEnableClasses")]
        public async Task<List<ClassViewModel>> GetEnableClasses() => await _classServices.GetEnableClasses();

        [HttpGet("GetDisableClasses")]
        public async Task<List<ClassViewModel>> GetDiableClasses() => await _classServices.GetDisableClasses();

        [HttpPut("UpdateClass/{ClassId}")]
        public async Task<ClassViewModel> UpdateClass(Guid ClassId, ClassViewModel Class) => await _classServices.UpdateClass(ClassId, Class);

    }
}
