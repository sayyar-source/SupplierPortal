using SupplierPortal.Application.DTOs;

namespace SupplierPortal.Application.Services;

public interface IPurchaseRequestItemService
{

    Task<PurchaseRequestItemDTO?> GetItemByIdAsync(int itemId);

    Task<IEnumerable<PurchaseRequestItemDTO>> GetItemsByPurchaseRequestAsync(int purchaseRequestId);

    Task<IEnumerable<PurchaseRequestItemDTO>> GetUnpricedItemsAsync(int purchaseRequestId);

    Task<PurchaseRequestItemDTO> UpdateItemPriceAsync(int itemId, UpdatePurchaseRequestItemDTO updateDto);

    Task<IEnumerable<PurchaseRequestItemDTO>> BulkUpdateItemsAsync(
        int purchaseRequestId, 
        IEnumerable<UpdatePurchaseRequestItemDTO> updates);

    Task<decimal> GetTotalPriceAsync(int purchaseRequestId);

    Task<bool> AreAllItemsPricedAsync(int purchaseRequestId);

    Task<int> GetUnpricedItemsCountAsync(int purchaseRequestId);

    Task ResetItemPricingAsync(int itemId);

    Task DeleteItemAsync(int itemId);
}