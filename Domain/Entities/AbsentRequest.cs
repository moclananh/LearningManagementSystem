using Domain.Base;

namespace Domain.Entities
{
    public class AbsentRequest : BaseEntity
    {
        public string AbsentReason { get; set; }
        public DateTime AbsentDate { get; set; }
        public bool IsAccepted { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ClassId { get; set; }
        public Class Class { get; set; }

    }
}
