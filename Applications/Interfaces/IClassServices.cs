using Applications.ViewModels.ClassViewModels;

namespace Applications.Interfaces
{
    public interface IClassServices
    {
        public Task<List<ClassViewModel>> GetAllClasses();
        public Task<List<ClassViewModel>> GetEnableClasses();
        public Task<List<ClassViewModel>> GetDisableClasses();
        public Task<ClassViewModel> GetClassById(Guid ClassId);
        public Task<List<ClassViewModel>> GetClassByName(string Name);
        public Task<ClassViewModel?> CreateClass(ClassViewModel classDTO);
        public Task<ClassViewModel?> UpdateClass(Guid ClassId, ClassViewModel classDTO);
    }
}
