using Domain.Entities;
using Domain.Enum.AttendenceEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.ViewModels.AttendanceViewModels
{
    public class CreateAttendanceViewModel
    {
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public AttendenceStatus Status { get; set; }
        public Guid UserId { get; set; }
        public Guid ClassId { get; set; }
    }
}
