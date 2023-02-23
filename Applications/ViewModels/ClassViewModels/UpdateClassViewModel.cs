﻿using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.ClassViewModels
{
    public class UpdateClassViewModel
    {
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public Status Status { get; set; }
        public Guid? DeleteBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
