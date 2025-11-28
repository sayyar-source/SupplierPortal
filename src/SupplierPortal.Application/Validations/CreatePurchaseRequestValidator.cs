using FluentValidation;
using SupplierPortal.Application.DTOs;

namespace SupplierPortal.Application.Validations;

public class CreatePurchaseRequestValidator : AbstractValidator<CreatePurchaseRequestDTO>
{
    public CreatePurchaseRequestValidator()
    {
        RuleFor(x => x.SupplierId)
            .GreaterThan(0).WithMessage("Valid supplier ID is required");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required")
            .Must(items => items.Count > 0).WithMessage("Purchase request must contain at least one item");

        RuleForEach(x => x.Items).SetValidator(new CreatePurchaseRequestItemValidator());
    }
}
