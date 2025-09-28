using FluentValidation;

namespace WebMusic.Application.Validators
{
    public class CreateSongCommandValidator : AbstractValidator<Commands.Songs.CreateSongCommand>
    {
        public CreateSongCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.FileUrl)
                .NotEmpty().WithMessage("File URL is required")
                .MaximumLength(500).WithMessage("File URL cannot exceed 500 characters")
                .Must(BeAValidUrl).WithMessage("File URL must be a valid URL");

            RuleFor(x => x.CoverImage)
                .MaximumLength(500).WithMessage("Cover image URL cannot exceed 500 characters")
                .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.CoverImage))
                .WithMessage("Cover image URL must be a valid URL");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than 0");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID is required");
        }

        private static bool BeAValidUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return true;

            return Uri.TryCreate(url, UriKind.Absolute, out var result) &&
                   (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
        }
    }
}
