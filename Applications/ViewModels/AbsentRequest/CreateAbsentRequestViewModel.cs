

namespace Applications.ViewModels.AbsentRequest
{
    public class CreateAbsentRequestViewModel
    {
        public Guid Id { get; set; }
        public string AbsentReason { get; set; }
        public DateTime AbsentDate { get; set; }
        public bool IsAccepted {get; set; }
        public Guid UserId { get; set; }
        public Guid ClassId { get; set; }
    }
}
