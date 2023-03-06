using Application.Interfaces;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassUserController : ControllerBase
    {
        private readonly IClassUserServices _classUserServices;
        private readonly IClassService _classService;
        public ClassUserController(IClassUserServices classuserServices, IClassService classService)
        {
            _classUserServices = classuserServices;
            _classService = classService;
        }

        [HttpGet("GetAllClassUser")]
        public async Task<Response> GetAllClassUser(int pageIndex = 0, int pageSize = 10)
        {
            return await _classUserServices.GetAllClassUsersAsync(pageIndex, pageSize);
        }

        [HttpPost("UploadClassUserFile")]
        public async Task<Response> Import(IFormFile formFile) => await _classUserServices.UploadClassUserFile(formFile);

        [HttpGet("{ClassCode}/export")]
        public async Task<IActionResult> Export(string ClassCode)
        {
            var clas = await _classService.GetClassByClassCode(ClassCode);
            if (clas is null)
            {
                return BadRequest("ClassCode Not Exist");
            }
            var content = await _classUserServices.ExportClassUserByClassId(clas.Id);
            if (content == null)
            {
                return NotFound("Something wrong while exporting file, please remake the export command");
            }
            else
            {
                var fileName = $"ClassUsers_{ClassCode}.xlsx";
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}
