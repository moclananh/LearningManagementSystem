using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Applications.ViewModels.ClassViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;

namespace Applications.Services
{
    public class ClassServices : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ClassServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateClassTrainingProgramViewModel> AddTrainingProgramToClass(Guid ClassId, Guid TrainingProgramId)
        {
            var classOjb = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);
            var trainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(TrainingProgramId);
            if (classOjb != null && trainingProgram != null)
            {
                var classTrainingProgram = new ClassTrainingProgram()
                {
                    Class = classOjb,
                    TrainingProgram = trainingProgram
                };
                await _unitOfWork.ClassTrainingProgramRepository.AddAsync(classTrainingProgram);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<CreateClassTrainingProgramViewModel>(classTrainingProgram);
                }
            }
            return null;
        }

        public async Task<CreateClassViewModel?> CreateClass(CreateClassViewModel classDTO)
        {
            var classOjb = _mapper.Map<Class>(classDTO);
            await _unitOfWork.ClassRepository.AddAsync(classOjb);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CreateClassViewModel>(classOjb);
            }
            return null;

        }

        public async Task<Pagination<ClassViewModel>> GetAllClasses(int pageIndex = 0, int pageSize = 10)
        {
            var classes = await _unitOfWork.ClassRepository.ToPagination();
            var result = _mapper.Map<Pagination<ClassViewModel>>(classes);
            return result;
        }

        public async Task<ClassViewModel> GetClassById(Guid ClassId)
        {
            var classObj = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);
            var result = _mapper.Map<ClassViewModel>(classObj);
            return result;
        }

        public async Task<Pagination<ClassViewModel>> GetClassByName(string Name, int pageIndex = 0, int pageSize = 10)
        {
            var classes = await _unitOfWork.ClassRepository.GetClassByName(Name);
            var result = _mapper.Map<Pagination<ClassViewModel>>(classes);
            return result;
        }

        public async Task<Pagination<ClassViewModel>> GetDisableClasses(int pageIndex = 0, int pageSize = 10)
        {
            var classes = await _unitOfWork.ClassRepository.GetDisableClasses();
            var result = _mapper.Map<Pagination<ClassViewModel>>(classes);
            return result;
        }

        public async Task<Pagination<ClassViewModel>> GetEnableClasses(int pageIndex = 0, int pageSize = 10)
        {
            var classes = await _unitOfWork.ClassRepository.GetEnableClasses();
            var result = _mapper.Map<Pagination<ClassViewModel>>(classes);
            return result;
        }

        public async Task<CreateClassTrainingProgramViewModel> RemoveTrainingProgramToClass(Guid ClassId, Guid TrainingProgramId)
        {
            var classTrainingProgram = await _unitOfWork.ClassTrainingProgramRepository.GetClassTrainingProgram(ClassId, TrainingProgramId);
            if (classTrainingProgram != null)
            {
                _unitOfWork.ClassTrainingProgramRepository.SoftRemove(classTrainingProgram);
                var isSucces = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSucces)
                {
                    return _mapper.Map<CreateClassTrainingProgramViewModel>(classTrainingProgram);
                }
            }
            return null;
        }

        public async Task<UpdateClassViewModel?> UpdateClass(Guid ClassId, UpdateClassViewModel classDTO)
        {
            var classObj = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);
            if (classObj != null)
            {
                _mapper.Map(classDTO, classObj);
                _unitOfWork.ClassRepository.Update(classObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateClassViewModel>(classObj);
                }
            }
            return null;
        }
    }
}
