using MicrobloggingApp.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace MicrobloggingApp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiBaseUrl"] + "api/auth/login";

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(dto);
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<LoginResultDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var token = result?.Token;
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(string.Empty, "Failed to retrieve token.");
                return View(dto);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dto.Username),
                new Claim("JwtToken", token)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign out of the authentication system
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Remove the jwt_token cookie (if it's set)
            Response.Cookies.Delete("jwt_token");

            // Optionally, you can also remove other session cookies or session-related information
            // Example: Response.Cookies.Delete("another_cookie");

            return RedirectToAction("Login", "Auth"); // Redirecting back to the login page
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View(); // returns Views/Auth/AccessDenied.cshtml
        }
    }
}
