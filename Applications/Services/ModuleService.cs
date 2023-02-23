using Applications.Interfaces;
using Applications.ViewModels.ModuleViewModels;
using AutoMapper;
using Domain.Entities;

namespace Applications.Services
{
    public class ModuleService : IModuleServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ModuleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ModuleViewModels?> CreateModule(ModuleViewModels moduleDTO)
        {
            var moduleMap = _mapper.Map<Module>(moduleDTO);
            await _unitOfWork.ModuleRepository.AddAsync(moduleMap);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if(isSuccess)
            {
                return _mapper.Map<ModuleViewModels>(moduleDTO);
            }
            return null;
        }

        public async Task<List<ModuleViewModels>> GetDisableModules()
        {
            var module = await _unitOfWork.ModuleRepository.GetDisableModules();
            var result = _mapper.Map<List<ModuleViewModels>>(module);
            return result;
        }

        public async Task<List<ModuleViewModels>> GetEnableModules()
        {
            var module = await _unitOfWork.ModuleRepository.GetEnableModules();
            var result = _mapper.Map<List<ModuleViewModels>>(module);
            return result;
        }

        public async Task<ModuleViewModels> GetModuleById(Guid moduleId)
        {
            var module = await _unitOfWork.ModuleRepository.GetByIdAsync(moduleId);
            var result = _mapper.Map<ModuleViewModels>(module);
            return result;
        }

        public async Task<List<ModuleViewModels>> GetModulesByName(string name)
        {
            var module = await _unitOfWork.ModuleRepository.GetModuleByName(name);
            var result = _mapper.Map<List<ModuleViewModels>>(module);
            return result;
        }

        public async Task<List<ModuleViewModels>> GetModulesBySyllabusId(Guid syllabusId)
        {
            var module = await _unitOfWork.ModuleRepository.GetModulesBySyllabusId(syllabusId);
            var result = _mapper.Map<List<ModuleViewModels>>(module);
            return result;
        }

        public async Task<ModuleViewModels> UpdateModule(Guid moduleId, ModuleViewModels moduleDTO)
        {
            var module = await _unitOfWork.ModuleRepository.GetByIdAsync(moduleId);
            if(module != null)
            {
                _mapper.Map(moduleDTO,module);
                _unitOfWork.ModuleRepository.Update(module);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if(isSuccess)
                {
                    return _mapper.Map<ModuleViewModels>(module);
                }
                
            }
            return null;
        }
    }
}
