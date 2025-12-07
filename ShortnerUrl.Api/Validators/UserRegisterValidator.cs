using FluentValidation;
using ShortnerUrl.Api.Dtos.User.Request;

namespace ShortnerUrl.Api.Validators;

public class UserRegisterValidator : AbstractValidator<UserRegisterRequestDto>
{
    public UserRegisterValidator()
    {
        RuleFor(u => u.Email)
            .EmailAddress().WithMessage("The field must be a valid email address.")
            .NotNull().WithMessage("The field must be a valid email address.")
            .NotEmpty().WithMessage("The  Email field is required.");
        
        RuleFor(u => u.Username)
            .NotNull().WithMessage("The field must be a valid username.")
            .NotEmpty().WithMessage("The  Username field is required.")
            .MinimumLength(5).WithMessage("The field must have 5 characters.")
            .MaximumLength(255).WithMessage("The field must have 255 characters.");
        
        RuleFor(u => u.Password)
            .NotNull().WithMessage("The field must be a valid password.")
            .NotEmpty().WithMessage("The  Password field is required.")
            .MinimumLength(5).WithMessage("The field must have 5 characters.")
            .MaximumLength(255).WithMessage("The field must have 255 characters.");
        
        RuleFor(u => u.ConfirmPassword)
            .NotNull().WithMessage("The field must be a valid password.")
            .NotEmpty().WithMessage("The  ConfirmPassword field is required.")
            .Equal(u => u.Password).WithMessage("The password and confirmation password do not match.");
    }
}