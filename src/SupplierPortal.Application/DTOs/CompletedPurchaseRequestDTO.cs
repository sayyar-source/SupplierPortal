namespace SupplierPortal.Application.DTOs;

public class CompletedPurchaseRequestDTO
{
    public int Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public string SupplierCode { get; set; } = string.Empty;
    public string SupplierTitle { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
    public List<PurchaseRequestItemDTO> Items { get; set; } = new();
}