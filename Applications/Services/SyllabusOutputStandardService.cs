using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
using AutoMapper;
using Domain.EntityRelationship;
using System.Net;

namespace Applications.Services
{
    public class SyllabusOutputStandardService : ISyllabusOutputStandardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SyllabusOutputStandardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response> GetAllSyllabusOutputStandards(int pageIndex = 0, int pageSize = 10)
        {
            var syllabusOutputStandards = await _unitOfWork.SyllabusOutputStandardRepository.ToPagination(pageIndex, pageSize);
            if (syllabusOutputStandards.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<SyllabusOutputStandardViewModel>>(syllabusOutputStandards));
        }

        public async Task<SyllabusOutputStandardViewModel> UpdatSyllabusOutputStandardAsync(Guid SyllabusOutputStandardId, Guid OutputStandardId, SyllabusOutputStandardViewModel SyllabusOutputStandardDTO)
        {
            var syllabusOutputStandardObj = await _unitOfWork.SyllabusOutputStandardRepository.GetSyllabusOutputStandard(SyllabusOutputStandardId, OutputStandardId);
            if (syllabusOutputStandardObj != null)
            {
                _mapper.Map(SyllabusOutputStandardDTO, syllabusOutputStandardObj);
                _unitOfWork.SyllabusOutputStandardRepository.Update(syllabusOutputStandardObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<SyllabusOutputStandardViewModel>(syllabusOutputStandardObj);
                }
            }
            return null;
        }

        public async Task<Response> AddMultipleOutputStandardsToSyllabus(Guid syllabusId, List<Guid> outputStandardIds)
        {
            var syllabusObj = await _unitOfWork.SyllabusRepository.GetByIdAsync(syllabusId);
            if (syllabusObj == null)
            {
                return new Response(HttpStatusCode.NotFound, "Syllabus Not Found");
            }

            foreach (var outputStandardId in outputStandardIds)
            {
                var outputStandardObj = await _unitOfWork.OutputStandardRepository.GetByIdAsync(outputStandardId);
                if (outputStandardObj == null)
                {
                    return new Response(HttpStatusCode.NotFound, "Output Standard Not Found");
                }

                var syllabusOutputStandardObj = new SyllabusOutputStandard
                {
                    SyllabusId = syllabusId,
                    OutputStandardId = outputStandardId
                };
                await _unitOfWork.SyllabusOutputStandardRepository.AddAsync(syllabusOutputStandardObj);
            }

            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return new Response(HttpStatusCode.OK, "Output Standards Added Successfully");
            }
            else
            {
                return new Response(HttpStatusCode.InternalServerError, "Failed to save changes to the database");
            }
        }
    }
}
