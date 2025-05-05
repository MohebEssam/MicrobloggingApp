using MicrobloggingApp.Infrastructure.Models;

namespace MicrobloggingApp.Infrastructure.Interfacses
{
    public interface IPostRepository
    {
        Task<Post> CreatePostAsync(Post post);
        Task<List<Post>> GetAllPostsAsync(Guid userId);
        Task<Post> GetPostByIdAsync(int postId);
        Task<Post> UpdatePostAsync(Post post);
        Task DeletePostAsync(int postId);
    }
}
