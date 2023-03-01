using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using AutoMapper;
using Domain.EntityRelationship;

namespace Applications.Services
{
    public class ClassTrainingProgramService : IClassTrainingProgramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClassTrainingProgramService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Pagination<ClassTrainingProgramViewModel>> GetAllClassTrainingProgram(int pageIndex = 0, int pageSize = 10)
        {
            var cltrainingp = await _unitOfWork.ClassTrainingProgramRepository.GetAllClassTrainingProgram(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ClassTrainingProgramViewModel>>(cltrainingp);
            return result;
        }
    }
}
