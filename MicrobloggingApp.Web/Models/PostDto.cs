namespace MicrobloggingApp.Web.Models
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? OriginalImageUrl { get; set; }
        public string? ProcessedImageUrl { get; set; }
    }
}
