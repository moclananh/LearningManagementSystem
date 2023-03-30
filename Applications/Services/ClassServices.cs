using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Applications.ViewModels.ClassUserViewModels;
using Applications.ViewModels.ClassViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Enum.RoleEnum;
using Domain.Enum.StatusEnum;
using System.Net;

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

        public async Task<ClassViewModel?> CreateClass(CreateClassViewModel classDTO)
        {
            try
            {
                var classOjb = _mapper.Map<Class>(classDTO);

                if (classDTO.TraingProgramId != null)
                {
                    var trainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(classDTO.TraingProgramId);
                    var classTrainingProgram = new ClassTrainingProgram()
                    {
                        Class = classOjb,
                        TrainingProgram = trainingProgram
                    };

                    await _unitOfWork.ClassTrainingProgramRepository.AddAsync(classTrainingProgram);
                }

                var classUser = new List<ClassUser>();
                if (classDTO.AdminId != null)
                {
                    var adminList = await _unitOfWork.UserRepository.GetEntitiesByIdsAsync(classDTO.AdminId);

                    foreach (var item in adminList)
                    {
                        var adminClass = new ClassUser()
                        {
                            User = item,
                            Class = classOjb
                        };
                        classUser.Add(adminClass);
                    }
                }

                if (classDTO.TrainerId != null)
                {
                    var trainerList = await _unitOfWork.UserRepository.GetEntitiesByIdsAsync(classDTO.TrainerId);

                    foreach (var item in trainerList)
                    {
                        var trainerClass = new ClassUser()
                        {
                            User = item,
                            Class = classOjb
                        };
                        classUser.Add(trainerClass);
                    }
                }

                await _unitOfWork.ClassUserRepository.AddRangeAsync(classUser);
                await _unitOfWork.ClassRepository.AddAsync(classOjb);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;

                if (isSuccess)
                {
                    return _mapper.Map<ClassViewModel>(classOjb);
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
            var classes = await _unitOfWork.ClassRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ClassViewModel>>(classes);

            var guidList = classes.Items.Select(x => x.CreatedBy).ToList();
            var users = await _unitOfWork.UserRepository.GetEntitiesByIdsAsync(guidList);

            foreach (var item in result.Items)
            {
                if (string.IsNullOrEmpty(item.CreatedBy)) continue;

                var createdBy = users.FirstOrDefault(x => x.Id == Guid.Parse(item.CreatedBy));
                if (createdBy != null)
                {
                    item.CreatedBy = createdBy.Email;
                }
            }

            return result;
        }

        public async Task<Pagination<ClassViewModel>> GetClassByFilter(ClassFiltersViewModel filters, int pageNumber = 0, int pageSize = 10)
        {
            if (filters.StartDate is null)
            {
                filters.StartDate = new DateTime(1999, 1, 1);
            }
            if (filters.EndDate is null)
            {
                filters.EndDate = new DateTime(3999, 1, 1);
            }

            var classes = await _unitOfWork.ClassRepository.GetAllAsync();
            var itemCount = classes.Count();
            var baseQuery = classes.Where(x => x.StartDate >= filters.StartDate && x.EndDate <= filters.EndDate);

            if (filters.Location.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.Location == filters.Location);
            }
            if (filters.FSU.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.FSU == filters.FSU);
            }
            if (filters.Attendee.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.Attendee == filters.Attendee);
            }
            if (filters.Status.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.Status == filters.Status);
            }

            var items = baseQuery.OrderByDescending(x => x.CreationDate)
                                 .Skip(pageNumber * pageSize)
                                 .Take(pageSize)
                                 .ToList();

            var mapping = new Pagination<Class>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            var result = _mapper.Map<Pagination<ClassViewModel>>(mapping);

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

        public async Task<CreateClassTrainingProgramViewModel?> RemoveTrainingProgramFromClass(Guid ClassId, Guid TrainingProgramId)
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
                    await _unitOfWork.SaveChangeAsync();
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

        public async Task<Response> UpdateStatusOnlyOfClass(Guid ClassId, UpdateStatusOnlyOfClass ClassDTO)
        {
            var classObj = await _unitOfWork.ClassRepository.GetByIdAsync(ClassId);
            if (classObj != null)
            {
                _mapper.Map(ClassDTO, classObj);
                _unitOfWork.ClassRepository.Update(classObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return new Response(HttpStatusCode.OK, "update success", classObj);
                }
            }
            return new Response(HttpStatusCode.BadRequest, "Update failed");
        }
    }
}
