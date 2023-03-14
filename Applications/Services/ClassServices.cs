using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Applications.ViewModels.ClassUserViewModels;
using Applications.ViewModels.ClassViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Enum.ClassEnum;
using Domain.Enum.RoleEnum;
using Domain.Enum.StatusEnum;

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
            try
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
            catch (Exception)
            {
                throw new ArgumentException("Error at AddTrainingProgramToClass");
            }
        }

        public async Task<CreateClassViewModel?> CreateClass(CreateClassViewModel classDTO)
        {
            try
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
            catch (Exception)
            {
                throw new ArgumentException("Error at CreateClass");
            }
        }

        public async Task<Pagination<ClassViewModel>> GetAllClasses(int pageIndex = 0, int pageSize = 10)
        {
            var classes = await _unitOfWork.ClassRepository.ToPagination(pageIndex = 0, pageSize = 10);
            var result = _mapper.Map<Pagination<ClassViewModel>>(classes);

            return result;
        }

        public async Task<Pagination<ClassViewModel>> GetClassByFilter(LocationEnum? locations, ClassTimeEnum? classTime, Status? status, AttendeeEnum? attendee, FSUEnum? fsu, DateTime? startDate, DateTime? endDate, int pageNumber = 0, int pageSize = 10)
        {
            if (startDate == null)
            {
                startDate = new DateTime(1999, 1, 1);
            }
            if (endDate == null)
            {
                endDate = new DateTime(3999, 1, 1);
            }

            var classes = await _unitOfWork.ClassRepository.GetClassByFilter(locations, classTime, status, attendee, fsu, startDate, endDate, pageNumber = 0, pageSize = 10);
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
            var classes = await _unitOfWork.ClassRepository.GetClassByName(Name, pageIndex = 0, pageSize = 10);
            var result = _mapper.Map<Pagination<ClassViewModel>>(classes);

            return result;
        }

        public async Task<ClassDetailsViewModel> GetClassDetails(Guid ClassId)
        {
            var classObj = await _unitOfWork.ClassRepository.GetClassDetails(ClassId);
            var classView = _mapper.Map<ClassDetailsViewModel>(classObj);

            classView.Trainner = new List<User>();
            classView.SuperAdmin = new List<User>();
            classView.ClassAdmin = new List<User>();
            classView.Student = new List<User>();

            foreach (var user in classObj.ClassUsers)
            {
                var tempUser = await _unitOfWork.UserRepository.GetByIdAsync(user.UserId);
                if (tempUser.Role == Role.Trainer)
                {
                    classView.Trainner.Add(tempUser);
                }
                else if (tempUser.Role == Role.SuperAdmin)
                {
                    classView.SuperAdmin.Add(tempUser);
                }
                else if (tempUser.Role == Role.ClassAdmin)
                {
                    classView.ClassAdmin.Add(tempUser);
                }
                else if (tempUser.Role == Role.Student)
                {
                    classView.Student.Add(tempUser);
                }
            }

            var CreatedBy = await _unitOfWork.UserRepository.GetByIdAsync(classObj.CreatedBy);
            if (CreatedBy != null) { classView.CreatedBy = CreatedBy.Email; }

            var ModificationBy = await _unitOfWork.UserRepository.GetByIdAsync(classObj.ModificationBy);
            if (ModificationBy != null) { classView.ModificationBy = ModificationBy.Email; }

            var DeletedBy = await _unitOfWork.UserRepository.GetByIdAsync(classObj.DeleteBy);
            if (DeletedBy != null) { classView.DeleteBy = DeletedBy.Email; }

            return classView;
        }

        public async Task<Class?> GetClassByClassCode(string classCode) => await _unitOfWork.ClassRepository.GetClassByClassCode(classCode);

        public async Task<Pagination<ClassViewModel>> GetDisableClasses(int pageIndex = 0, int pageSize = 10)
        {
            var classes = await _unitOfWork.ClassRepository.GetDisableClasses(pageIndex = 0, pageSize = 10);
            var result = _mapper.Map<Pagination<ClassViewModel>>(classes);

            return result;
        }

        public async Task<Pagination<ClassViewModel>> GetEnableClasses(int pageIndex = 0, int pageSize = 10)
        {
            var classes = await _unitOfWork.ClassRepository.GetEnableClasses(pageIndex = 0, pageSize = 10);
            var result = _mapper.Map<Pagination<ClassViewModel>>(classes);

            return result;
        }

        public async Task<CreateClassTrainingProgramViewModel> RemoveTrainingProgramFromClass(Guid ClassId, Guid TrainingProgramId)
        {
            try
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
            catch (Exception)
            {
                throw new ArgumentException("Error at RemoveTrainingProgramToClass");
            }
        }

        public async Task<CreateClassUserViewModel> RemoveUserFromClass(Guid ClassId, Guid UserId)
        {
            try
            {
                var user = await _unitOfWork.ClassUserRepository.GetClassUser(ClassId, UserId);

                if (user != null)
                {
                    _unitOfWork.ClassUserRepository.SoftRemove(user);
                    var isSucces = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSucces)
                    {
                        return _mapper.Map<CreateClassUserViewModel>(user);
                    }
                }

                return null;
            }
            catch (Exception)
            {
                throw new ArgumentException("Error at RemoverUserFromClass");
            }
        }

        public async Task<UpdateClassViewModel?> UpdateClass(Guid ClassId, UpdateClassViewModel classDTO)
        {
            try
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
            catch (Exception)
            {
                throw new ArgumentException("Error at UpdateClass");
            }
        }

        public async Task<ClassViewModel> ApprovedClass(Guid ClassId)
        {
            try
            {
                var classObj = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);

                if (classObj != null)
                {
                    _unitOfWork.ClassRepository.Approve(classObj);
                    classObj.Status = Status.Enable;
                    _unitOfWork.SaveChangeAsync();
                    return _mapper.Map<ClassViewModel>(classObj);
                }

                return null;
            }
            catch (Exception)
            {
                throw new ArgumentException("Error at ApproverdClass");
            }
        }

        public async Task<ClassViewModel> AddUserToClass(Guid ClassId, Guid UserId)
        {
            try
            {
                var classObj = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);
                var user = await _unitOfWork.UserRepository.GetByIdAsync(UserId);
                if (classObj != null && user != null)
                {
                    var classUser = new ClassUser()
                    {
                        Class = classObj,
                        User = user
                    };
                    await _unitOfWork.ClassUserRepository.AddAsync(classUser);
                    await _unitOfWork.SaveChangeAsync();
                    return _mapper.Map<ClassViewModel>(classObj);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error from AddUserToClass:" + ex.Message);
            }
        }
    }
}
