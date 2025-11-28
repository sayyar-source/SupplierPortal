using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SupplierPortal.Domain.Entities;
using SupplierPortal.Domain.Enums;

namespace SupplierPortal.Infrastructure.Data.Extensions;

public static class SeedDataExtension
{
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SupplierPortalDbContext>();

            try
            {
                // Seed initial data if database is empty
                await SeedDatabaseAsync(dbContext);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during database initialization");
                throw;
            }
        }
    }

    private static async Task SeedDatabaseAsync(SupplierPortalDbContext dbContext)
    {
        try
        {
            Log.Information("Checking existing data for seeding...");

            var hasher = new PasswordHasher<object>();

            // -------------------------
            // Seed Accounts (Admin, User, Supplier)
            // -------------------------
            if (!await dbContext.Accounts.AnyAsync())
            {
                Log.Information("Seeding default accounts (Admin, User, Supplier)...");

                var admin = new Account
                {
                    Username = "admin",
                    Email = "admin@supplierportal.local",
                    PasswordHash = hasher.HashPassword(new object(), "Admin@123"),
                    FullName = "Portal Administrator",
                    IsActive = true,
                    Role = AccountRole.Admin,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                var user = new Account
                {
                    Username = "user1",
                    Email = "user1@supplierportal.local",
                    PasswordHash = hasher.HashPassword(new object(), "User@123"),
                    FullName = "Demo User",
                    IsActive = true,
                    Role = AccountRole.User,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                var supplierAccount1 = new Account
                {
                    Username = "supplier1",
                    Email = "contact@abctrading.com",
                    PasswordHash = hasher.HashPassword(new object(), "abc@123"),
                    FullName = "ABC Trading Company",
                    IsActive = true,
                    Role = AccountRole.Supplier,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                var supplierAccount2 = new Account
                {
                    Username = "supplier2",
                    Email = "contact2@xyzmanufacturing.com",
                    PasswordHash = hasher.HashPassword(new object(), "abc@123"),
                    FullName = "XYZ Manufacturing Ltd",
                    IsActive = true,
                    Role = AccountRole.Supplier,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await dbContext.Accounts.AddRangeAsync(admin, user, supplierAccount1, supplierAccount2);
                await dbContext.SaveChangesAsync();

                Log.Information("Added {Count} accounts", 4);

                // Create supplier profiles linked to the supplier accounts
                var profile1 = new SupplierProfile
                {
                    AccountId = supplierAccount1.Id,
                    Code = "SUPP001",
                    Title = "ABC Trading Company",
                    Phone = "+1234567890",
                    Address = "123 Main Street, City, Country",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                var profile2 = new SupplierProfile
                {
                    AccountId = supplierAccount2.Id,
                    Code = "SUPP002",
                    Title = "XYZ Manufacturing Ltd",
                    Phone = "+9876543210",
                    Address = "456 Industrial Avenue, Factory Zone, Country",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                // Only add profiles if they don't already exist
                if (!await dbContext.SupplierProfiles.AnyAsync(sp => sp.AccountId == supplierAccount1.Id || sp.Code == profile1.Code))
                    await dbContext.SupplierProfiles.AddAsync(profile1);

                if (!await dbContext.SupplierProfiles.AnyAsync(sp => sp.AccountId == supplierAccount2.Id || sp.Code == profile2.Code))
                    await dbContext.SupplierProfiles.AddAsync(profile2);

                await dbContext.SaveChangesAsync();
                Log.Information("Added supplier profiles for seeded supplier accounts.");
            }
            else
            {
                Log.Information("Accounts exist; skipping account seeding.");
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while seeding database");
            throw;
        }
    }
}