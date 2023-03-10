using Domain.Enum.LevelEnum;
using Domain.Enum.RoleEnum;
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.UserViewModels;

public class FilterUserRequest
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateTime? DOB { get; set; }
    public List<Role?> Roles { get; set; } = new List<Role?>();
    public List<OverallStatus?> OverallStatus { get; set; } = new List<OverallStatus?>();
    public List<bool?> Genders { get; set; } = new List<bool?>();
    public List<Level?> Levels { get; set; } = new List<Level?>();

}