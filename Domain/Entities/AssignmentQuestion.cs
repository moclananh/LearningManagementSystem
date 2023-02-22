using Domain.Base;

namespace Domain.Entities
{
    public class AssignmentQuestion : BaseEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Note { get; set; }
        public Guid AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
    }
}
