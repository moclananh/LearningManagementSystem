using Applications.Commons;
using Domain.Entities;
using Domain.EntityRelationship;

namespace Applications.Repositories
{
    public interface IAssignmentQuestionRepository : IGenericRepository<AssignmentQuestion>
    {
        Task<Pagination<AssignmentQuestion>> GetAllAssignmentQuestionByAssignmentId(Guid AssignmentId, int pageNumber = 0, int pageSize = 10);
        Task UploadAssignmentListAsync(List<AssignmentQuestion> assignmentQuestionList);
    }
}
