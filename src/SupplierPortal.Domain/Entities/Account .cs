using SupplierPortal.Domain.Entities.Base;
using SupplierPortal.Domain.Enums;

namespace SupplierPortal.Domain.Entities;

public class Account : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public bool IsActive { get; set; } = true;
    public AccountRole Role { get; set; }

    public SupplierProfile? SupplierProfile { get; set; }
    public ICollection<PurchaseRequest>? PurchaseRequests { get; set; }
}