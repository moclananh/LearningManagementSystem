using Applications.Interfaces;
using System.Security.Claims;

namespace APIs.Services;

public class ClaimsService : IClaimService
{
    public ClaimsService(IHttpContextAccessor httpContextAccessor)
    {
        var Id = httpContextAccessor.HttpContext?.User?.FindFirstValue("userID");
        GetCurrentUserId = string.IsNullOrEmpty(Id) ? Guid.Empty : Guid.Parse(Id);
    }
    public Guid GetCurrentUserId { get; }
    
}