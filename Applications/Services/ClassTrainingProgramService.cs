using Applications.Commons;
using Applications.Interfaces;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Applications.ViewModels.Response;
using AutoMapper;
using Domain.Entities;
using Domain.EntityRelationship;
using System.Net;

namespace Applications.Services
{
    public class ClassTrainingProgramService : IClassTrainingProgramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClassTrainingProgramService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> GetAllClassTrainingProgram(int pageIndex = 0, int pageSize = 10)
        {
            var cltrainingp = await _unitOfWork.ClassTrainingProgramRepository.ToPagination(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<ClassTrainingProgramViewModel>>(cltrainingp);
            var guidList = cltrainingp.Items.Select(x => x.CreatedBy).ToList();

            foreach (var item in result.Items)
            {
                foreach (var user in guidList)
                {
                    var createBy = await _unitOfWork.UserRepository.GetByIdAsync(user);
                    if (createBy != null)
                    {
                        item.CreatedBy = createBy.Email;
                    }
                }
            }
            if (cltrainingp.Items.Count() < 1)
                return new Response(HttpStatusCode.NoContent, "No Class Training Program Found");
            else
                return new Response(HttpStatusCode.OK, "Search Succeed", result);
        }
    }
}
