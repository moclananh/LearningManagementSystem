using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.OutputStandardViewModels;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;
using System.Net;

namespace Applications.Services
{
    public class OutputStandardService : IOutputStandardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OutputStandardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response> GetOutputStandardByOutputStandardIdAsync(Guid OutputStandardId)
        {
            var outputStandard = await _unitOfWork.OutputStandardRepository.GetByIdAsync(OutputStandardId);
            var result = _mapper.Map<OutputStandardViewModel>(outputStandard);
            var createBy = await _unitOfWork.UserRepository.GetByIdAsync(outputStandard.CreatedBy);
            if (createBy != null)
            {
                result.CreatedBy = createBy.Email;
            }
            if (outputStandard == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed",result);
        }
        public async Task<CreateOutputStandardViewModel> CreateOutputStandardAsync(CreateOutputStandardViewModel OutputStandardDTO)
        {
            var outputStandardOjb = _mapper.Map<OutputStandard>(OutputStandardDTO);
            await _unitOfWork.OutputStandardRepository.AddAsync(outputStandardOjb);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CreateOutputStandardViewModel>(outputStandardOjb);
            }
            return null;
        }
        public async Task<UpdateOutputStandardViewModel> UpdatOutputStandardAsync(Guid OutputStandardId, UpdateOutputStandardViewModel OutputStandardDTO)
        {
            var outputStandardOjb = await _unitOfWork.OutputStandardRepository.GetByIdAsync(OutputStandardId);
            if (outputStandardOjb != null)
            {
                _mapper.Map(OutputStandardDTO, outputStandardOjb);
                _unitOfWork.OutputStandardRepository.Update(outputStandardOjb);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateOutputStandardViewModel>(outputStandardOjb);
                }
            }
            return null;
        }
        public async Task<CreateSyllabusOutputStandardViewModel> AddOutputStandardToSyllabus(Guid SyllabusId, Guid OutputStandardId)
        {
            var syllabusOjb = await _unitOfWork.SyllabusRepository.GetByIdAsync(SyllabusId);
            var outputStandard = await _unitOfWork.OutputStandardRepository.GetByIdAsync(OutputStandardId);
            if (syllabusOjb != null && outputStandard != null)
            {
                var syllabusoutputStandardProgram = new SyllabusOutputStandard()
                {
                    Syllabus = syllabusOjb,
                    OutputStandard = outputStandard
                };
                await _unitOfWork.SyllabusOutputStandardRepository.AddAsync(syllabusoutputStandardProgram);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<CreateSyllabusOutputStandardViewModel>(syllabusoutputStandardProgram);
                }
            }
            return null;
        }
        public async Task<CreateSyllabusOutputStandardViewModel> RemoveOutputStandardToSyllabus(Guid SyllabusId, Guid OutputStandardId)
        {
            var syllabusOutputStandard = await _unitOfWork.SyllabusOutputStandardRepository.GetSyllabusOutputStandard(SyllabusId, OutputStandardId);
            if (syllabusOutputStandard != null && !syllabusOutputStandard.IsDeleted)
            {
                syllabusOutputStandard.IsDeleted = true;
                _unitOfWork.SyllabusOutputStandardRepository.Update(syllabusOutputStandard);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<CreateSyllabusOutputStandardViewModel>(syllabusOutputStandard);
                }
            }
            return null;
        }

        public async Task<Response> GetOutputStandardBySyllabusIdAsync(Guid SyllabusId, int pageIndex, int pageSize)
        {
            var outputStandard = await _unitOfWork.OutputStandardRepository.GetOutputStandardBySyllabusIdAsync(SyllabusId, pageIndex, pageSize);
            if (outputStandard.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<OutputStandardViewModel>>(outputStandard));
        }

        public async Task<Response> GetAllOutputStandardAsync(int pageIndex = 0, int pageSize = 10)
        {
            var outputStandard = await _unitOfWork.OutputStandardRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<OutputStandardViewModel>>(outputStandard);
            var guidList = outputStandard.Items.Select(x => x.CreatedBy).ToList();
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
            if (outputStandard.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Not Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }
    }
}
