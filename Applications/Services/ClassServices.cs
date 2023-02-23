using Applications.Interfaces;
using Applications.ViewModels.ClassViewModels;
using AutoMapper;
using Domain.Entities;

namespace Applications.Services
{
    public class ClassServices : IClassServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ClassServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<List<ClassViewModel>> GetAllClasses()
        {
            var classes = await _unitOfWork.ClassRepository.GetAllAsync();
            var result = _mapper.Map<List<ClassViewModel>>(classes);
            return result;
        }

        public async Task<ClassViewModel> GetClassById(Guid ClassId)
        {
            var classObj = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);
            var result = _mapper.Map<ClassViewModel>(classObj);
            return result;
        }

        public async Task<List<ClassViewModel>> GetClassByName(string Name)
        {
            var classes = await _unitOfWork.ClassRepository.GetClassByName(Name);
            var result = _mapper.Map<List<ClassViewModel>>(classes);
            return result;
        }

        public async Task<List<ClassViewModel>> GetDisableClasses()
        {
            var classes = await _unitOfWork.ClassRepository.GetDisableClasses();
            var result = _mapper.Map<List<ClassViewModel>>(classes);
            return result;
        }

        public async Task<List<ClassViewModel>> GetEnableClasses()
        {
            var classes = await _unitOfWork.ClassRepository.GetEnableClasses();
            var result = _mapper.Map<List<ClassViewModel>>(classes);
            return result;
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
