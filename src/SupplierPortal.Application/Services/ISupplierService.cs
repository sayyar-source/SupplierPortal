using SupplierPortal.Application.DTOs;
using SupplierPortal.Domain.Entities;

namespace SupplierPortal.Application.Services;

public interface ISupplierService
{
    Task<SupplierResultDTO> CreateSupplierAsync(CreateSupplierDTO createSupplierDto);
    Task<SupplierResultDTO?> GetSupplierByIdAsync(int id);
    Task<SupplierResultDTO?> GetSupplierByUsernameAsync(string username);
    Task<SupplierResultDTO?> GetByCodeAsync(string code);
    Task<SupplierResultDTO?> GetByTitleAsync(string title);
    Task<IEnumerable<SupplierResultDTO>> GetActiveSuppliersAsync();
    Task<SupplierResultDTO?> GetByEmailAsync(string email);
    Task<IEnumerable<SupplierResultDTO>> GetAllSuppliersAsync();
    Task UpdateSupplierAsync(int id, SupplierDTO supplierDto);
    Task DeleteSupplierAsync(int id);
    Task<bool> VerifySupplierPasswordAsync(string username, string password);
}