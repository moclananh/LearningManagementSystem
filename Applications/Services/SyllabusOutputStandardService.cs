using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
using AutoMapper;
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
    }
}
