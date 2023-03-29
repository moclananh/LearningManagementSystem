using Applications.Interfaces;
using Applications;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Applications.Utils;
using Applications.ViewModels.TokenViewModels;
using Domain.Entities;

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

    public async Task<TokenModel> GetToken(string email)
    {
        var user = (await _unitOfWork.UserRepository.Find(x => x.Email == email)).FirstOrDefault();

        if (user is null)
        {
            return null;
        }

        var authClaims = new List<Claim>
        {
            new Claim("userID",user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:SecretKey").Value!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: authClaims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
            );
        
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = StringUtils.RandomString();
        var refreshTokenEntity = new RefreshToken()
        {
            Id = Guid.NewGuid(),
            JwtId = token.Id,
            UserId = user.Id,
            Token = refreshToken,
            IsUsed = false,
            IsRevoked = false,
            IssuedAt = DateTime.UtcNow,
            ExpiredAt = DateTime.UtcNow.AddDays(10)
        };
        await _unitOfWork.RefreshTokenRepository.AddAsync(refreshTokenEntity);
        var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
        if (isSuccess)
        {
            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        return new TokenModel
        {
            AccessToken = string.Empty,
            RefreshToken = string.Empty
        };
    }
    
}
