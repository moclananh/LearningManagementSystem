using Domain.Enum.RoleEnum;
using Domain.Enum.StatusEnum;

namespace Applications.ViewModels.UserViewModels;

public class FilterUserRequest
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public DateTime? DOB { get; set; }
    public List<Role?> Roles { get; set; } 
    public List<OverallStatus?> OverallStatus { get; set; }
    public List<bool?> Genders { get; set; }
    public string? Level { get; set; }
}