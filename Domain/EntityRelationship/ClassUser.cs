using Domain.Base;
using Domain.Entities;

namespace Domain.EntityRelationship
{
    public class ClassUser : BaseEntity
    {
        public Guid ClassId { get; set; }
        public Guid UserId { get; set; }
        public Class Class { get; set; }
        public User User { get; set; }
    }
}
