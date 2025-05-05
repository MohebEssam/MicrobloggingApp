namespace MicrobloggingApp.Infrastructure.Interfacses
{
    public interface IQueueService
    {
        Task SendMessageAsync(string message);
    }
}
