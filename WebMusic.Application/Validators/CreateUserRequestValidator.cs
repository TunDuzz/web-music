using FluentValidation;

namespace WebMusic.Application.Validators
{
    public class CreateUserRequestValidator : AbstractValidator<Services.CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be a valid email address")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            RuleFor(x => x.Role)
                .Must(BeAValidRole).When(x => !string.IsNullOrEmpty(x.Role))
                .WithMessage("Role must be User, Admin, or Moderator");
        }

        private static bool BeAValidRole(string? role)
        {
            if (string.IsNullOrEmpty(role))
                return true;

            var validRoles = new[] { "User", "Admin", "Moderator" };
            return validRoles.Contains(role);
        }
    }
}
