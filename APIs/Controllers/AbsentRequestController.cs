using Applications.Interfaces;
using Applications.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "AuthUser")]
    public class AbsentRequestController : ControllerBase
    {
        private readonly IAbsentRequestServices _absentrequestService;

        public AbsentRequestController(IAbsentRequestServices absentrequestServices
             )
        {
            _absentrequestService = absentrequestServices;
        }

        //[HttpGet("GetAllAbsentRequestByEmail/{Email}")]
        //public async Task<Response> GetAllAbsentRequestByEmail(string Email, int pageIndex = 0, int pageSize = 10) => await _absentrequestService.GetAllAbsentRequestByEmail(Email, pageIndex, pageSize);
        
        [HttpGet("GetAbsentById/{AbsentId}")]
        [Authorize(policy: "All")]
        public async Task<Response> GetAbsentById(Guid AbsentId) => await _absentrequestService.GetAbsentById(AbsentId);
    }
}
