using FluentValidation;
using SupplierPortal.Application.DTOs;

namespace SupplierPortal.Application.Validations;

public class CreateSupplierValidator : AbstractValidator<CreateSupplierDTO>
{
    public CreateSupplierValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Supplier code is required")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Supplier title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required")
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");
    }
}
