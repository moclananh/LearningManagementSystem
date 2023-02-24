using Applications.Interfaces;
using Applications.ViewModels.OutputStandardViewModels;
using AutoMapper;
using Domain.Entities;
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
        public async Task<List<OutputStandardViewModel>> ViewAllOutputStandardAsync()
        {
            var outputStandard = await _unitOfWork.OutputStandardRepository.GetAllAsync();
            var result = _mapper.Map<List<OutputStandardViewModel>>(outputStandard);
            return result;
        }

        public async Task<OutputStandardViewModel> GetOutputStandardByOutputStandardIdAsync(Guid OutputStandardId)
        {
            var outputStandard = await _unitOfWork.OutputStandardRepository.GetByIdAsync(OutputStandardId);
            var result = _mapper.Map<OutputStandardViewModel>(outputStandard);
            return result;
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

    }
}
