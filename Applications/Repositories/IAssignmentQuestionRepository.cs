using Domain.Entities;
using Domain.EntityRelationship;

namespace Applications.Repositories
{
    public interface IAssignmentQuestionRepository : IGenericRepository<AssignmentQuestion>
    {
        Task<List<AssignmentQuestion>> GetAllAssignmentQuestionByAssignmentId(Guid AssignmentId);
        Task UploadAssignmentListAsync(List<AssignmentQuestion> assignmentQuestionList);
    }
}
