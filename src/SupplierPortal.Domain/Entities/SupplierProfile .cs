using SupplierPortal.Domain.Entities.Base;

namespace SupplierPortal.Domain.Entities;
public class SupplierProfile : BaseEntity
{
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public static void ValidateCreation(string code, string title, string username, string email)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Supplier code is required", nameof(code));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Supplier title is required", nameof(title));
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required", nameof(username));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
    }
}
