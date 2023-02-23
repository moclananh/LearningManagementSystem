using Application.ViewModels.TrainingProgramModels;
using Applications.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingProgramController : ControllerBase
    {
        private readonly ITrainingProgramService _trainingProgramService;

        public TrainingProgramController(ITrainingProgramService trainingProgramService)
        {
            _trainingProgramService = trainingProgramService;
        }

        [HttpPost("CreateTrainingProgram")]
        public async Task<ViewTrainingProgram> CreateTrainingProgram(ViewTrainingProgram CreateTrainingProgram) => await _trainingProgramService.CreateTrainingProgramAsync(CreateTrainingProgram);

        [HttpPut("UpdateTrainingProgram/{TrainingProgramId}")]
        public async Task<ViewTrainingProgram> UpdateTrainingProgram(Guid TrainingProgramId, ViewTrainingProgram CreateTrainingProgram) => await _trainingProgramService.UpdateTrainingProgramAsync(TrainingProgramId, CreateTrainingProgram);

        [HttpGet("GetAllTrainingProgram")]
        public async Task<List<ViewTrainingProgram>> ViewAllTrainingProgram() => await _trainingProgramService.ViewAllTrainingProgramAsync();

        [HttpGet("GetTrainingProgramDisable")]
        public async Task<List<ViewTrainingProgram>> ViewTrainingProgramDisable() => await _trainingProgramService.ViewTrainingProgramDisableAsync();

        [HttpGet("GetTrainingProgramEnable")]
        public async Task<List<ViewTrainingProgram>> ViewTrainingProgramEnable() => await _trainingProgramService.ViewTrainingProgramEnableAsync();

        [HttpGet("GetTrainingProgramById/{TrainingProgramId}")]
        public async Task<ViewTrainingProgram> GetTrainingProgramById(Guid TrainingProgramId) => await _trainingProgramService.GetTrainingProgramById(TrainingProgramId);

        [HttpGet("GetTrainingProgramByClassId/{ClassId}")]
        public async Task<List<ViewTrainingProgram>> GetTrainingProgramByClassId(Guid ClassId) => await _trainingProgramService.GetTrainingProgramByClassId(ClassId);
    }
}
