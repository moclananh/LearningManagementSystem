using Application.ViewModels.QuizzViewModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.PracticeViewModels;
using FluentValidation.Results;
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

        [HttpGet("GetAllPractice")]
        public async Task<Pagination<PracticeViewModel>> GetPracticePagingsion(int pageIndex = 0, int pageSize = 10) => await _service.GetAllPractice(pageIndex, pageSize);

        [HttpPost("CreatePractice")]
        public async Task<CreatePracticeViewModel> CreatePractice(CreatePracticeViewModel PracticeModel) => await _service.CreatePracticeAsync(PracticeModel);
        [HttpGet("GetPracticeByName/{PracticeName}")]
        public async Task<Pagination<PracticeViewModel>> GetPracticeByName(string PracticeName, int pageIndex = 0, int pageSize = 10) => await _service.GetpracticeByName(PracticeName, pageIndex, pageSize);
        [HttpGet("GetEnablePractice")]
        public async Task<Pagination<PracticeViewModel>> GetEnablePractices(int pageIndex = 0, int pageSize = 10) => await _service.GetEnablePractice(pageIndex, pageSize);
        [HttpGet("GetDisablePractice")]
        public async Task<Pagination<PracticeViewModel>> GetDisablePractice(int pageIndex = 0, int pageSize = 10) => await _service.GetDisablePractice(pageIndex, pageSize);
        [HttpPut("UpdatePractice/{PracticeId}")]
        public async Task<UpdatePracticeViewModel> UpdatePractice(Guid PracticeId, UpdatePracticeViewModel practiceDTO) => await _service.UpdatePractice(PracticeId, practiceDTO);
    }
}
