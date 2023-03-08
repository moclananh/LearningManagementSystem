using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusModuleViewModel;
using Applications.ViewModels.SyllabusViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;
using System.Net;

namespace Applications.Services
{
    public class SyllabusServices : ISyllabusServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SyllabusServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SyllabusModuleViewModel> AddSyllabusModule(Guid SyllabusId, Guid ModuleId)
        {
            var moduleOjb = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            var unitObj = await _unitOfWork.ModuleRepository.GetByIdAsync(ModuleId);
            if (moduleOjb != null && unitObj != null)
            {
                var SyllabusModule = new SyllabusModule()
                {
                    Syllabus = moduleOjb,
                    Module = unitObj
                };
                await _unitOfWork.SyllabusModuleRepository.AddAsync(SyllabusModule);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<SyllabusModuleViewModel>(SyllabusModule);
                }
            }
            return null;
        }

        public async Task<CreateSyllabusViewModel?> CreateSyllabus(CreateSyllabusViewModel SyllabusDTO)
        {
            var syllabus = _mapper.Map<Syllabus>(SyllabusDTO);
            await _unitOfWork.SyllabusRepository.AddAsync(syllabus);
            var isSucces = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSucces)
            {
                return _mapper.Map<CreateSyllabusViewModel>(syllabus);
            }
            return null;
        }

        public async Task<Response> GetAllSyllabus(int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.ToPagination(pageNumber, pageSize);
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<SyllabusViewModel>>(syllabus));

        }

        public async Task<Response> GetDisableSyllabus(int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetDisableSyllabus(pageNumber, pageSize);
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<SyllabusViewModel>>(syllabus));
        }

        public async Task<Response> GetEnableSyllabus(int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetEnableSyllabus(pageNumber, pageSize);
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<SyllabusViewModel>>(syllabus));
        }

        public async Task<Response> GetSyllabusById(Guid SyllabusId)
        {

            var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            if (syllabus == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", _mapper.Map<SyllabusViewModel>(syllabus));
        }

        public async Task<Response> GetSyllabusByName(string Name, int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByName(Name, pageNumber, pageSize);
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<SyllabusViewModel>>(syllabus));
        }

        public async Task<Response> GetSyllabusByOutputStandardId(Guid OutputStandardId, int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByOutputStandardId(OutputStandardId, pageNumber, pageSize);
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<SyllabusViewModel>>(syllabus));
        }

        public async Task<Response> GetSyllabusByTrainingProgramId(Guid TrainingProgramId, int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusByTrainingProgramId(TrainingProgramId, pageNumber, pageSize);
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<SyllabusViewModel>>(syllabus));
        }

        public async Task<UpdateSyllabusViewModel?> UpdateSyllabus(Guid SyllabusId, UpdateSyllabusViewModel SyllabusDTO)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            if (syllabus != null)
            {
                _mapper.Map(SyllabusDTO, syllabus);
                _unitOfWork.SyllabusRepository.Update(syllabus);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateSyllabusViewModel>(syllabus);
                }
            }
            return null;
        }
        public async Task<SyllabusModuleViewModel> RemoveSyllabusModule(Guid SyllabusId, Guid ModuleId)
        {
            var moduleOjb = await _unitOfWork.SyllabusModuleRepository.GetSyllabusModule(SyllabusId, ModuleId);
            if (moduleOjb != null)
            {
                _unitOfWork.SyllabusModuleRepository.SoftRemove(moduleOjb);
                var isSucces = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSucces)
                {
                    return _mapper.Map<SyllabusModuleViewModel>(moduleOjb);
                }
            }
            return null;
        }

        public async Task<Response> GetSyllabusDetails(Guid syllabusId)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetSyllabusDetails(syllabusId);
            if (syllabus == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", _mapper.Map<SyllabusViewModel>(syllabus));
        }

        public async Task<Response> GetAllSyllabusDetail(int pageNumber = 0, int pageSize = 10)
        {
            var syllabus = await _unitOfWork.SyllabusRepository.GetAllSyllabusDetail(pageNumber, pageSize);
            var result = _mapper.Map<Pagination<SyllabusViewModel>>(syllabus);
            var guidList = syllabus.Items.Select(x => x.CreatedBy).ToList();
            var userList = new List<string>();
            foreach (var item in result.Items)
            {
                foreach (var user in guidList)
                {
                    var createBy = await _unitOfWork.UserRepository.GetByIdAsync(user);
                    item.CreatedBy = createBy.Email;
                }
            }
            if (syllabus.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }
    }
}
