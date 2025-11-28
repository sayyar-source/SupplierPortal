using SupplierPortal.Application.DTOs;

namespace SupplierPortal.Application.Services;

public interface IPurchaseRequestService
{
    Task<PurchaseRequestDTO> CreatePurchaseRequestAsync(CreatePurchaseRequestDTO createDto);
    Task<PurchaseRequestDTO?> GetPurchaseRequestByIdAsync(int id);
    Task<IEnumerable<PurchaseRequestDTO>> GetPurchaseRequestsBySupplierAsync(int supplierId);
    Task<IEnumerable<CompletedPurchaseRequestDTO>> GetCompletedPurchaseRequestsAsync();
    Task UpdatePurchaseRequestItemAsync(int purchaseRequestId, UpdatePurchaseRequestItemDTO updateDto);
    Task CompletePurchaseRequestAsync(int purchaseRequestId);
    Task<IEnumerable<PurchaseRequestDTO>> GetPendingPurchaseRequestsAsync();
}