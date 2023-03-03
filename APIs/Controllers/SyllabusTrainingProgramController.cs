using Applications.Interfaces;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        public class SyllabusTrainingProgramController : ControllerBase
        {
            private readonly ISyllabusTrainingProgramService _syllabusTrainingProgramService;
            public SyllabusTrainingProgramController(ISyllabusTrainingProgramService syllabusTrainingProgramService)
            {
                _syllabusTrainingProgramService = syllabusTrainingProgramService;
            }

            [HttpGet("GetAllSyllabusTrainingProgram")]
            public async Task<Response> GetAllSyllabusOutputStandards(int pageIndex = 0, int pageSize = 10)
            {
                return await _syllabusTrainingProgramService.GetAllSyllabusTrainingPrograms(pageIndex, pageSize);
            }
        }
}
