using Applications.Interfaces;
using Applications;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIs.Services;

public class TokenService : ITokenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public TokenService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<string> GetToken(string email)
    {
        var user = (await _unitOfWork.UserRepository.Find(x => x.Email == email)).FirstOrDefault();

        if (user is null)
        {
            return null;
        }

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name,user.Email),
            new Claim("userID",user.Id.ToString()),
            new Claim("firstName",user.firstName),
            new Claim("lastName",user.lastName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:SecretKey").Value!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: authClaims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
