using AutoMapper;
using Microsoft.Extensions.Logging;
using SupplierPortal.Application.DTOs;
using SupplierPortal.Domain.Entities;
using SupplierPortal.Domain.Interfaces;

namespace SupplierPortal.Application.Services;

public class PurchaseRequestItemService : IPurchaseRequestItemService
{
    private readonly IPurchaseRequestItemRepository _purchaseRequestItemRepository;
    private readonly IPurchaseRequestRepository _purchaseRequestRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PurchaseRequestItemService> _logger;

    public PurchaseRequestItemService(
        IPurchaseRequestItemRepository purchaseRequestItemRepository,
        IPurchaseRequestRepository purchaseRequestRepository,
        IMapper mapper,
        ILogger<PurchaseRequestItemService> logger)
    {
        _purchaseRequestItemRepository = purchaseRequestItemRepository;
        _purchaseRequestRepository = purchaseRequestRepository;
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<PurchaseRequestItemDTO?> GetItemByIdAsync(int itemId)
    {
        try
        {
            var item = await _purchaseRequestItemRepository.GetByIdAsync(itemId);
            return item == null ? null : _mapper.Map<PurchaseRequestItemDTO>(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving purchase request item {ItemId}", itemId);
            throw;
        }
    }

    public async Task<IEnumerable<PurchaseRequestItemDTO>> GetItemsByPurchaseRequestAsync(int purchaseRequestId)
    {
        try
        {
            var items = await _purchaseRequestItemRepository.GetByPurchaseRequestIdAsync(purchaseRequestId);
            return _mapper.Map<IEnumerable<PurchaseRequestItemDTO>>(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving items for purchase request {PurchaseRequestId}", purchaseRequestId);
            throw;
        }
    }

    public async Task<IEnumerable<PurchaseRequestItemDTO>> GetUnpricedItemsAsync(int purchaseRequestId)
    {
        try
        {
            var items = await _purchaseRequestItemRepository.GetUnpricedItemsByPurchaseRequestIdAsync(purchaseRequestId);
            return _mapper.Map<IEnumerable<PurchaseRequestItemDTO>>(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unpriced items for purchase request {PurchaseRequestId}", purchaseRequestId);
            throw;
        }
    }

    public async Task<PurchaseRequestItemDTO> UpdateItemPriceAsync(int itemId, UpdatePurchaseRequestItemDTO updateDto)
    {
        try
        {
            var item = await _purchaseRequestItemRepository.GetByIdAsync(itemId);
            if (item == null)
                throw new InvalidOperationException($"Purchase request item with ID {itemId} not found");

            // Validate and update using domain method
            item.UpdatePrice(updateDto.Price, updateDto.DeliveryDate);

            await _purchaseRequestItemRepository.UpdateAsync(item);
            _logger.LogInformation("Item {ItemId} price updated: Price={Price}, DeliveryDate={DeliveryDate}",
                itemId, updateDto.Price, updateDto.DeliveryDate);

            return _mapper.Map<PurchaseRequestItemDTO>(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating price for item {ItemId}", itemId);
            throw;
        }
    }

    public async Task<IEnumerable<PurchaseRequestItemDTO>> BulkUpdateItemsAsync(
        int purchaseRequestId,
        IEnumerable<UpdatePurchaseRequestItemDTO> updates)
    {
        try
        {
            if (updates == null || !updates.Any())
                throw new ArgumentException("Updates collection cannot be null or empty", nameof(updates));

            var purchaseRequest = await _purchaseRequestRepository.GetByIdAsync(purchaseRequestId);
            if (purchaseRequest == null)
                throw new InvalidOperationException($"Purchase request with ID {purchaseRequestId} not found");

            var updatedItems = new List<PurchaseRequestItem>();

            foreach (var update in updates)
            {
                var item = await _purchaseRequestItemRepository.GetByIdAsync(update.ItemId);
                if (item == null)
                {
                    _logger.LogWarning("Item {ItemId} not found during bulk update", update.ItemId);
                    continue;
                }

                // Validate and update
                item.UpdatePrice(update.Price, update.DeliveryDate);
                updatedItems.Add(item);
            }

            if (updatedItems.Any())
            {
                await _purchaseRequestItemRepository.BulkUpdatePricesAsync(updatedItems);
                _logger.LogInformation("Bulk updated {Count} items for purchase request {PurchaseRequestId}",
                    updatedItems.Count, purchaseRequestId);
            }

            return _mapper.Map<IEnumerable<PurchaseRequestItemDTO>>(updatedItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk updating items for purchase request {PurchaseRequestId}", purchaseRequestId);
            throw;
        }
    }

    public async Task<decimal> GetTotalPriceAsync(int purchaseRequestId)
    {
        try
        {
            return await _purchaseRequestItemRepository.GetTotalPriceAsync(purchaseRequestId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating total price for purchase request {PurchaseRequestId}", purchaseRequestId);
            throw;
        }
    }

    public async Task<bool> AreAllItemsPricedAsync(int purchaseRequestId)
    {
        try
        {
            return await _purchaseRequestItemRepository.AreAllItemsPricedAsync(purchaseRequestId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if all items are priced for purchase request {PurchaseRequestId}", purchaseRequestId);
            throw;
        }
    }

    public async Task<int> GetUnpricedItemsCountAsync(int purchaseRequestId)
    {
        try
        {
            return await _purchaseRequestItemRepository.GetUnpricedItemsCountAsync(purchaseRequestId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unpriced items count for purchase request {PurchaseRequestId}", purchaseRequestId);
            throw;
        }
    }

    public async Task ResetItemPricingAsync(int itemId)
    {
        try
        {
            var item = await _purchaseRequestItemRepository.GetByIdAsync(itemId);
            if (item == null)
                throw new InvalidOperationException($"Purchase request item with ID {itemId} not found");

            item.Price = null;
            item.DeliveryDate = null;
            item.IsPriced = false;

            await _purchaseRequestItemRepository.UpdateAsync(item);
            _logger.LogInformation("Item {ItemId} pricing reset", itemId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting pricing for item {ItemId}", itemId);
            throw;
        }
    }

    public async Task DeleteItemAsync(int itemId)
    {
        try
        {
            var item = await _purchaseRequestItemRepository.GetByIdAsync(itemId);
            if (item == null)
                throw new InvalidOperationException($"Purchase request item with ID {itemId} not found");

            await _purchaseRequestItemRepository.DeleteAsync(itemId);
            _logger.LogInformation("Item {ItemId} deleted", itemId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting item {ItemId}", itemId);
            throw;
        }
    }
}