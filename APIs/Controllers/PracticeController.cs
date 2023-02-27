using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.PracticeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticeController : ControllerBase
    {
        private readonly IPracticeService _service;

        public PracticeController(IPracticeService service)
        {
            _service = service;
        }

        [HttpGet("GetPracticesByUnitId/{UnitId}")]
        public async Task<Pagination<PracticeViewModel>> GetPracticesByUnitId(Guid UnitId) => await _service.GetPracticeByUnitId(UnitId);

        [HttpGet("GetPracticeById/{PracticeId}")]
        public async Task<PracticeViewModel> GetPracticeById(Guid PracticeId) => await _service.GetPracticeById(PracticeId);
    }
}
