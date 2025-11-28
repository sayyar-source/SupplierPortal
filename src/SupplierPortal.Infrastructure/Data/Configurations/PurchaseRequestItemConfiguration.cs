using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplierPortal.Domain.Entities;

namespace SupplierPortal.Infrastructure.Data.Configurations;

public class PurchaseRequestItemConfiguration : IEntityTypeConfiguration<PurchaseRequestItem>
{
    public void Configure(EntityTypeBuilder<PurchaseRequestItem> builder)
    {
        builder.ToTable("PurchaseRequestItems");

        builder.HasKey(pri => pri.Id);

        builder.Property(pri => pri.PurchaseRequestId)
            .IsRequired();

        builder.Property(pri => pri.ProductName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("NVARCHAR(200)");

        builder.Property(pri => pri.Quantity)
            .IsRequired()
            .HasColumnType("DECIMAL(18,4)");

        builder.Property(pri => pri.Unit)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnType("NVARCHAR(20)");

        builder.Property(pri => pri.Price)
            .IsRequired(false)
            .HasColumnType("DECIMAL(18,4)");

        builder.Property(pri => pri.DeliveryDate)
            .IsRequired(false)
            .HasColumnType("DATETIME2");

        builder.Property(pri => pri.IsPriced)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(pri => pri.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(pri => pri.UpdatedAt)
            .IsRequired(false);

        builder.Property(pri => pri.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(pri => pri.PurchaseRequestId)
            .HasDatabaseName("IX_PurchaseRequestItem_PurchaseRequestId");

        builder.HasIndex(pri => pri.IsPriced)
            .HasDatabaseName("IX_PurchaseRequestItem_IsPriced");

        builder.HasIndex(pri => pri.IsDeleted)
            .HasDatabaseName("IX_PurchaseRequestItem_IsDeleted");

        builder.HasOne(pri => pri.PurchaseRequest)
            .WithMany(pr => pr.Items)
            .HasForeignKey(pri => pri.PurchaseRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}