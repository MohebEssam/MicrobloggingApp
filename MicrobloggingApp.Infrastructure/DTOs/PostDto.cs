using Microsoft.AspNetCore.Http;

namespace MicrobloggingApp.Infrastructure.DTOs
{
    public class PostDto
    {
        public string Text { get; set; }
        public IFormFile? Image { get; set; }
    }
}
