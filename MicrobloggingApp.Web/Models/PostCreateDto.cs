namespace MicrobloggingApp.Web.Models
{
    public class PostCreateDto
    {
        public string Text { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}
