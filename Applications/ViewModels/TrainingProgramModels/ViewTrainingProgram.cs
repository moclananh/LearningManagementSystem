using Domain.EntityRelationship;
using Domain.Enum;
using Domain.Enum.StatusEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.TrainingProgramModels
{
    public class ViewTrainingProgram
    {
        public string TrainingProgramName { get; set; }
        public string Duration { get; set; }
        public Status Status { get; set; }

    }
}
