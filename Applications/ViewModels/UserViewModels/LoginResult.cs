using Domain.Enum;
using Domain.Enum.RoleEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.ViewModels.UserViewModels;

public class LoginResult
{
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime DOB { get; set; }
    public string Gender { get; set; }
    public string Role { get; set; }
    public  string Status { get; set; }
    public string Token { get; set; }
}
