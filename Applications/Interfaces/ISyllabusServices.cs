using Applications.Commons;
using Applications.ViewModels.SyllabusViewModels;

namespace Applications.Interfaces
{
    public interface ISyllabusServices
    {
        public Task<CreateSyllabusViewModel?> CreateSyllabus(CreateSyllabusViewModel SyllabusDTO);
        public Task<UpdateSyllabusViewModel?> UpdateSyllabus(Guid SyllabusId, UpdateSyllabusViewModel SyllabusDTO);
        public Task<Pagination<SyllabusViewModel>> GetAllSyllabus(int pageNumber = 0, int pageSize = 10);
        public Task<Pagination<SyllabusViewModel>> GetEnableSyllabus(int pageNumber = 0, int pageSize = 10);
        public Task<Pagination<SyllabusViewModel>> GetDisableSyllabus(int pageNumber = 0, int pageSize = 10);
        public Task<SyllabusViewModel> GetSyllabusById(Guid SyllabusId);
        public Task<Pagination<SyllabusViewModel>> GetSyllabusByName(string SyllabusName, int pageNumber = 0, int pageSize = 10);
        public Task<Pagination<SyllabusViewModel>> GetSyllabusByTrainingProgramId(Guid TrainingProgramId, int pageNumber = 0, int pageSize = 10);
        public Task<Pagination<SyllabusViewModel>> GetSyllabusByOutputStandardId(Guid OutputStandardId, int pageNumber = 0, int pageSize = 10);
    }
}
