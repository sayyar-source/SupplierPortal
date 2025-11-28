using SupplierPortal.Domain.Entities.Base;
using SupplierPortal.Domain.Enums;

namespace SupplierPortal.Domain.Entities;

public class PurchaseRequest : BaseEntity
{
    public string RequestNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    public PurchaseRequestStatus Status { get; set; } = PurchaseRequestStatus.Pending;
    public string? Notes { get; set; }
    public DateTime? CompletedAt { get; set; }

    public virtual Account? Account{ get; set; }
    public virtual ICollection<PurchaseRequestItem>? Items { get; set; }=new List<PurchaseRequestItem>();

    public void MarkAsCompleted()
    {
        if (Status == PurchaseRequestStatus.Completed)
            throw new InvalidOperationException("Purchase request is already completed");

        // Validate all items have price and delivery date
        if (!Items!.All(item => item.IsPriced && item.Price.HasValue && item.DeliveryDate.HasValue))
            throw new InvalidOperationException("All items must have price and delivery date before marking as completed");

        Status = PurchaseRequestStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }
    public void StartProgress()
    {
        if (Status != PurchaseRequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be moved to in progress");

        Status = PurchaseRequestStatus.InProgress;
    }

    public void UpdateItemPrice(int itemId, decimal price, DateTime deliveryDate)
    {
        if (Status == PurchaseRequestStatus.Completed)
            throw new InvalidOperationException("Cannot update completed request");

        var item = Items!.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new InvalidOperationException($"Item with ID {itemId} not found in this request");

        item.UpdatePrice(price, deliveryDate);
    }
}