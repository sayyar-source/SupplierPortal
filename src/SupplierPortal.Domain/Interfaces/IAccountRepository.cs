using SupplierPortal.Domain.Entities;

namespace SupplierPortal.Domain.Interfaces;

public interface IAccountRepository : IRepository<Account>
{
    Task<Account?> GetByUsernameAsync(string username);
    Task<Account?> GetByEmailAsync(string email);
}
