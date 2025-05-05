using MicrobloggingApp.Infrastructure.DTOs;
using MicrobloggingApp.Infrastructure.Interfacses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MicrobloggingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromForm] PostDto postDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var postId = await _postService.CreatePostAsync(postDto, Guid.Parse(userId));
            return Ok(new { PostId = postId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPosts()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return Unauthorized("User ID not found in token.");
            }

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid User ID format.");
            }

            var posts = await _postService.GetAllPostsAsync(userId);
            return Ok(posts);
        }
    }
}
