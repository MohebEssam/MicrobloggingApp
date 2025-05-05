namespace MicrobloggingApp.Infrastructure.Interfacses
{
    public interface IAuthService
    {
        string? Authenticate(string username, string password);
    }

}
