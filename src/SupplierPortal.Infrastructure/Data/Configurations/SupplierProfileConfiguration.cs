using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplierPortal.Domain.Entities;

namespace SupplierPortal.Infrastructure.Data.Configurations;
public class SupplierProfileConfiguration : IEntityTypeConfiguration<SupplierProfile>
{
    public void Configure(EntityTypeBuilder<SupplierProfile> builder)
    {
        builder.ToTable("SupplierProfiles");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Code)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("NVARCHAR(50)");

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("NVARCHAR(200)");

        builder.Property(s => s.Phone)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnType("NVARCHAR(20)");

        builder.Property(s => s.Address)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnType("NVARCHAR(500)");

        builder.Property(s => s.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(s => s.UpdatedAt)
            .IsRequired(false);

        builder.Property(s => s.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(s => s.Code)
            .IsUnique()
            .HasDatabaseName("IX_Supplier_Code");

        builder.HasIndex(s => s.IsDeleted)
            .HasDatabaseName("IX_Supplier_IsDeleted");

    }
}