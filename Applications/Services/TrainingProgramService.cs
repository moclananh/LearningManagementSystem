using Application.ViewModels.TrainingProgramModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.TrainingProgramModels;
using Applications.ViewModels.TrainingProgramSyllabi;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;

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

        public async Task<CreateTrainingProgramSyllabi> AddSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId)
        {
            var SyllabusObj = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            var trainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(TrainingProgramId);
            if (SyllabusObj != null && trainingProgram != null)
            {
                var trainingProgramSyllabus = new TrainingProgramSyllabus()
                {
                    Syllabus = SyllabusObj,
                    TrainingProgram = trainingProgram
                };
                await _unitOfWork.TrainingProgramSyllabiRepository.AddAsync(trainingProgramSyllabus);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<CreateTrainingProgramSyllabi>(trainingProgramSyllabus);
                }
            }
            return null;
        }

        public async Task<CreateTrainingProgramViewModel?> CreateTrainingProgramAsync(CreateTrainingProgramViewModel TrainingProgramDTO)
        {
            var TrainingProgramObj = _mapper.Map<TrainingProgram>(TrainingProgramDTO);
            await _unitOfWork.TrainingProgramRepository.AddAsync(TrainingProgramObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CreateTrainingProgramViewModel>(TrainingProgramObj);
            }
            return null;
        }

        public async Task<Pagination<TrainingProgramViewModel>> GetTrainingProgramByClassId(Guid ClassId, int pageIndex = 0, int pageSize = 10)
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramByClassId(ClassId);
            var result = _mapper.Map<Pagination<TrainingProgramViewModel>>(TrainingPrograms);
            return result;
        }

        public async Task<TrainingProgramViewModel> GetTrainingProgramById(Guid TrainingProramId)
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(TrainingProramId);
            var result = _mapper.Map<TrainingProgramViewModel>(TrainingPrograms);
            return result;
        }

        public async Task<CreateTrainingProgramSyllabi> RemoveSyllabusToTrainingProgram(Guid SyllabusId, Guid TrainingProgramId)
        {
            var trainingProgramSyllabus = await _unitOfWork.TrainingProgramSyllabiRepository.GetTrainingProgramSyllabus(SyllabusId, TrainingProgramId);
            if (trainingProgramSyllabus != null)
            {
                _unitOfWork.TrainingProgramSyllabiRepository.SoftRemove(trainingProgramSyllabus);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<CreateTrainingProgramSyllabi>(trainingProgramSyllabus);
                }
            }
            return null;
        }

        public async Task<UpdateTrainingProgramViewModel?> UpdateTrainingProgramAsync(Guid TrainingProgramId, UpdateTrainingProgramViewModel TrainingProgramDTO)
        {
            var TrainingProgramObj = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(TrainingProgramId);
            if (TrainingProgramObj != null)
            {
                _mapper.Map(TrainingProgramDTO, TrainingProgramObj);
                _unitOfWork.TrainingProgramRepository.Update(TrainingProgramObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateTrainingProgramViewModel>(TrainingProgramObj);
                }
            }
            return null;
        }

        public async Task<Pagination<TrainingProgramViewModel>> ViewAllTrainingProgramAsync(int pageIndex = 0, int pageSize = 10)
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<TrainingProgramViewModel>>(TrainingPrograms);
            return result;
        }

        public async Task<Pagination<TrainingProgramViewModel>> ViewTrainingProgramDisableAsync(int pageIndex = 0, int pageSize = 10)
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramDisable(pageIndex,pageSize);
            var result = _mapper.Map<Pagination<TrainingProgramViewModel>>(TrainingPrograms);
            return result;
        }

        public async Task<Pagination<TrainingProgramViewModel>> ViewTrainingProgramEnableAsync(int pageIndex = 0, int pageSize = 10)
        {
            var TrainingPrograms = await _unitOfWork.TrainingProgramRepository.GetTrainingProgramEnable(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<TrainingProgramViewModel>>(TrainingPrograms);
            return result;
        }
    }
}
