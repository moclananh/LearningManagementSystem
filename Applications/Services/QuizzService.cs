using Application.ViewModels.QuizzViewModels;
using Applications.Commons;
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

        public async Task<QuizzViewModel> GetQuizzByQuizzIdAsync(Guid QuizzId)
        {
            var quizz = await _unitOfWork.QuizzRepository.GetByIdAsync(QuizzId);
            var result = _mapper.Map<QuizzViewModel>(quizz);
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

        public async Task<Pagination<QuizzViewModel>> GetQuizzByName(string Name, int pageIndex = 0, int pageSize = 10)
        {
            var quizzes = await _unitOfWork.QuizzRepository.GetQuizzByName(Name, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<QuizzViewModel>>(quizzes);
            return result;
        }

        public async Task<Pagination<QuizzViewModel>> GetAllQuizzes(int pageIndex = 0, int pageSize = 10)
        {
            var quizzes = await _unitOfWork.QuizzRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<QuizzViewModel>>(quizzes);
            return result;
        }

        public async Task<Pagination<QuizzViewModel>> GetEnableQuizzes(int pageIndex = 0, int pageSize = 10)
        {
            var quizzes = await _unitOfWork.QuizzRepository.GetEnableQuizzes(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<QuizzViewModel>>(quizzes);
            return result;
        }

        public async Task<Pagination<QuizzViewModel>> GetDisableQuizzes(int pageIndex = 0, int pageSize = 10)
        {
            var quizzes = await _unitOfWork.QuizzRepository.GetDisableQuizzes(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<QuizzViewModel>>(quizzes);
            return result;
        }

        public async Task<Pagination<QuizzViewModel>> GetQuizzByUnitIdAsync(Guid UnitId, int pageIndex = 0, int pageSize = 10)
        {
            var quizzes = await _unitOfWork.QuizzRepository.GetQuizzByUnitIdAsync(UnitId, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<QuizzViewModel>>(quizzes);
            return result;
        }
    }
}
