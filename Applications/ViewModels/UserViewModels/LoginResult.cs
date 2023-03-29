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
    public Guid ID { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string Email { get; set; }
    public string Image { get; set; }
    public string Role { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
