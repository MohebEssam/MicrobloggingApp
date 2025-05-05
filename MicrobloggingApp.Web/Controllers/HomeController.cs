using MicrobloggingApp.Web.Models;
using MicrobloggingApp.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicrobloggingApp.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly PostApiService _apiService;

        public HomeController(PostApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _apiService.GetTimelineAsync();
            return View(posts);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostCreateDto postDto)
        {
            var response = await _apiService.CreatePostAsync(postDto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Failed to create post");
            return RedirectToAction(nameof(Index));
        }
    }

}
