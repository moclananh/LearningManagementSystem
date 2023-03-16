using Domain.Enum;
using Domain.Enum.RoleEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum.LevelEnum;

namespace Applications.ViewModels.UserViewModels;

public class UpdateUserViewModel
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime DOB { get; set; }
    public bool Gender { get; set; }
    public Role Role { get; set; }

    public string Image { get; set; }
    public Level Level { get; set; }
}
