using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.OutputStandardViewModels;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;

namespace Applications.Services
{
    public class OutputStandardService : IOutputStandardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OutputStandardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<OutputStandardViewModel>> ViewAllOutputStandardAsync()
        {
            var outputStandard = await _unitOfWork.OutputStandardRepository.GetAllAsync();
            var result = _mapper.Map<List<OutputStandardViewModel>>(outputStandard);
            return result;
        }
        public async Task<OutputStandardViewModel> GetOutputStandardByOutputStandardIdAsync(Guid OutputStandardId)
        {
            var outputStandard = await _unitOfWork.OutputStandardRepository.GetByIdAsync(OutputStandardId);
            var result = _mapper.Map<OutputStandardViewModel>(outputStandard);
            return result;
        }
        public async Task<CreateOutputStandardViewModel> CreateOutputStandardAsync(CreateOutputStandardViewModel OutputStandardDTO)
        {
            var outputStandardOjb = _mapper.Map<OutputStandard>(OutputStandardDTO);
            await _unitOfWork.OutputStandardRepository.AddAsync(outputStandardOjb);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CreateOutputStandardViewModel>(outputStandardOjb);
            }
            return null;
        }
        public async Task<UpdateOutputStandardViewModel> UpdatOutputStandardAsync(Guid OutputStandardId, UpdateOutputStandardViewModel OutputStandardDTO)
        {
            var outputStandardOjb = await _unitOfWork.OutputStandardRepository.GetByIdAsync(OutputStandardId);
            if (outputStandardOjb != null)
            {
                _mapper.Map(OutputStandardDTO, outputStandardOjb);
                _unitOfWork.OutputStandardRepository.Update(outputStandardOjb);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateOutputStandardViewModel>(outputStandardOjb);
                }
            }
            return null;
        }
        public async Task<CreateSyllabusOutputStandardViewModel> AddOutputStandardToSyllabus(Guid SyllabusId, Guid OutputStandardId)
        {
            var syllabusOjb = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            var outputStandard = await _unitOfWork.OutputStandardRepository.GetByIdAsync(OutputStandardId);
            if (syllabusOjb != null && outputStandard != null)
            {
                var syllabusoutputStandardProgram = new SyllabusOutputStandard()
                {
                    Syllabus = syllabusOjb,
                    OutputStandard = outputStandard
                };
                await _unitOfWork.SyllabusOutputStandardRepository.AddAsync(syllabusoutputStandardProgram);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<CreateSyllabusOutputStandardViewModel>(syllabusoutputStandardProgram);
                }
            }
            return null;
        }
        public async Task<CreateSyllabusOutputStandardViewModel> RemoveOutputStandardToSyllabus(Guid SyllabusId, Guid OutputStandardId)
        {
            var classTrainingProgram = await _unitOfWork.SyllabusOutputStandardRepository.GetSyllabusOutputStandard(SyllabusId, OutputStandardId);
            if (classTrainingProgram != null)
            {
                _unitOfWork.SyllabusOutputStandardRepository.SoftRemove(classTrainingProgram);
                var isSucces = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSucces)
                {
                    return _mapper.Map<CreateSyllabusOutputStandardViewModel>(classTrainingProgram);
                }
            }
            return null;
        }
        public async Task<Pagination<OutputStandardViewModel>> GetOutputStandardBySyllabusIdAsync(Guid SyllabusId, int pageIndex, int pageSize)
        {
            var outputStandardOjb = await _unitOfWork.OutputStandardRepository.GetOutputStandardBySyllabusIdAsync(SyllabusId, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<OutputStandardViewModel>>(outputStandardOjb);
            return result;
        }
    }
}
