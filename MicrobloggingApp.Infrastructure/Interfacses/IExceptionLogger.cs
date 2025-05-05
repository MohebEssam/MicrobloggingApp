namespace MicrobloggingApp.Infrastructure.Interfacses
{
    public interface IExceptionLogger
    {
        void Log(Exception ex, string context = null);
    }

}
