using Applications.Interfaces;
using Applications.ViewModels.AssignmentQuestionViewModels;
using Applications.ViewModels.AssignmentViewModels;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services
{
    public class AssignmentQuestionService : IAssignmentQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AssignmentQuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<AssignmentQuestionViewModel>> GetAssignmentQuestionByAssignmentId(Guid AssignmentId)
        {
            var asmQObj = await _unitOfWork.AssignmentQuestionRepository.GetAllAssignmentQuestionByAssignmentId(AssignmentId);
            var result = _mapper.Map<List<AssignmentQuestionViewModel>>(asmQObj);
            return result;
        }
    }
}
