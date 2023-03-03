using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ModuleViewModels;
using Applications.ViewModels.Response;
using Applications.ViewModels.UnitModuleViewModel;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;
using System.Net;

namespace Applications.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ModuleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateModuleViewModel?> CreateModule(CreateModuleViewModel moduleDTO)
        {
            var moduleMap = _mapper.Map<Module>(moduleDTO);
            await _unitOfWork.ModuleRepository.AddAsync(moduleMap);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if(isSuccess)
            {
                return _mapper.Map<CreateModuleViewModel>(moduleDTO);
            }
            return null;
        }

        public async Task<Response> GetAllModules(int pageIndex = 0, int pageSize = 10)
        {
            var modules = await _unitOfWork.ModuleRepository.ToPagination(pageIndex, pageSize);
            if (modules.Items.Count < 1) return new Response(HttpStatusCode.NoContent, "Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<ModuleViewModels>>(modules));
        }

        public async Task<Response> GetDisableModules(int pageIndex = 0, int pageSize = 10)
        {
            var module = await _unitOfWork.ModuleRepository.GetDisableModules();
            if (module.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<ModuleViewModels>>(module));
        }

        public async Task<Response> GetEnableModules(int pageIndex = 0, int pageSize = 10)
        {
            var module = await _unitOfWork.ModuleRepository.GetEnableModules();
            if (module.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<ModuleViewModels>>(module));
        }

        public async Task<Response> GetModuleById(Guid moduleId)
        {
            var module = await _unitOfWork.ModuleRepository.GetByIdAsync(moduleId);
            if (module == null) return new Response(HttpStatusCode.NoContent, "Id Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<ModuleViewModels>(module));
        }

        public async Task<Response> GetModulesByName(string name, int pageIndex = 0, int pageSize = 10)
        {
            var module = await _unitOfWork.ModuleRepository.GetModuleByName(name, pageIndex, pageSize);
            if (module.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<ModuleViewModels>>(module));
        }

        public async Task<Response> GetModulesBySyllabusId(Guid syllabusId, int pageIndex = 0, int pageSize = 10)
        {
            var module = await _unitOfWork.ModuleRepository.GetModulesBySyllabusId(syllabusId, pageIndex, pageSize);
            if (module.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<ModuleViewModels>>(module));
        }

        public async Task<UpdateModuleViewModel> UpdateModule(Guid moduleId, UpdateModuleViewModel moduleDTO)
        {
            var module = await _unitOfWork.ModuleRepository.GetByIdAsync(moduleId);
            if(module != null)
            {
                _mapper.Map(moduleDTO,module);
                _unitOfWork.ModuleRepository.Update(module);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if(isSuccess)
                {
                    return _mapper.Map<UpdateModuleViewModel>(module);
                }
                
            }
            return null;
        }

        public async Task<ModuleUnitViewModel> AddUnitToModule(Guid ModuleId, Guid UnitId)
        {
            var moduleOjb = await _unitOfWork.ModuleRepository.GetByIdAsync(ModuleId);
            var unitObj = await _unitOfWork.UnitRepository.GetByIdAsync(UnitId);
            if (moduleOjb != null && unitObj != null)
            {
                var moduleUnit = new ModuleUnit()
                {
                    Module = moduleOjb,
                    Unit = unitObj
                };
                await _unitOfWork.ModuleUnitRepository.AddAsync(moduleUnit );
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<ModuleUnitViewModel>(moduleUnit);
                }
            }
            return null;
        }
        public async Task<ModuleUnitViewModel> RemoveUnitToModule(Guid ModuleId, Guid UnitId)
        {
            var moduleOjb = await _unitOfWork.ModuleUnitRepository.GetModuleUnit(ModuleId, UnitId);
            if (moduleOjb != null)
            {
                _unitOfWork.ModuleUnitRepository.SoftRemove(moduleOjb);
                var isSucces = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSucces)
                {
                    return _mapper.Map<ModuleUnitViewModel>(moduleOjb);
                }
            }
            return null;
        }
    }
}
