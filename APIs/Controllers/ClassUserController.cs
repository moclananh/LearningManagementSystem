using Application.Interfaces;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassUserController : ControllerBase
    {
        private readonly IClassUserServices _classUserServices;
        public ClassUserController(IClassUserServices classuserServices)
        {
            _classUserServices = classuserServices;
        }

        [HttpGet("GetAllClassUser")]
        public async Task<Response> GetAllClassUser(int pageIndex = 0, int pageSize = 10)
        {
            return await _classUserServices.GetAllClassUsersAsync(pageIndex, pageSize);
        }

        [HttpPost("UploadClassUserFile")]
        public async Task<Response> Import(IFormFile formFile) => await _classUserServices.UploadClassUserFile(formFile);

        [HttpGet("{ClassId}/export")]
        public async Task<IActionResult> Export(Guid ClassId)
        {
            var content = await _classUserServices.ExportClassUserByClassId(ClassId);

            var fileName = $"ClassUsers_{ClassId}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
