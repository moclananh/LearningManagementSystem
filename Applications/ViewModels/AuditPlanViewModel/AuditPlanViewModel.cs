using Domain.Enum.StatusEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.ViewModels.AuditPlanViewModel
{
    public class AuditPlanViewModel
    {
        public Guid Id { get; set; }
        public string AuditPlanName { get; set; }
        public string Description { get; set; }
        public DateTime AuditDate { get; set; }
        public string Note { get; set; }
        public Status Status { get; set; }
        public Guid ModuleId { get; set; }
        public Guid ClassId { get; set; }
    }
}
