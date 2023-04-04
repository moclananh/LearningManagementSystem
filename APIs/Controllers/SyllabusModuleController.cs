using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusModuleViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class SyllabusModuleController : ControllerBase
    {
        private readonly ISyllabusModuleService _syllabusModuleService;
        public SyllabusModuleController(ISyllabusModuleService syllabusModuleService)
        {
            _syllabusModuleService = syllabusModuleService;
        }

        [HttpGet("GetAllSyllabusModule")]
        [Authorize(policy: "Admins")]
        public async Task<Pagination<SyllabusModuleViewModel>> GetAllSyllabusModule(int pageIndex = 0, int pageSize = 10)
        {
            return await _syllabusModuleService.GetAllSyllabusModuleAsync(pageIndex, pageSize);
        }

        [HttpPost("AddMultiModulesToSyllabus/{syllabusId}")]
        [Authorize(policy: "Admins")]
        public async Task<Response> AddMultiModulesToSyllabus(Guid syllabusId, [FromBody] List<Guid> moduleIds)
        {
            return await _syllabusModuleService.AddMultiModulesToSyllabus(syllabusId, moduleIds);
        }
    }
}
