using MicrobloggingApp.API.Controllers;
using MicrobloggingApp.Infrastructure.DTOs;
using MicrobloggingApp.Infrastructure.Interfacses;
using MicrobloggingApp.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace MicrobloggingApp.Tests
{
    public class PostControllerTests
    {
        private readonly Mock<IPostService> _mockPostService;
        private readonly PostController _controller;

        public PostControllerTests()
        {
            _mockPostService = new Mock<IPostService>();
            _controller = new PostController(_mockPostService.Object);
        }

        private void SetUserContext(Guid userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task CreatePost_ValidUser_ReturnsPostId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            SetUserContext(userId);

            var postDto = new PostDto { Text = "Hello Test" };
            _mockPostService.Setup(x => x.CreatePostAsync(postDto, userId))
                            .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _controller.CreatePost(postDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("PostId", okResult.Value!.ToString());
        }

        [Fact]
        public async Task GetPosts_ValidUser_ReturnsPosts()
        {
            // Arrange
            var userId = Guid.NewGuid();
            SetUserContext(userId);

            var postDtos = new List<Post>
            {
                new Post { Text = "Hello Test", CreatedAt = DateTime.UtcNow, UserId = userId }
            };

            _mockPostService.Setup(x => x.GetAllPostsAsync(userId))
                            .ReturnsAsync(postDtos); // ✅ Correct return type

            // Act
            var result = await _controller.GetPosts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPosts = Assert.IsAssignableFrom<IEnumerable<Post>>(okResult.Value);
            Assert.Single(returnedPosts);
        }


        [Fact]
        public async Task GetPosts_NoUserClaim_ReturnsUnauthorized()
        {
            // No user context set
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await _controller.GetPosts();

            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("User ID not found in token.", unauthorized.Value);
        }

        [Fact]
        public async Task GetPosts_InvalidUserIdFormat_ReturnsBadRequest()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "not-a-guid")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var result = await _controller.GetPosts();

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid User ID format.", badRequest.Value);
        }
    }
}
