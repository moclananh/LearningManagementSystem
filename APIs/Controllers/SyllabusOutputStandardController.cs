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
    public class SyllabusOutputStandardController : ControllerBase
    {
        private readonly ISyllabusOutputStandardService _syllabusOutputStandardService;
        public SyllabusOutputStandardController(ISyllabusOutputStandardService syllabusOutputStandardService)
        {
            _syllabusOutputStandardService = syllabusOutputStandardService;
        }

        [HttpGet("GetAllSyllabusOutputStandard")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetAllSyllabusOutputStandards(int pageIndex = 0, int pageSize = 10)
        {
            return await _syllabusOutputStandardService.GetAllSyllabusOutputStandards(pageIndex, pageSize);
        }

        [HttpPost("AddMultipleOutputStandardsToSyllabus/{syllabusId}")]
        [Authorize(policy: "Admins")]
        public async Task<Response> AddMultipleOutputStandardsToSyllabus(Guid syllabusId, List<Guid> outputStandardIds)
        {
            return await _syllabusOutputStandardService.AddMultipleOutputStandardsToSyllabus(syllabusId, outputStandardIds);
        }

    }
}
