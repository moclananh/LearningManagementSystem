using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.ViewModels.PracticeQuestionViewModels
{
    public class PracticeQuestionViewModel
    {
        public Guid PracticeId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Note { get; set; }
    }
}
