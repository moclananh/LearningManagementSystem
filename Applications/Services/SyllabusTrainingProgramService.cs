using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.Response;
using Applications.ViewModels.TrainingProgramSyllabi;
using AutoMapper;
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
    }
}
