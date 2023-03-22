using Domain.Enum.StatusEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.ViewModels.SyllabusViewModels
{
    public class LectureCreate
    {
        public string LectureName { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
    }
}
