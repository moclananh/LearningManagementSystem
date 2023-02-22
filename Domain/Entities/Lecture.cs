﻿using Domain.Base;
using Domain.Enum.StatusEnum;

namespace Domain.Entities
{
    public class Lecture : BaseEntity
    {
        public string LectureName { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Status Status { get; set; }
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
