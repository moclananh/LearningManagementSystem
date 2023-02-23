using Application.Interfaces;
using Application.ViewModels.QuizzViewModels;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.ClassUserViewModels;
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
        public async Task<List<CreateClassUserViewModel>> ViewAllClassUserAsync() => await _classUserServices.ViewAllClassUserAsync();

        [HttpPost("UploadClassUserFile")]
        public async Task<Response> Import(IFormFile formFile) => await _classUserServices.UploadClassUserFile(formFile);
    }
}
