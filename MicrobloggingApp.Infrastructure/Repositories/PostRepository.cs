using MicrobloggingApp.Infrastructure.Data;
using MicrobloggingApp.Infrastructure.Interfacses;
using MicrobloggingApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace MicrobloggingApp.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        // Create a new post
        public async Task<Post> CreatePostAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        // Get all posts
        public async Task<List<Post>> GetAllPostsAsync(Guid userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        // Get post by ID
        public async Task<Post> GetPostByIdAsync(int postId)
        {
            return await _context.Posts.FindAsync(postId);
        }

        // Update a post
        public async Task<Post> UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }

        // Delete a post
        public async Task DeletePostAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
