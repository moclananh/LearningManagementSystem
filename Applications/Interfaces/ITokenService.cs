namespace Applications.Interfaces;

public interface ITokenService
{
    Task<string> GetToken(string email);
}
