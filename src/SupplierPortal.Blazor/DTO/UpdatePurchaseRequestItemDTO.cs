namespace SupplierPortal.Blazor.DTO;

public class UpdatePurchaseRequestItemDTO
{
    public int ItemId { get; set; }
    public decimal? Price { get; set; }
    public DateTime? DeliveryDate { get; set; }
}
