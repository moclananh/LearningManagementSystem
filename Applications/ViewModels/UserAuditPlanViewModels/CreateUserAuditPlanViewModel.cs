using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.ViewModels.UserAuditPlanViewModels
{
    public class CreateUserAuditPlanViewModel
    {
        Guid AuditPlanId { get; set; }
        Guid UserId { get; set; }
    }
}
