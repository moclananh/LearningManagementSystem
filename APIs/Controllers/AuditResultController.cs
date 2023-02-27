using Application.Interfaces;
using Applications.ViewModels.AuditResultViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditResultController : ControllerBase
    {
        private readonly IAuditResultServices _service;

        public AuditResultController(IAuditResultServices service)
        {
            _service = service;
        }

        [HttpGet("GetByAuditPlanId")]
        public async Task<AuditResultViewModel> GetByAuditPlanId(Guid id)
        {
            return await _service.GetByAudiPlanId(id);
        }
    }
}
