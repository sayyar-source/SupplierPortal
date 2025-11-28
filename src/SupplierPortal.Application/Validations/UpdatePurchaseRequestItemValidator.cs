using FluentValidation;
using SupplierPortal.Application.DTOs;

namespace SupplierPortal.Application.Validations;

public class UpdatePurchaseRequestItemValidator : AbstractValidator<UpdatePurchaseRequestItemDTO>
{
    public UpdatePurchaseRequestItemValidator()
    {
        RuleFor(x => x.ItemId)
            .GreaterThan(0).WithMessage("Valid item ID is required");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative")
            .LessThanOrEqualTo(999999.9999m).WithMessage("Price is too large");

        RuleFor(x => x.DeliveryDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Delivery date cannot be in the past");
    }
}