using Domain.Base;
using Domain.Enum.AttendenceEnum;

namespace Domain.Entities
{
    public class Attendance : BaseEntity
    {
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public AttendenceStatus Status { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ClassId { get; set; }
        public Class Class { get; set; }
    }
}
