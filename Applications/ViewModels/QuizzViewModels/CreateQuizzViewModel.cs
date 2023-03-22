﻿using Domain.Enum.StatusEnum;

namespace Application.ViewModels.QuizzViewModels
{
    public class CreateQuizzViewModel
    {
        public string QuizzName { get; set; }
        public string Description { get; set; }
        public double Duration { get; set; }
        public Status Status { get; set; }
        public Guid UnitId { get; set; }
    }
}
