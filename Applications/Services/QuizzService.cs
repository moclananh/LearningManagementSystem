using Application.ViewModels.QuizzViewModels;
using Applications.Interfaces;
using AutoMapper;
using Domain.Entities;
namespace Applications.Services
{
    public class QuizzServices : IQuizzServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public QuizzServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<QuizzViewModel>> ViewAllQuizzAsync()
        {
            var quizzes = await _unitOfWork.QuizzRepository.GetAllAsync();
            var result = _mapper.Map<List<QuizzViewModel>>(quizzes);
            return result;
        }

        public async Task<QuizzViewModel> GetQuizzByQuizzIdAsync(Guid QuizzId)
        {
            var quizz = await _unitOfWork.QuizzRepository.GetByIdAsync(QuizzId);
            var result = _mapper.Map<QuizzViewModel>(quizz);
            return result;
        }

        public async Task<List<QuizzViewModel>> GetQuizzByUnitIdAsync(Guid UnitId)
        {
            var quizz = await _unitOfWork.QuizzRepository.GetQuizzByUnitIdAsync(UnitId);
            var result = _mapper.Map<List<QuizzViewModel>>(quizz);
            return result;
        }

        public async Task<CreateQuizzViewModel> CreateQuizzAsync(CreateQuizzViewModel QuizzDTO)
        {
            var quizzOjb = _mapper.Map<Quizz>(QuizzDTO);
            await _unitOfWork.QuizzRepository.AddAsync(quizzOjb);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CreateQuizzViewModel>(quizzOjb);
            }
            return null;
        }

        public async Task<UpdateQuizzViewModel> UpdatQuizzAsync(Guid QuizzId, UpdateQuizzViewModel QuizzDTO)
        {
            var quizzObj = await _unitOfWork.QuizzRepository.GetByIdAsync(QuizzId);
            if (quizzObj != null)
            {
                _mapper.Map(QuizzDTO, quizzObj);
                _unitOfWork.QuizzRepository.Update(quizzObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateQuizzViewModel>(quizzObj);
                }
            }
            return null;
        }

        public async Task<List<QuizzViewModel>> GetQuizzByName(string Name)
        {
            var quizzes = await _unitOfWork.QuizzRepository.GetQuizzByName(Name);
            var result = _mapper.Map<List<QuizzViewModel>>(quizzes);
            return result;
        }

        public async Task<List<QuizzViewModel>> GetEnableQuizzes()
        {
            var quizzes = await _unitOfWork.QuizzRepository.GetEnableQuizzes();
            var result = _mapper.Map<List<QuizzViewModel>>(quizzes);
            return result;
        }

        public async Task<List<QuizzViewModel>> GetDisableQuizzes()
        {
            var quizzes = await _unitOfWork.QuizzRepository.GetDisableQuizzes();
            var result = _mapper.Map<List<QuizzViewModel>>(quizzes);
            return result;
        }
    }
}
