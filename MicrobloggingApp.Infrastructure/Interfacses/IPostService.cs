using MicrobloggingApp.Infrastructure.DTOs;
using MicrobloggingApp.Infrastructure.Models;

namespace MicrobloggingApp.Infrastructure.Interfacses
{
    public interface IPostService
    {
        Task<Guid> CreatePostAsync(PostDto postDto, Guid userid);
        Task<IEnumerable<Post>> GetAllPostsAsync(Guid userId);
    }
}
