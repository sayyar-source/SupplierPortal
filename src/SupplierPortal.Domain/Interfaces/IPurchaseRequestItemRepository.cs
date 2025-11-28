using SupplierPortal.Domain.Entities;

namespace SupplierPortal.Domain.Interfaces;
public interface IPurchaseRequestItemRepository : IRepository<PurchaseRequestItem>
{
    Task<IEnumerable<PurchaseRequestItem>> GetByPurchaseRequestIdAsync(int purchaseRequestId);

    Task<IEnumerable<PurchaseRequestItem>> GetUnpricedItemsByPurchaseRequestIdAsync(int purchaseRequestId);

    Task<IEnumerable<PurchaseRequestItem>> GetPricedItemsByPurchaseRequestIdAsync(int purchaseRequestId);

    Task<bool> AreAllItemsPricedAsync(int purchaseRequestId);

    Task<int> GetUnpricedItemsCountAsync(int purchaseRequestId);

    Task<decimal> GetTotalPriceAsync(int purchaseRequestId);

    Task BulkUpdatePricesAsync(IEnumerable<PurchaseRequestItem> items);

    Task DeleteByPurchaseRequestIdAsync(int purchaseRequestId);
}