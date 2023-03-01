using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassTrainingProgramController : ControllerBase
    {
        private readonly IClassTrainingProgramService _classTrainingProgramService;

        public ClassTrainingProgramController(IClassTrainingProgramService classTrainingProgramService)
        {
            _classTrainingProgramService = classTrainingProgramService;
        }

        [HttpGet("GetAllClassTrainingProgram")]
        public async Task<Pagination<ClassTrainingProgramViewModel>> GetAllClassTrainingProgram(int pageIndex = 0, int pageSize = 10)
        {
            return await _classTrainingProgramService.GetAllClassTrainingProgram(pageIndex, pageSize);
        }
    }
}
