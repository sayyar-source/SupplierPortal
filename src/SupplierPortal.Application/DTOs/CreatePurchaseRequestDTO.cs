namespace SupplierPortal.Application.DTOs;

public class CreatePurchaseRequestDTO
{
    public int SupplierId { get; set; }
    public string? Notes { get; set; }
    public List<CreatePurchaseRequestItemDTO> Items { get; set; } = new();
}
