using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusOutputStandardController : ControllerBase
    {
        private readonly ISyllabusOutputStandardService _syllabusOutputStandardService;
        public SyllabusOutputStandardController(ISyllabusOutputStandardService syllabusOutputStandardService)
        {
            _syllabusOutputStandardService = syllabusOutputStandardService;
        }

        [HttpGet("GetAllSyllabusOutputStandard")]
        public async Task<Pagination<SyllabusOutputStandardViewModel>> GetAllSyllabusOutputStandard(int pageIndex = 0, int pageSize = 10)
        {
            return await _syllabusOutputStandardService.GetSyllabusOutputStandardPagingsionAsync(pageIndex, pageSize);
        }
    }
}
