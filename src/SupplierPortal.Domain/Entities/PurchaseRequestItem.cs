using SupplierPortal.Domain.Entities.Base;

namespace SupplierPortal.Domain.Entities;

public class PurchaseRequestItem : BaseEntity
{
    #region Properties

    public int PurchaseRequestId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public string Unit { get; set; } = string.Empty;
    public decimal? Price { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public bool IsPriced { get; set; }

    #endregion

    #region Navigation Properties

    public virtual PurchaseRequest PurchaseRequest { get; set; } = null!;

    #endregion

    #region Domain Methods

    public void UpdatePrice(decimal price, DateTime deliveryDate)
    {
        ValidatePriceInput(price, deliveryDate);

        Price = price;
        DeliveryDate = deliveryDate;
        IsPriced = true;
        UpdatedAt = DateTime.UtcNow;
    }
    #endregion

    #region Private Validation Methods

    private void ValidatePriceInput(decimal price, DateTime deliveryDate)
    {
        if (price < 0)
            throw new ArgumentException($"Price cannot be negative. Provided: {price}", nameof(price));

        if (deliveryDate < DateTime.UtcNow.Date)
            throw new ArgumentException(
                $"Delivery date cannot be in the past. Provided: {deliveryDate:yyyy-MM-dd}, Current: {DateTime.UtcNow:yyyy-MM-dd}",
                nameof(deliveryDate));
    }

    #endregion
}