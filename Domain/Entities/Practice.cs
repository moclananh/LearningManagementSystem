﻿
using Domain.Base;
using Domain.Enum.StatusEnum;

namespace Domain.Entities
{
    public class Practice : BaseEntity
    {
        public string PracticeName { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Deadline { get; set; }
        public bool isDone { get; set; }
        public Status Status { get; set; }
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
        public ICollection<PracticeQuestion> PracticeQuestions { get; set; }
    }
}
