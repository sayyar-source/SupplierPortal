using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SupplierPortal.Application.DTOs;
using SupplierPortal.Domain.Entities;
using SupplierPortal.Domain.Interfaces;

namespace SupplierPortal.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ISupplierProfileRepository _profileRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<SupplierService> _logger;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _config;

    public SupplierService(
        IAccountRepository accountRepository,
        ISupplierProfileRepository profileRepository,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        ILogger<SupplierService> logger,
        IEmailService emailService,
        IConfiguration config)
    {
        _accountRepository = accountRepository;
        _profileRepository = profileRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _logger = logger;
        _emailService = emailService;
        _config = config;
    }

    public async Task<SupplierResultDTO> CreateSupplierAsync(CreateSupplierDTO createSupplierDto)
    {
        try
        {
            SupplierProfile.ValidateCreation(createSupplierDto.Code, createSupplierDto.Title, createSupplierDto.Username, createSupplierDto.Email);

            var existingProfile = await _profileRepository.GetByCodeAsync(createSupplierDto.Code);
            if (existingProfile != null) throw new InvalidOperationException($"Supplier with code '{createSupplierDto.Code}' already exists");

            var existingAccount = await _accountRepository.GetByUsernameAsync(createSupplierDto.Username);
            if (existingAccount != null) throw new InvalidOperationException($"Username '{createSupplierDto.Username}' is already taken");

            var account = new Account
            {
                Username = createSupplierDto.Username.Trim(),
                Email = createSupplierDto.Email.Trim(),
                PasswordHash = _passwordHasher.HashPassword(createSupplierDto.Password),
                FullName = createSupplierDto.Title.Trim(),
                IsActive = true,
                Role = Domain.Enums.AccountRole.Supplier,
                SupplierProfile=new SupplierProfile
                {
                    Code = createSupplierDto.Code.Trim(),
                    Title = createSupplierDto.Title.Trim(),
                    Phone = createSupplierDto.Phone.Trim(),
                    Address = createSupplierDto.Address.Trim()
                }
            };
            await _profileRepository.AddAsync(account);

            _logger.LogInformation("Supplier profile and account created: {Code} / {Username}", account.SupplierProfile.Code, account.Username);

            await SendSupplierNotificationEmailAsync(createSupplierDto);

            return _mapper.Map<SupplierResultDTO>(account);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating supplier");
            throw;
        }
    }

    public async Task<SupplierResultDTO?> GetSupplierByIdAsync(int id)
    {
        var account = await _profileRepository.GetByIdAsync(id);
        return account == null ? null : _mapper.Map<SupplierResultDTO>(account);
    }

    public async Task<SupplierResultDTO?> GetSupplierByUsernameAsync(string username)
    {
       var account = await _profileRepository.GetByUsernameAsync(username);
       return account == null? null: _mapper.Map<SupplierResultDTO>(account);
    }

    public async Task<IEnumerable<SupplierResultDTO>> GetAllSuppliersAsync()
    {
        var account = await _profileRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SupplierResultDTO>>(account);
    }

    public async Task UpdateSupplierAsync(int id, SupplierDTO dto)
    {
        var account = await _profileRepository.GetByIdAsync(id);

        if (account == null)
            throw new InvalidOperationException($"Supplier with ID {id} not found");

        account.SupplierProfile?.Title= dto.Title?.Trim() ?? account.SupplierProfile.Title;
        account.SupplierProfile?.Phone= dto.Phone?.Trim() ?? account.SupplierProfile.Phone;
        account.SupplierProfile?.Address = dto.Address?.Trim() ?? account.SupplierProfile.Address;
        account.IsActive = dto.IsActive;
        account.UpdatedAt = DateTime.UtcNow;

        await _profileRepository.UpdateAsync(account);

        _logger.LogInformation("Supplier updated successfully: {Code}", account.SupplierProfile?.Code);
    }

    public async Task DeleteSupplierAsync(int id)
    {
        var profile = await _profileRepository.GetByIdAsync(id);
        if (profile == null) throw new InvalidOperationException($"Supplier with ID {id} not found");

        profile.IsDeleted = true;
        profile.UpdatedAt = DateTime.UtcNow;
        await _profileRepository.UpdateAsync(profile);

        // optionally deactivate linked account
        var account = await _accountRepository.GetByIdAsync(profile.Id);
        if (account != null)
        {
            account.IsActive = false;
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(account);
        }

        _logger.LogInformation("Supplier profile soft-deleted: {Code}", profile.SupplierProfile?.Code);
    }

    public async Task<bool> VerifySupplierPasswordAsync(string username, string password)
    {
        var account = await _profileRepository.GetByUsernameAsync(username);
        if (account == null || !account.IsActive) return false;
        return _passwordHasher.VerifyPassword(password, account.PasswordHash);
    }

    public async Task<SupplierResultDTO?> GetByCodeAsync(string code)
    {
        var account = await _profileRepository.GetByCodeAsync(code);
        return account == null ? null : _mapper.Map<SupplierResultDTO>(account);
    }

    public async Task<SupplierResultDTO?> GetByTitleAsync(string title)
    {
        var account = await _profileRepository.GetByTitleAsync(title);
        return account == null ? null : _mapper.Map<SupplierResultDTO>(account);
    }

    public async Task<IEnumerable<SupplierResultDTO>> GetActiveSuppliersAsync()
    {
        var account = await _profileRepository.GetActiveSuppliersAsync();
        return _mapper.Map<IEnumerable<SupplierResultDTO>>(account);
    }

    public async Task<SupplierResultDTO?> GetByEmailAsync(string email)
    {
        var account = await _profileRepository.GetByEmailAsync(email);
        return account == null ? null : _mapper.Map<SupplierResultDTO>(account);
    }

    private async Task SendSupplierNotificationEmailAsync(CreateSupplierDTO dto)
    {
        try
        {
            var subject = _config["EmailSettings:DefaultSubject"];
            var bodyTemplate = _config["EmailSettings:DefaultBody"];

            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(bodyTemplate))
                throw new InvalidOperationException("Email subject or body template is missing in configuration.");

            string body = string.Format(bodyTemplate, dto.Title, dto.Username, dto.Password);

            await _emailService.SendEmailAsync(dto.Email, subject, body);

            _logger.LogInformation("Notification email sent to {Email}", dto.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send notification email to {Email}", dto.Email);
            throw; // rethrow to preserve stack trace
        }
    }

}