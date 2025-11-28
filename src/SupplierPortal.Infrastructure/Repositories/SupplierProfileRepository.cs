using Microsoft.EntityFrameworkCore;
using SupplierPortal.Domain.Entities;
using SupplierPortal.Domain.Interfaces;
using SupplierPortal.Infrastructure.Data;

namespace SupplierPortal.Infrastructure.Repositories;

public class SupplierProfileRepository : ISupplierProfileRepository
{
    private readonly SupplierPortalDbContext _context;

    public SupplierProfileRepository(SupplierPortalDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        return await _context.Accounts
            .Where(a => a.Role == Domain.Enums.AccountRole.Supplier)
            .Include(a => a.SupplierProfile)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _context.Accounts
            .Where(a=>a.Role == Domain.Enums.AccountRole.Supplier)
            .Include(a => a.SupplierProfile)
            .ToListAsync();
    }

    public async Task<Account> AddAsync(Account entity)
    {
        _context.Accounts.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Account entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Accounts.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var supplier = await GetByIdAsync(id);
        if (supplier != null)
        {
            supplier.IsDeleted = true;
            await UpdateAsync(supplier);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.SupplierProfiles.AnyAsync(s => s.Id == id && !s.IsDeleted);
    }

    public async Task<Account?> GetByCodeAsync(string code)
    {
        var account = await _context.Accounts
            .Where(a => a.Role == Domain.Enums.AccountRole.Supplier)
            .Include(a => a.SupplierProfile)
            .FirstOrDefaultAsync(s => s.SupplierProfile != null && s.SupplierProfile.Code == code);
        return account!;
    }

    public async Task<Account?> GetByUsernameAsync(string username)
    {
        return await _context.Accounts
            .Where(a => a.Role == Domain.Enums.AccountRole.Supplier)
            .Include(a => a.SupplierProfile)
            .FirstOrDefaultAsync(s => s.Username == username);
    }

    public async Task<Account?> GetByEmailAsync(string email)
    {
        return await _context.Accounts
            .Where(a => a.Role == Domain.Enums.AccountRole.Supplier)
            .Include(a => a.SupplierProfile)
            .FirstOrDefaultAsync(s => s.Email == email);
    }

    public async Task<IEnumerable<Account>> GetActiveSuppliersAsync()
    {
        return await _context.Accounts
            .Where(a => a.Role == Domain.Enums.AccountRole.Supplier)
            .Include(a => a.SupplierProfile)
            .Where(s => s.IsActive)
            .ToListAsync();
    }

    public async Task<Account?> GetByAccountIdAsync(int accountId)
    {
        return await _context.Accounts
            .Where(a => a.Role == Domain.Enums.AccountRole.Supplier)
            .Include(a => a.SupplierProfile)
            .FirstOrDefaultAsync(s => s.Id == accountId);
    }

    public async Task<Account?> GetByTitleAsync(string title)
    {
       return await _context.Accounts
            .Where(a => a.Role == Domain.Enums.AccountRole.Supplier)
            .Include(a => a.SupplierProfile)
            .FirstOrDefaultAsync(s => s.SupplierProfile != null && s.SupplierProfile.Title == title);
    }
}