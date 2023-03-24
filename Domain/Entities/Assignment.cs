using Domain.Base;
using Domain.Enum.StatusEnum;

namespace Domain.Entities
{
    public class Assignment : BaseEntity
    {
        public string AssignmentName { get; set; }
        public string? Description { get; set; }
        public double Duration { get; set; }
        public bool IsOnline { get; set; }
        public Status Status { get; set; }
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
        public ICollection<AssignmentQuestion> AssignmentQuestions { get; set; }
    }
}
