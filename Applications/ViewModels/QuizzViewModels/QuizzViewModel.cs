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
    public class QuizzViewModel
    {
        public Guid QuizzId { get; set; }
        public Guid UnitId { get; set; }
        public string QuizzName { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Deadline { get; set; }
        public bool isDone { get; set; }
        public Status Status { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? ModificationBy { get; set; }
        public bool IsDeleted { get; set; }

    }
}
