using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplierPortal.Domain.Entities;

namespace SupplierPortal.Infrastructure.Data.Configurations;

public class PurchaseRequestConfiguration : IEntityTypeConfiguration<PurchaseRequest>
{
    public void Configure(EntityTypeBuilder<PurchaseRequest> builder)
    {
        builder.ToTable("PurchaseRequests");

        builder.HasKey(pr => pr.Id);

        builder.Property(pr => pr.RequestNumber)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("NVARCHAR(50)");

        builder.Property(pr => pr.SupplierId)
            .IsRequired();

        builder.Property(pr => pr.RequestDate)
            .IsRequired()
            .HasColumnType("DATETIME2");

        builder.Property(pr => pr.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(pr => pr.Notes)
            .IsRequired(false)
            .HasMaxLength(1000)
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(pr => pr.CompletedAt)
            .IsRequired(false)
            .HasColumnType("DATETIME2");

        builder.Property(pr => pr.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(pr => pr.UpdatedAt)
            .IsRequired(false);

        builder.Property(pr => pr.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(pr => pr.RequestNumber)
            .IsUnique()
            .HasDatabaseName("IX_PurchaseRequest_RequestNumber");

        builder.HasIndex(pr => pr.SupplierId)
            .HasDatabaseName("IX_PurchaseRequest_SupplierId");

        builder.HasIndex(pr => pr.Status)
            .HasDatabaseName("IX_PurchaseRequest_Status");

        builder.HasIndex(pr => pr.RequestDate)
            .HasDatabaseName("IX_PurchaseRequest_RequestDate");

        builder.HasIndex(pr => pr.CompletedAt)
            .HasDatabaseName("IX_PurchaseRequest_CompletedAt");

        builder.HasIndex(pr => pr.IsDeleted)
            .HasDatabaseName("IX_PurchaseRequest_IsDeleted");

        builder.HasOne(pr => pr.Account)
            .WithMany(s => s.PurchaseRequests)
            .HasForeignKey(pr => pr.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(pr => pr.Items)
            .WithOne(pri => pri.PurchaseRequest)
            .HasForeignKey(pri => pri.PurchaseRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}