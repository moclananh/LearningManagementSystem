﻿using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.TrainingProgramSyllabi;
using AutoMapper;
using Domain.EntityRelationship;
using System.Net;

namespace Applications.Services
{
    public class SyllabusTrainingProgramService : ISyllabusTrainingProgramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SyllabusTrainingProgramService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> GetAllSyllabusTrainingPrograms(int pageIndex = 0, int pageSize = 10)
        {
            var syllabusTrainingPrograms = await _unitOfWork.TrainingProgramSyllabiRepository.ToPagination(pageIndex, pageSize);
            if (syllabusTrainingPrograms.Items.Count() < 1) return new Response(HttpStatusCode.NoContent, "No Syllabus Found");
            else return new Response(HttpStatusCode.OK, "Search Succeed", _mapper.Map<Pagination<TrainingProgramSyllabiView>>(syllabusTrainingPrograms));
        }
        public async Task<Response> AddMultipleSyllabusesToTrainingProgram(Guid trainingProgramId, List<Guid> SyllabusIds)
        {
            var trainingPrograms = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(trainingProgramId);
            var trainingProgramSyllabus = new List<TrainingProgramSyllabus>();
            foreach (var item in SyllabusIds)
            {
                var syllabuses = await _unitOfWork.SyllabusRepository.GetByIdAsync(item);
                if (syllabuses != null && trainingPrograms != null)
                {
                    var trainingProgramSyllabuses = new TrainingProgramSyllabus()
                    {
                        TrainingProgramId = trainingProgramId,
                        SyllabusId = item
                    };
                    trainingProgramSyllabus.Add(trainingProgramSyllabuses);
                }
                await _unitOfWork.TrainingProgramSyllabiRepository.AddRangeAsync(trainingProgramSyllabus);
            }
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return new Response(HttpStatusCode.OK, "Syllabuses Added Successfully");
            }
            return new Response(HttpStatusCode.NotFound, "TrainingProgram Not Found");
        }
    }
}
