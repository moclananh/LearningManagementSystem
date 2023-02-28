using Applications.Commons;
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

        public async Task<Pagination<SyllabusViewModel>> GetSyllabusToPagination(int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.ToPagination(pageNumber, pageSize);
            return _mapper.Map<Pagination<SyllabusViewModel>>(syllabus);
        }

        public async Task<Pagination<SyllabusViewModel>> GetDisableSyllabus(int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetDisableSyllabus(pageNumber, pageSize);
            return _mapper.Map<Pagination<SyllabusViewModel>>(syllabus);
        }

        public async Task<Pagination<SyllabusViewModel>> GetEnableSyllabus(int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetEnableSyllabus(pageNumber, pageSize);
            return _mapper.Map<Pagination<SyllabusViewModel>>(syllabus);
        }

        public async Task<SyllabusViewModel> GetSyllabusById(Guid SyllabusId)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            return _mapper.Map<SyllabusViewModel>(syllabus);
        }

        public async Task<Pagination<SyllabusViewModel>> GetSyllabusByName(string Name, int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByName(Name, pageNumber, pageSize);
            return _mapper.Map<Pagination<SyllabusViewModel>>(syllabus);
        }

        public async Task<Pagination<SyllabusViewModel>> GetSyllabusByOutputStandardId(Guid OutputStandardId, int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByOutputStandardId(OutputStandardId, pageNumber, pageSize);
            return _mapper.Map<Pagination<SyllabusViewModel>>(syllabus);
        }

        public async Task<Pagination<SyllabusViewModel>> GetSyllabusByTrainingProgramId(Guid TrainingProgramId, int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByTrainingProgramId(TrainingProgramId, pageNumber, pageSize);
            return _mapper.Map<Pagination<SyllabusViewModel>>(syllabus);
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
