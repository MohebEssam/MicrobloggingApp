using FluentValidation;
using MicrobloggingApp.Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;

namespace MicrobloggingApp.Infrastructure.Validators
{
    public class PostDtoValidator : AbstractValidator<PostDto>
    {
        public PostDtoValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .MaximumLength(140);

            RuleFor(x => x.Image)
                .Must(BeAValidImageFormat)
                .When(x => x.Image != null)
                .WithMessage("Only JPG, PNG, WebP images are allowed.");

            RuleFor(x => x.Image.Length)
                .LessThanOrEqualTo(2 * 1024 * 1024) // 2 MB
                .When(x => x.Image != null)
                .WithMessage("Image size must be less than 2 MB.");
        }

        private bool BeAValidImageFormat(IFormFile file)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            return allowedTypes.Contains(file.ContentType);
        }
    }
}
