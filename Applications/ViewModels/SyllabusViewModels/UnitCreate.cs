using Domain.Enum.StatusEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.ViewModels.SyllabusViewModels
{
    public class UnitCreate
    {
        public string UnitName { get; set; }
        public LectureCreate Lecture { get; set; }
        public AssignmentCreate Assignment { get; set; }
        public QuizzCreate Quzizz { get; set; }
    }
}
