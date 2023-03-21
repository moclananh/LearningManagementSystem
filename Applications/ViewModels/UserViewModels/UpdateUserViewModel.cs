using Domain.Enum.RoleEnum;
using Domain.Enum.LevelEnum;
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.UserViewModels;

public class UpdateUserViewModel
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string Email { get; set; }
    public DateTime DOB { get; set; }
    public bool Gender { get; set; }
    public Role Role { get; set; }
    public OverallStatus OverallStatus { get; set; }
    public string? Image { get; set; }
    public Level Level { get; set; }
}
