using MicrobloggingApp.Web.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MicrobloggingApp.Web.Services
{
    public class PostApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _context;
        private readonly IConfiguration _config;

        public PostApiService(HttpClient httpClient, IConfiguration config, IHttpContextAccessor context)
        {
            _httpClient = httpClient;
            _context = context;
            _config = config;

            _httpClient.BaseAddress = new Uri(_config["ApiBaseUrl"]);
        }

        private void AttachJwtToken()
        {
            var jwt = _context.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "JwtToken")?.Value;
            if (string.IsNullOrWhiteSpace(jwt))
            {
                // Redirect to login or throw an exception so the controller handles it
                throw new UnauthorizedAccessException("JWT token missing. Redirecting to login.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        public async Task<List<PostDto>> GetTimelineAsync()
        {
            AttachJwtToken();

            var response = await _httpClient.GetAsync($"api/Post");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<PostDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }

        public async Task<HttpResponseMessage> CreatePostAsync(PostCreateDto post)
        {
            AttachJwtToken();

            var form = new MultipartFormDataContent();
            form.Add(new StringContent(post.Text), "Text");

            if (post.Image != null)
            {
                var streamContent = new StreamContent(post.Image.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(post.Image.ContentType);
                form.Add(streamContent, "Image", post.Image.FileName);
            }

            return await _httpClient.PostAsync("api/Post", form);
        }
    }

}
