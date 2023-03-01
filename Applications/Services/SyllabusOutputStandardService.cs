using Application.ViewModels.QuizzViewModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
using AutoMapper;
using Domain.Entities;

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
        public async Task<Pagination<SyllabusOutputStandardViewModel>> GetAllSyllabusOutputStandards(int pageIndex = 0, int pageSize = 10)
        {
            var syllabusOutputStandards = await _unitOfWork.SyllabusOutputStandardRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<SyllabusOutputStandardViewModel>>(syllabusOutputStandards);
            return result;
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
