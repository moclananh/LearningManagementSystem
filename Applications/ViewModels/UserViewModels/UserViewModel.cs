using Domain.Enum;
using Domain.Enum.RoleEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.ViewModels.UserViewModels;

public class UserViewModel
{
    public Guid ID { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime DOB { get; set; }
    public string Gender { get; set; }
    public string Role { get; set; }
    public string Image { get; set; }
    public string Level { get; set; }
    public string Status { get; set; }
    public string? OverallStatus { get; set; }
    public Guid? CreatedBy { get; set; }
    public string? createByEmail { get; set; }
}