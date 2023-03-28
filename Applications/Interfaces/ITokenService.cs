using Applications.ViewModels.TokenViewModels;

namespace Applications.Interfaces;

public interface ITokenService
{
    Task<TokenModel> GetToken(string email);
}
