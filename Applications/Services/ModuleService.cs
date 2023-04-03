using Applications;
using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Applications.ViewModels.ModuleUnitViewModels;
using Applications.ViewModels.ModuleViewModels;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
using Applications.ViewModels.UnitModuleViewModel;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.VariantTypes;
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
            var module = await _unitOfWork.ModuleRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ModuleViewModels>>(module);

            var guidList = module.Items.Select(x => x.CreatedBy).ToList();
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
            if (module.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
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
            var result = _mapper.Map<ModuleViewModels>(module);
            var createBy = await _unitOfWork.UserRepository.GetByIdAsync(module.CreatedBy);
            result.CreatedBy = createBy.Email;
            if (module == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", result);
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

        public async Task<Response> AddMultipleUnitToModule(Guid ModuleId, List<Guid> UnitId)
        {
            var moduleOjb = await _unitOfWork.ModuleRepository.GetByIdAsync(ModuleId);
            var moduleUnits = new List<ModuleUnit>();
            foreach (var item in UnitId)
            {
                var unitObj = await _unitOfWork.UnitRepository.GetByIdAsync(item);
                if (moduleOjb != null && unitObj != null)
                {
                    var moduleUnit = new ModuleUnit()
                    {
                        ModuleId = ModuleId,
                        UnitId = item
                    };
                    moduleUnits.Add(moduleUnit);
                }
            }
            await _unitOfWork.ModuleUnitRepository.AddRangeAsync(moduleUnits);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return new Response(HttpStatusCode.OK, "Output Standards Added Successfully", _mapper.Map<List<CreateModuleUnitViewModel>>(moduleUnits));
            }
            return new Response(HttpStatusCode.NotFound, "Module Not Found");
        }

        public async Task<CreateModuleUnitViewModel> AddUnitToModule(Guid ModuleId, Guid UnitId)
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
                await _unitOfWork.ModuleUnitRepository.AddAsync(moduleUnit);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<CreateModuleUnitViewModel>(moduleUnit);
                }
            }
            return null;
        }

        public async Task<ModuleUnitViewModel> RemoveUnitToModule(Guid ModuleId, Guid UnitId)
        {
            var ModuleUnit = await _unitOfWork.ModuleUnitRepository.GetModuleUnit(ModuleId, UnitId);
            if (ModuleUnit != null && !ModuleUnit.IsDeleted)
            {
                ModuleUnit.IsDeleted = true;
                _unitOfWork.ModuleUnitRepository.Update(ModuleUnit);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<ModuleUnitViewModel>(ModuleUnit);
                }
            }
            return null;
        }
    }
}





