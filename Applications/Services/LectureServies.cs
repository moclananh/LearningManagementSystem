using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.LectureViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
using Domain.Entities;
using System.Net;

namespace Applications.Services
{
    public class LectureServices : ILectureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LectureServices(IUnitOfWork unitOfWork, IMapper mapper)
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

        public async Task<Response> GetAllLectures(int pageIndex = 0, int pageSize = 10)
        {
            var lectures = await _unitOfWork.LectureRepository.ToPagination(pageIndex, pageSize);
            if (lectures.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Lecture Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<LectureViewModel>>(lectures));
        }

        public async Task<Response> GetLectureById(Guid LectureId)
        {
            var lectures = await _unitOfWork.LectureRepository.GetByIdAsync(LectureId);
            if (lectures == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", lectures);
        }
        public async Task<Response> GetLectureByUnitId(Guid UnitId, int pageIndex = 0, int pageSize = 10)
        {
            var lectureObj = await _unitOfWork.LectureRepository.GetLectureByUnitId(UnitId, pageIndex, pageSize);
            if (lectureObj.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<LectureViewModel>>(lectureObj));
        }
        public async Task<Response> GetLectureByName(string Name, int pageIndex = 0, int pageSize = 10)
        {
            var lectures = await _unitOfWork.LectureRepository.GetLectureByName(Name, pageIndex, pageSize);
            if (lectures.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Lecture Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<LectureViewModel>>(lectures));
        }

        public async Task<Response> GetEnableLectures(int pageIndex = 0, int pageSize = 10)
        {
            var lectures = await _unitOfWork.LectureRepository.GetEnableLectures(pageIndex, pageSize);
            if (lectures.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Lecture Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<LectureViewModel>>(lectures));
        }
        public async Task<Response> GetDisableLectures(int pageIndex = 0, int pageSize = 10)
        {
            var lectures = await _unitOfWork.LectureRepository.GetDisableLectures(pageIndex, pageSize);
            if (lectures == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", _mapper.Map<Pagination<LectureViewModel>>(lectures));
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
    }
}

