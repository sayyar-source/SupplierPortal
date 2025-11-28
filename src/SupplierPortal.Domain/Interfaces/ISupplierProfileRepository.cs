using SupplierPortal.Domain.Entities;

namespace SupplierPortal.Domain.Interfaces;

public interface ISupplierProfileRepository : IRepository<Account>
{
    Task<Account?> GetByCodeAsync(string code);
    Task<Account?> GetByAccountIdAsync(int accountId);
    Task<Account?> GetByTitleAsync(string title);
    Task<IEnumerable<Account>> GetActiveSuppliersAsync();
    Task<Account?> GetByEmailAsync(string email);
    Task<Account?> GetByUsernameAsync(string username);
}
