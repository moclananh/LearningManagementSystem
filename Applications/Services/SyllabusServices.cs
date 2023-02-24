using Applications.Interfaces;
using Applications.ViewModels.SyllabusViewModels;
using AutoMapper;
using Domain.Entities;

namespace Applications.Services
{
    public class SyllabusServices : ISyllabusServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SyllabusServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CreateSyllabusViewModel?> CreateSyllabus(CreateSyllabusViewModel SyllabusDTO)
        {
            var syllabus = _mapper.Map<Syllabus>(SyllabusDTO);
            await _unitOfWork.SyllabusRepository.AddAsync(syllabus);
            var isSucces = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSucces)
            {
                return _mapper.Map<CreateSyllabusViewModel>(syllabus);
            }
            return null;
        }

        public async Task<List<SyllabusViewModel>> GetAllSyllabus()
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetAllAsync();
            return _mapper.Map<List<SyllabusViewModel>>(syllabus);
        }

        public async Task<List<SyllabusViewModel>> GetDisableSyllabus()
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetDisableSyllabus();
            return _mapper.Map<List<SyllabusViewModel>>(syllabus);
        }

        public async Task<List<SyllabusViewModel>> GetEnableSyllabus()
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetEnableSyllabus();
            return _mapper.Map<List<SyllabusViewModel>>(syllabus);
        }

        public async Task<SyllabusViewModel> GetSyllabusById(Guid SyllabusId)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            return _mapper.Map<SyllabusViewModel>(syllabus);
        }

        public async Task<List<SyllabusViewModel>> GetSyllabusByName(string Name)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByName(Name);
            return _mapper.Map<List<SyllabusViewModel>>(syllabus);
        }

        public async Task<List<SyllabusViewModel>> GetSyllabusByOutputStandardId(Guid OutputStandardId)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByOutputStandardId(OutputStandardId);
            return _mapper.Map<List<SyllabusViewModel>>(syllabus);
        }

        public async Task<List<SyllabusViewModel>> GetSyllabusByTrainingProgramId(Guid TrainingProgramId)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByTrainingProgramId(TrainingProgramId);
            return _mapper.Map<List<SyllabusViewModel>>(syllabus);
        }

        public async Task<UpdateSyllabusViewModel?> UpdateSyllabus(Guid SyllabusId, UpdateSyllabusViewModel SyllabusDTO)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            if (syllabus != null)
            {
                _mapper.Map(SyllabusDTO, syllabus);
                _unitOfWork.SyllabusRepository.Update(syllabus);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateSyllabusViewModel>(syllabus);
                }
            }
            return null;
        }
    }
}
