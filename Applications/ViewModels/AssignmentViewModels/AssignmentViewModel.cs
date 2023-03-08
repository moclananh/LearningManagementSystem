using Domain.Enum.StatusEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.ViewModels.AssignmentViewModels
{
    public class AssignmentViewModel
    {
        public Guid Id { get; set; }
        public string AssignmentName { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Deadline { get; set; }
        public bool isDone { get; set; }
        public Status Status { get; set; }
        public Guid UnitId { get; set; }
    }
}
