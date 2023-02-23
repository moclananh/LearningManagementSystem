using Domain.Enum.StatusEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.ViewModels.LectureViewModels
{
    public class UpdateLectureViewModel
    {
        public string LectureName { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Status Status { get; set; }
    }
}
