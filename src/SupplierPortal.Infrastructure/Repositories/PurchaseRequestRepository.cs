using Microsoft.EntityFrameworkCore;
using SupplierPortal.Domain.Entities;
using SupplierPortal.Domain.Enums;
using SupplierPortal.Domain.Interfaces;
using SupplierPortal.Infrastructure.Data;

namespace SupplierPortal.Infrastructure.Repositories;

public class PurchaseRequestRepository : IPurchaseRequestRepository
{
    private readonly SupplierPortalDbContext _context;

    public PurchaseRequestRepository(SupplierPortalDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseRequest?> GetByIdAsync(int id)
    {
        return await _context.PurchaseRequests
            .Include(pr => pr.Account)
            .Include(pr => pr.Items)
            .Where(pr => !pr.IsDeleted)
            .FirstOrDefaultAsync(pr => pr.Id == id);
    }

    public async Task<IEnumerable<PurchaseRequest>> GetAllAsync()
    {
        return await _context.PurchaseRequests
            .Include(pr => pr.Account)
            .Include(pr => pr.Items)
            .Where(pr => !pr.IsDeleted)
            .ToListAsync();
    }

    public async Task<PurchaseRequest> AddAsync(PurchaseRequest entity)
    {
        _context.PurchaseRequests.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(PurchaseRequest entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.PurchaseRequests.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var purchaseRequest = await GetByIdAsync(id);
        if (purchaseRequest != null)
        {
            purchaseRequest.IsDeleted = true;
            await UpdateAsync(purchaseRequest);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.PurchaseRequests.AnyAsync(pr => pr.Id == id && !pr.IsDeleted);
    }

    public async Task<IEnumerable<PurchaseRequest>> GetBySupplierIdAsync(int supplierId)
    {
        return await _context.PurchaseRequests
            .Include(pr => pr.Account)
            .Include(pr => pr.Items)
            .Where(pr => pr.SupplierId == supplierId && !pr.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<PurchaseRequest>> GetCompletedRequestsAsync()
    {
        return await _context.PurchaseRequests
            .Include(pr => pr.Account)
            .Include(pr => pr.Items)
            .Where(pr => pr.Status == PurchaseRequestStatus.Completed && !pr.IsDeleted)
            .OrderByDescending(pr => pr.CompletedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<PurchaseRequest>> GetByStatusAsync(PurchaseRequestStatus status)
    {
        return await _context.PurchaseRequests
            .Include(pr => pr.Account)
            .Include(pr => pr.Items)
            .Where(pr => pr.Status == status && !pr.IsDeleted)
            .ToListAsync();
    }

    public async Task<PurchaseRequest?> GetByRequestNumberAsync(string requestNumber)
    {
        return await _context.PurchaseRequests
            .Include(pr => pr.Account)
            .Include(pr => pr.Items)
            .Where(pr => !pr.IsDeleted)
            .FirstOrDefaultAsync(pr => pr.RequestNumber == requestNumber);
    }

    public async Task<string> GenerateRequestNumberAsync()
    {
        var lastNumber = await _context.PurchaseRequests
            .OrderByDescending(p => p.Id)
            .Select(p => p.RequestNumber)
            .FirstOrDefaultAsync();

        int next = 1;

        if (!string.IsNullOrEmpty(lastNumber))
        {
            var parts = lastNumber.Split('-');   // PR-2025-000123
            int.TryParse(parts.Last(), out next);
            next++;
        }

        string result = $"PR-{DateTime.UtcNow.Year}-{next.ToString("D6")}";
        return result;
    }
}