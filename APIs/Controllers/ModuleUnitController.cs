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
    public class ModuleUnitController : ControllerBase
    {
        private readonly IModuleUnitService _moduleUnitService;
        public ModuleUnitController(IModuleUnitService moduleUnitService)
        {
            _moduleUnitService = moduleUnitService;
        }

        [HttpGet("GetAllModuleUnits")]
        [Authorize(policy: "Admins")]
        public async Task<Response> GetAllModuleUnits(int pageIndex = 0, int pageSize = 10)
        {
            return await _moduleUnitService.GetAllModuleUnitsAsync(pageIndex, pageSize);
        }
    }
}
