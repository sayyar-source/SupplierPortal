using Microsoft.EntityFrameworkCore;
using SupplierPortal.Domain.Entities;
using SupplierPortal.Domain.Interfaces;
using SupplierPortal.Infrastructure.Data;

namespace SupplierPortal.Infrastructure.Repositories;
public class PurchaseRequestItemRepository : IPurchaseRequestItemRepository
{
    private readonly SupplierPortalDbContext _context;

    public PurchaseRequestItemRepository(SupplierPortalDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<PurchaseRequestItem?> GetByIdAsync(int id)
    {
        return await _context.PurchaseRequestItems
            .Include(item => item.PurchaseRequest)
            .Where(item => !item.IsDeleted)
            .FirstOrDefaultAsync(item => item.Id == id);
    }
    public async Task<IEnumerable<PurchaseRequestItem>> GetAllAsync()
    {
        return await _context.PurchaseRequestItems
            .Include(item => item.PurchaseRequest)
            .Where(item => !item.IsDeleted)
            .ToListAsync();
    }
    public async Task<PurchaseRequestItem> AddAsync(PurchaseRequestItem entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.PurchaseRequestItems.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public async Task UpdateAsync(PurchaseRequestItem entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        entity.UpdatedAt = DateTime.UtcNow;
        _context.PurchaseRequestItems.Update(entity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var item = await GetByIdAsync(id);
        if (item != null)
        {
            item.IsDeleted = true;
            await UpdateAsync(item);
        }
    }
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.PurchaseRequestItems
            .AnyAsync(item => item.Id == id && !item.IsDeleted);
    }
    public async Task<IEnumerable<PurchaseRequestItem>> GetByPurchaseRequestIdAsync(int purchaseRequestId)
    {
        return await _context.PurchaseRequestItems
            .Where(item => item.PurchaseRequestId == purchaseRequestId && !item.IsDeleted)
            .OrderBy(item => item.Id)
            .ToListAsync();
    }
    public async Task<IEnumerable<PurchaseRequestItem>> GetUnpricedItemsByPurchaseRequestIdAsync(int purchaseRequestId)
    {
        return await _context.PurchaseRequestItems
            .Where(item => item.PurchaseRequestId == purchaseRequestId && !item.IsPriced && !item.IsDeleted)
            .OrderBy(item => item.Id)
            .ToListAsync();
    }
    public async Task<IEnumerable<PurchaseRequestItem>> GetPricedItemsByPurchaseRequestIdAsync(int purchaseRequestId)
    {
        return await _context.PurchaseRequestItems
            .Where(item => item.PurchaseRequestId == purchaseRequestId && item.IsPriced && !item.IsDeleted)
            .OrderBy(item => item.Id)
            .ToListAsync();
    }
    public async Task<bool> AreAllItemsPricedAsync(int purchaseRequestId)
    {
        var totalItems = await _context.PurchaseRequestItems
            .CountAsync(item => item.PurchaseRequestId == purchaseRequestId && !item.IsDeleted);

        var pricedItems = await _context.PurchaseRequestItems
            .CountAsync(item => item.PurchaseRequestId == purchaseRequestId && item.IsPriced && !item.IsDeleted);

        return totalItems > 0 && totalItems == pricedItems;
    }
    public async Task<int> GetUnpricedItemsCountAsync(int purchaseRequestId)
    {
        return await _context.PurchaseRequestItems
            .CountAsync(item => item.PurchaseRequestId == purchaseRequestId && !item.IsPriced && !item.IsDeleted);
    }
    public async Task<decimal> GetTotalPriceAsync(int purchaseRequestId)
    {
        return await _context.PurchaseRequestItems
            .Where(item => item.PurchaseRequestId == purchaseRequestId && item.IsPriced && !item.IsDeleted)
            .SumAsync(item => (item.Price ?? 0) * item.Quantity);
    }
    public async Task BulkUpdatePricesAsync(IEnumerable<PurchaseRequestItem> items)
    {
        if (items == null || !items.Any())
            throw new ArgumentException("Items collection cannot be null or empty", nameof(items));

        foreach (var item in items)
        {
            item.UpdatedAt = DateTime.UtcNow;
        }

        _context.PurchaseRequestItems.UpdateRange(items);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByPurchaseRequestIdAsync(int purchaseRequestId)
    {
        var items = await _context.PurchaseRequestItems
            .Where(item => item.PurchaseRequestId == purchaseRequestId && !item.IsDeleted)
            .ToListAsync();

        foreach (var item in items)
        {
            item.IsDeleted = true;
            item.UpdatedAt = DateTime.UtcNow;
        }

        if (items.Any())
        {
            _context.PurchaseRequestItems.UpdateRange(items);
            await _context.SaveChangesAsync();
        }
    }
}