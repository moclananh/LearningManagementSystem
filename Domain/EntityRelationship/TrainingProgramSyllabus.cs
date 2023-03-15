using Domain.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.EntityRelationship
{
    public class TrainingProgramSyllabus : BaseEntity
    {
        public Guid TrainingProgramId { get; set; }
        public Guid SyllabusId { get; set; }
        public TrainingProgram TrainingProgram { get; set; }
        public Syllabus Syllabus { get; set; }

    }
}
