namespace BlogMVC.Services.Interfaces
{
    public interface ITokenService
    {
        string GetTokenFromCookie();
        void RemoveTokenCookie();
        void StoreUserToken(string token);
        object CreateUserToken(string login, string password, string role);
    }
}
