using Application.Interfaces;
using Application.ViewModels.QuizzViewModels;
using Applications.Commons;
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

        [HttpGet("GetClassUserPagingsion")]
        public async Task<Pagination<CreateClassUserViewModel>> GetClassUserPagingsion(int pageIndex = 0, int pageSize = 10)
        {
            return await _classUserServices.GetClassUserPagingsionAsync(pageIndex, pageSize);
        }

        [HttpPost("UploadClassUserFile")]
        public async Task<Response> Import(IFormFile formFile) => await _classUserServices.UploadClassUserFile(formFile);
    }
}
