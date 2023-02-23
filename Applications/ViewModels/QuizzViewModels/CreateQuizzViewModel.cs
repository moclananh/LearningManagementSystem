using Domain.Entities;
using Domain.Enum;
using Domain.Enum.StatusEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.QuizzViewModels
{
    public class CreateQuizzViewModel
    {
        public string QuizzName { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Deadline { get; set; }
        public bool isDone { get; set; }
        public Status Status { get; set; }
        public Guid UnitId { get; set; }
    }
}
