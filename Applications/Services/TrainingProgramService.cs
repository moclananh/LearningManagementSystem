using Application.ViewModels.TrainingProgramModels;
using Applications.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Applications.Services
{
    public class TrainingProgramService : ITrainingProgramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainingProgramService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ViewTrainingProgram?> CreateTrainingProgramAsync(ViewTrainingProgram TrainingProgramDTO)
        {
            var TrainingProgramObj = _mapper.Map<TrainingProgram>(TrainingProgramDTO);
            await _unitOfWork.TrainingProgramRepository.AddAsync(TrainingProgramObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<ViewTrainingProgram>(TrainingProgramObj);
            }
            return null;
        }

        public async Task<List<ViewTrainingProgram>> GetTrainingProgramByClassId(Guid ClassId)
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramByClassId(ClassId);
            var result = _mapper.Map<List<ViewTrainingProgram>>(TrainingPrograms);
            return result;
        }

        public async Task<ViewTrainingProgram> GetTrainingProgramById(Guid TrainingProramId)
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(TrainingProramId);
            var result = _mapper.Map<ViewTrainingProgram>(TrainingPrograms);
            return result;
        }

        public async Task<ViewTrainingProgram?> UpdateTrainingProgramAsync(Guid TrainingProgramId, ViewTrainingProgram TrainingProgramDTO)
        {
            var TrainingProgramObj = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(TrainingProgramId);
            if (TrainingProgramObj != null)
            {
                _mapper.Map(TrainingProgramDTO, TrainingProgramObj);
                _unitOfWork.TrainingProgramRepository.Update(TrainingProgramObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<ViewTrainingProgram>(TrainingProgramObj);
                }
            }
            return null;
        }

        public async Task<List<ViewTrainingProgram>> ViewAllTrainingProgramAsync()
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetAllAsync();
            var result = _mapper.Map<List<ViewTrainingProgram>>(TrainingPrograms);
            return result;
        }

        public async Task<List<ViewTrainingProgram>> ViewTrainingProgramDisableAsync()
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramDisable();
            var result = _mapper.Map<List<ViewTrainingProgram>>(TrainingPrograms);
            return result;
        }

        public async Task<List<ViewTrainingProgram>> ViewTrainingProgramEnableAsync()
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramEnable();
            var result = _mapper.Map<List<ViewTrainingProgram>>(TrainingPrograms);
            return result;
        }
    }
}
