﻿using Application.ViewModels.QuizzViewModels;
using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.SyllabusViewModels;
using AutoMapper;
using Domain.Entities;
using System.Net;

namespace Applications.Services
{
    public class QuizzService : IQuizzService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public QuizzService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> GetQuizzByQuizzIdAsync(Guid QuizzId)
        {
            var quizz = await _unitOfWork.QuizzRepository.GetByIdAsync(QuizzId);
            if (quizz == null) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search succeed", _mapper.Map<QuizzViewModel>(quizz));
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

        public async Task<Response> GetQuizzByName(string QuizzName, int pageIndex = 0, int pageSize = 10)
        {
            var quizzes = await _unitOfWork.QuizzRepository.GetQuizzByName(QuizzName, pageIndex, pageSize);
            if (quizzes.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Quizz Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<QuizzViewModel>>(quizzes));
        }

        public async Task<Response> GetAllQuizzes(int pageIndex = 0, int pageSize = 10)
        {
            var quizzes = await _unitOfWork.QuizzRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<QuizzViewModel>>(quizzes);
            var guidList = quizzes.Items.Select(x => x.CreatedBy).ToList();
            foreach (var item in result.Items)
            {
                foreach (var user in guidList)
                {
                    var createBy = await _unitOfWork.UserRepository.GetByIdAsync(user);
                    item.CreatedBy = createBy.Email;
                }
            }
            if (quizzes.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Quizz Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }

        public async Task<Response> GetEnableQuizzes(int pageIndex = 0, int pageSize = 10)
        {
            var quizzes = await _unitOfWork.QuizzRepository.GetEnableQuizzes(pageIndex, pageSize);
            if (quizzes.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Quizz Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<QuizzViewModel>>(quizzes));
        }

        public async Task<Response> GetDisableQuizzes(int pageIndex = 0, int pageSize = 10)
        {
            var quizzes = await _unitOfWork.QuizzRepository.GetDisableQuizzes(pageIndex, pageSize);
            if (quizzes.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Quizz Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<QuizzViewModel>>(quizzes));
        }

        public async Task<Response> GetQuizzByUnitIdAsync(Guid UnitId, int pageIndex = 0, int pageSize = 10)
        {
            var quizzes = await _unitOfWork.QuizzRepository.GetQuizzByUnitIdAsync(UnitId, pageIndex, pageSize);
            if (quizzes.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "Id not found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<QuizzViewModel>>(quizzes));
        }
    }
}
