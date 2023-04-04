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
    public class SyllabusTrainingProgramController : ControllerBase
    {
        private readonly ISyllabusTrainingProgramService _syllabusTrainingProgramService;
        public SyllabusTrainingProgramController(ISyllabusTrainingProgramService syllabusTrainingProgramService)
        {
            _syllabusTrainingProgramService = syllabusTrainingProgramService;
        }

        [HttpGet("GetAllSyllabusTrainingProgram")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetAllSyllabusOutputStandards(int pageIndex = 0, int pageSize = 10)
        {
            return await _syllabusTrainingProgramService.GetAllSyllabusTrainingPrograms(pageIndex, pageSize);
        }
        [HttpPost("AddMultipleSyllabusesToTrainingProgram/{trainingProgramId}" )]
        [Authorize(policy: "Admins")]
        public async Task<Response> AddMultipleSyllabusesToTrainingProgram(Guid trainingProgramId, List<Guid> syllabusesId)
        {
            return await _syllabusTrainingProgramService.AddMultipleSyllabusesToTrainingProgram(trainingProgramId, syllabusesId);
        }

    }
}
