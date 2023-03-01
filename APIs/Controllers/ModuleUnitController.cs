using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
using Applications.ViewModels.UnitModuleViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleUnitController : ControllerBase
    {
        private readonly IModuleUnitService _moduleUnitService;
        public ModuleUnitController(IModuleUnitService moduleUnitService)
        {
            _moduleUnitService = moduleUnitService;
        }

        [HttpGet("GetAllModuleUnits")]
        public async Task<Pagination<ModuleUnitViewModel>> GetAllModuleUnits(int pageIndex = 0, int pageSize = 10)
        {
            return await _moduleUnitService.GetAllModuleUnitsAsync(pageIndex, pageSize);
        }
    }
}
