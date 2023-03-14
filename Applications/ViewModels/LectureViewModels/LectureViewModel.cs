using Domain.Entities;
using Domain.Enum.StatusEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.ViewModels.LectureViewModels
{
    public class LectureViewModel
    {
        public Guid Id { get; set; }
        public string LectureName { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Status Status { get; set; }
        public Guid UnitId { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
