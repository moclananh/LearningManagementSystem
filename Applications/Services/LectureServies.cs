using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.LectureViewModels;
using AutoMapper;
using Domain.Entities;

namespace Applications.Services
{
    public class LectureServies : ILectureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LectureServies(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateLectureViewModel?> CreateLecture(CreateLectureViewModel lectureDTO)
        {
            var lectureOjb = _mapper.Map<Lecture>(lectureDTO);
            await _unitOfWork.LectureRepository.AddAsync(lectureOjb);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<CreateLectureViewModel>(lectureOjb);
            }
            return null;

        }

        public async Task<List<LectureViewModel>> GetAllLectures()
        {
            var lectures = await _unitOfWork.LectureRepository.GetAllAsync();
            var result = _mapper.Map<List<LectureViewModel>>(lectures);
            return result;
        }

        public async Task<LectureViewModel> GetLectureById(Guid LectureId)
        {
            var lectureObj = await _unitOfWork.LectureRepository.GetByIdAsync(LectureId);
            var result = _mapper.Map<LectureViewModel>(lectureObj);
            return result;
        }
        public async Task<List<LectureViewModel>> GetLectureByUnitId(Guid UnitId)
        {
            var lectureObj = await _unitOfWork.LectureRepository.GetLectureByUnitId(UnitId);
            var result = _mapper.Map<List<LectureViewModel>>(lectureObj);
            return result;
        }
        public async Task<List<LectureViewModel>> GetLectureByName(string Name)
        {
            var lectures = await _unitOfWork.LectureRepository.GetLectureByName(Name);
            var result = _mapper.Map<List<LectureViewModel>>(lectures);
            return result;
        }

        public async Task<List<LectureViewModel>> GetDisableLectures()
        {
            var lectures = await _unitOfWork.LectureRepository.GetDisableLectures();
            var result = _mapper.Map<List<LectureViewModel>>(lectures);
            return result;
        }

        public async Task<List<LectureViewModel>> GetEnableLectures()
        {
            var lectures = await _unitOfWork.LectureRepository.GetEnableLectures();
            var result = _mapper.Map<List<LectureViewModel>>(lectures);
            return result;
        }

        public async Task<UpdateLectureViewModel?> UpdateLecture(Guid LectureId, UpdateLectureViewModel lectureDTO)
        {
            var lectureObj = await _unitOfWork.LectureRepository.GetByIdAsync(LectureId);
            if (lectureObj != null)
            {
                _mapper.Map(lectureDTO, lectureObj);
                _unitOfWork.LectureRepository.Update(lectureObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<UpdateLectureViewModel>(lectureObj);
                }
            }
            return null;
        }
        public async Task<Pagination<LectureViewModel>> GetLecturePagingsionAsync(int pageIndex = 0, int pageSize = 10)
        {
            var lectures = await _unitOfWork.LectureRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<LectureViewModel>>(lectures);
            return result;
        }
    }
}

