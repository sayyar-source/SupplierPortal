using SupplierPortal.Domain.Enums;

namespace SupplierPortal.Application.DTOs;

public class SupplierResultDTO
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public bool IsActive { get; set; }
    public AccountRole Role { get; set; }
    public SupplierProfileDTO? SupplierProfileDTO { get; set; }
}
