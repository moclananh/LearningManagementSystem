﻿using Applications.ViewModels.ModuleViewModels;

namespace Applications.Interfaces
{
    public interface IModuleServices
    {
        public Task<List<ModuleViewModels>> GetEnableModules();
        public Task<List<ModuleViewModels>> GetDisableModules();
        public Task<List<ModuleViewModels>> GetModulesBySyllabusId(Guid syllabusId);
        public Task<ModuleViewModels> GetModuleById(Guid moduleId);
        public Task<List<ModuleViewModels>> GetModulesByName(string name);
        public Task<ModuleViewModels?> CreateModule(ModuleViewModels moduleDTO);
        public Task<ModuleViewModels> UpdateModule(Guid moduleId, ModuleViewModels moduleDTO);
    }
}