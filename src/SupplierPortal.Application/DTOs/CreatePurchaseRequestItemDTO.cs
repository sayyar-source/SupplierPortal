namespace SupplierPortal.Application.DTOs;

public class CreatePurchaseRequestItemDTO
{
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
}