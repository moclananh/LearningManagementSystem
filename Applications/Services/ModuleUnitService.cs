using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.UnitModuleViewModel;
using AutoMapper;
using System.Net;

namespace Applications.Services
{
    public class ModuleUnitService : IModuleUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ModuleUnitService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response> GetAllModuleUnitsAsync(int pageIndex = 0, int pageSize = 10)
        {
            var moduleUnit = await _unitOfWork.ModuleUnitRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ModuleUnitViewModel>>(moduleUnit);
            var guidList = moduleUnit.Items.Select(x => x.CreatedBy).ToList();
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
            if (moduleUnit.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }
    }
}
