using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.AssignmentViewModels
{
    public class UpdateAssignmentViewModel
    {
        public string AssignmentName { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Deadline { get; set; }
        public bool isDone { get; set; }
        public Status Status { get; set; }
    }
}
