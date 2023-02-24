using Applications.ViewModels.SyllabusViewModels;

namespace Applications.Interfaces
{
    public interface ISyllabusServices
    {
        Task<CreateSyllabusViewModel?> CreateSyllabus(CreateSyllabusViewModel SyllabusDTO);
        Task<UpdateSyllabusViewModel?> UpdateSyllabus(Guid SyllabusId, UpdateSyllabusViewModel SyllabusDTO);
        Task<List<SyllabusViewModel>> GetAllSyllabus();
        Task<List<SyllabusViewModel>> GetEnableSyllabus();
        Task<List<SyllabusViewModel>> GetDisableSyllabus();
        Task<SyllabusViewModel> GetSyllabusById(Guid SyllabusId);
        Task<List<SyllabusViewModel>> GetSyllabusByName(string SyllabusName);
        Task<List<SyllabusViewModel>> GetSyllabusByTrainingProgramId(Guid TrainingProgramId);
        Task<List<SyllabusViewModel>> GetSyllabusByOutputStandardId(Guid OutputStandardId);
    }
}
