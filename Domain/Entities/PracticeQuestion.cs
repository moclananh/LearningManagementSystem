using Domain.Base;

namespace Domain.Entities
{
    public class PracticeQuestion : BaseEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Note { get; set; }
        public Guid PracticeId { get; set; }
        public Practice Practice { get; set; }
    }
}
