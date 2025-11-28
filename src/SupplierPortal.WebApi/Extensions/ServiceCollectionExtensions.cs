using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SupplierPortal.Application.DTOs;
using SupplierPortal.Application.Mappings;
using SupplierPortal.Application.Services;
using SupplierPortal.Application.Validations;
using SupplierPortal.Domain.Interfaces;
using SupplierPortal.Infrastructure.Options;
using SupplierPortal.Infrastructure.Repositories;
using SupplierPortal.Infrastructure.Services;
using System.Text;

namespace SupplierPortal.WebApi.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        // Application Services Layer
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IPurchaseRequestService, PurchaseRequestService>();
        services.AddScoped<IPurchaseRequestItemService, PurchaseRequestItemService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        // Infrastructure Services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();
        // Email service uses IOptions<EmailSettings>
        services.AddScoped<IEmailService, SmtpEmailService>();

        // Repositories
        services.AddScoped<IPurchaseRequestRepository, PurchaseRequestRepository>();
        services.AddScoped<IPurchaseRequestItemRepository, PurchaseRequestItemRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ISupplierProfileRepository, SupplierProfileRepository>();

        // Register user services & repository (add near other services)
        services.AddScoped<IUserService, UserService>();
      
        // AutoMapper - Object-to-object mapping
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

        // FluentValidation - Data validation
        services.AddValidatorsFromAssemblyContaining<CreateSupplierValidator>();
        services.AddScoped<IValidator<CreateSupplierDTO>, CreateSupplierValidator>();
        services.AddScoped<IValidator<CreatePurchaseRequestDTO>, CreatePurchaseRequestValidator>();
        services.AddScoped<IValidator<CreatePurchaseRequestItemDTO>, CreatePurchaseRequestItemValidator>();
        services.AddScoped<IValidator<UpdatePurchaseRequestItemDTO>, UpdatePurchaseRequestItemValidator>();



        return services;
    }
}