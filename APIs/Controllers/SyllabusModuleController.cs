using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusModuleViewModel;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusModuleController : ControllerBase
    {
        private readonly ISyllabusModuleService _syllabusModuleService;
        public SyllabusModuleController(ISyllabusModuleService syllabusModuleService)
        {
            _syllabusModuleService = syllabusModuleService;
        }

        [HttpGet("GetAllSyllabusModule")]
        public async Task<Pagination<SyllabusModuleViewModel>> GetAllSyllabusModule(int pageIndex = 0, int pageSize = 10)
        {
            return await _syllabusModuleService.GetAllSyllabusModuleAsync(pageIndex, pageSize);
        }

        [HttpPost("{syllabusId}/AddMultiModulesToSyllabus")]
        public async Task<Response> AddMultiModulesToSyllabus(Guid syllabusId, [FromBody] List<Guid> moduleIds)
        {
            return await _syllabusModuleService.AddMultiModulesToSyllabus(syllabusId, moduleIds);
        }
    }
}
