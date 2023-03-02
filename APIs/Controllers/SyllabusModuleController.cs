using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.SyllabusModuleViewModel;
using Applications.ViewModels.UnitModuleViewModel;
using Microsoft.AspNetCore.Http;
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
    }
}
