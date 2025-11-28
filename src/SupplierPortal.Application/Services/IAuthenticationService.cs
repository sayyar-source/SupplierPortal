using SupplierPortal.Application.DTOs;

namespace SupplierPortal.Application.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponseDto> LoginAsync(LoginRequestDto loginRequest);
    Task<bool> ValidateTokenAsync(string token);
    Task<AccountDTO> GetCurrentAccount(string token);
}