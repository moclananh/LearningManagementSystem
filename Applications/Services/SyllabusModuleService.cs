using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ModuleUnitViewModels;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusModuleViewModel;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using Domain.EntityRelationship;
using System.Net;

namespace Applications.Services
{
    public class SyllabusModuleService : ISyllabusModuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SyllabusModuleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Pagination<SyllabusModuleViewModel>> GetAllSyllabusModuleAsync(int pageIndex = 0, int pageSize = 10)
        {
            var syllabusmodule = await _unitOfWork.SyllabusModuleRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<SyllabusModuleViewModel>>(syllabusmodule);
            return result;
        }

        public async Task<Response> AddMultiModulesToSyllabus(Guid syllabusId, List<Guid> moduleIds)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(syllabusId);

            var syllabusModules = new List<SyllabusModule>();
            foreach (var item in moduleIds)
            {
                var moduleObj = await _unitOfWork.ModuleRepository.GetByIdAsync(item);
                if (syllabus != null && moduleObj != null)
                {
                    var syllabusModule = new SyllabusModule()
                    {
                        SyllabusId = syllabusId,
                        ModuleId = item
                    };
                    syllabusModules.Add(syllabusModule);
                }
            }

            await _unitOfWork.SyllabusModuleRepository.AddRangeAsync(syllabusModules);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return new Response(HttpStatusCode.OK, "Syllabus Module Added Successfully", _mapper.Map<List<SyllabusModuleViewModel>>(syllabusModules));
            }
            return new Response(HttpStatusCode.NotFound, "Module Not Found");
        }
    }
}
