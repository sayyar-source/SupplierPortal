using FluentValidation;
using SupplierPortal.Application.DTOs;

namespace SupplierPortal.Application.Validations;

public class CreatePurchaseRequestItemValidator : AbstractValidator<CreatePurchaseRequestItemDTO>
{
    public CreatePurchaseRequestItemValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(999999.9999m).WithMessage("Quantity is too large");

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Unit is required")
            .MaximumLength(20).WithMessage("Unit cannot exceed 20 characters");
    }
}