using Applications.Interfaces;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    [EnableCors("AllowAll")]
    public class ClassTrainingProgramController : ControllerBase
    {
        private readonly IClassTrainingProgramService _classTrainingProgramService;

        public ClassTrainingProgramController(IClassTrainingProgramService classTrainingProgramService)
        {
            _classTrainingProgramService = classTrainingProgramService;
        }

        [HttpGet("GetAllClassTrainingProgram")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetAllClassTrainingProgram(int pageIndex = 0, int pageSize = 10)
        {
            return await _classTrainingProgramService.GetAllClassTrainingProgram(pageIndex, pageSize);
        }
    }
}
