﻿using Applications.Commons;
using Applications.ViewModels.OutputStandardViewModels;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
namespace Applications.Interfaces
{
    public interface IOutputStandardService
    {
        public Task<List<OutputStandardViewModel>> ViewAllOutputStandardAsync();
        public Task<OutputStandardViewModel> GetOutputStandardByOutputStandardIdAsync(Guid OutputStandardId);
        public Task<CreateOutputStandardViewModel> CreateOutputStandardAsync(CreateOutputStandardViewModel OutputStandardDTO);
        public Task<UpdateOutputStandardViewModel> UpdatOutputStandardAsync(Guid OutputStandardId, UpdateOutputStandardViewModel OutputStandardDTO);
        public Task<Pagination<OutputStandardViewModel>> GetOutputStandardBySyllabusIdAsync(Guid SyllabusId, int pageIndex = 0, int pageSize = 10);
        public Task<CreateSyllabusOutputStandardViewModel> AddOutputStandardToSyllabus(Guid SyllabusId, Guid OutputStandardId);
        public Task<CreateSyllabusOutputStandardViewModel> RemoveOutputStandardToSyllabus(Guid SyllabusId, Guid OutputStandardId);
    }
}
