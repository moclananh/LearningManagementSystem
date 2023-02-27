using Application.ViewModels.TrainingProgramModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.TrainingProgramSyllabi;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
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
        public async Task<Pagination<ViewTrainingProgram>> ViewAllTrainingProgram(int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.ViewAllTrainingProgramAsync(pageIndex, pageSize);

        [HttpGet("GetTrainingProgramDisable")]
        public async Task<Pagination<ViewTrainingProgram>> ViewTrainingProgramDisable(int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.ViewTrainingProgramDisableAsync(pageIndex, pageSize);

        [HttpGet("GetTrainingProgramEnable")]
        public async Task<Pagination<ViewTrainingProgram>> ViewTrainingProgramEnable(int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.ViewTrainingProgramEnableAsync(pageIndex, pageSize);

        [HttpGet("GetTrainingProgramById/{TrainingProgramId}")]
        public async Task<ViewTrainingProgram> GetTrainingProgramById(Guid TrainingProgramId) => await _trainingProgramService.GetTrainingProgramById(TrainingProgramId);
        [HttpGet("GetTrainingProgramByClassId/{ClassId}")]
        public async Task<Pagination<ViewTrainingProgram>> GetTrainingProgramByClassId(Guid ClassId, int pageIndex = 0, int pageSize = 10) => await _trainingProgramService.GetTrainingProgramByClassId(ClassId, pageIndex, pageSize);

        [HttpPost("AddTrainingProgramSyllabus/{SyllabusId}/{TrainingProgramId}")]
        public async Task<CreateTrainingProgramSyllabi> AddSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId) => await _trainingProgramService.AddSyllabusToTrainingProgram(SyllabusId, TrainingProgramId);

        [HttpDelete("DeleteTrainingProgramSyllabus/{SyllabusId}/{TrainingProgramId}")]
        public async Task<CreateTrainingProgramSyllabi> DeleteTrainingProgram(Guid SyllabusId, Guid TrainingProgramId) => await _trainingProgramService.RemoveSyllabusToTrainingProgram(SyllabusId, TrainingProgramId);
    }
}
