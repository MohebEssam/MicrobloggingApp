using MicrobloggingApp.API.Controllers;
using MicrobloggingApp.API.Extensions;
using MicrobloggingApp.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MicrobloggingApp.Tests
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            var jwtOptions = Options.Create(new JwtOptions
            {
                SecretKey = "Microblogging App Secret Key, This is Used to Sign and Verify JWT Tokens", // Use a secure key in real apps
                Issuer = "Micro-blogging-app-api",
                Audience = "Micro-blogging-apps",
                TokenDuration = 60
            });

            _controller = new AuthController(jwtOptions);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsToken()
        {
            var loginDto = new LoginDto
            {
                Username = "admin",
                Password = "password"
            };

            var result = _controller.Login(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            Assert.Contains("token", okResult.Value!.ToString());
        }

        [Fact]
        public void Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            var loginDto = new LoginDto
            {
                Username = "wronguser",
                Password = "wrongpass"
            };

            var result = _controller.Login(loginDto);

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
