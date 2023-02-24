using Applications.ViewModels.AssignmentQuestionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces
{
    public interface IAssignmentQuestionService
    {
        public Task<List<AssignmentQuestionViewModel>> GetAssignmentQuestionByAssignmentId(Guid AssignmentId);
    }
}
