using MicrobloggingApp.Infrastructure.Models;

namespace MicrobloggingApp.Infrastructure.Interfacses
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
    }
}
