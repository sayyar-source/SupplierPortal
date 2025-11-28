using Microsoft.AspNetCore.Identity;
using SupplierPortal.Application.Services;

namespace SupplierPortal.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(new object(), password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        var result = _hasher.VerifyHashedPassword(new object(), hash, password);
        return result == PasswordVerificationResult.Success;
    }
}