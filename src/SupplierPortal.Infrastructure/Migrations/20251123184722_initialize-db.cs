using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupplierPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initializedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestNumber = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 1000, nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Accounts_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierProfiles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequestItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseRequestId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: false),
                    Unit = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "DECIMAL(18,4)", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    IsPriced = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequestItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestItems_PurchaseRequests_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Accounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_IsActive",
                table: "Accounts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Account_IsDeleted",
                table: "Accounts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Username",
                table: "Accounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestItem_IsDeleted",
                table: "PurchaseRequestItems",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestItem_IsPriced",
                table: "PurchaseRequestItems",
                column: "IsPriced");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestItem_PurchaseRequestId",
                table: "PurchaseRequestItems",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequest_CompletedAt",
                table: "PurchaseRequests",
                column: "CompletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequest_IsDeleted",
                table: "PurchaseRequests",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequest_RequestDate",
                table: "PurchaseRequests",
                column: "RequestDate");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequest_RequestNumber",
                table: "PurchaseRequests",
                column: "RequestNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequest_Status",
                table: "PurchaseRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequest_SupplierId",
                table: "PurchaseRequests",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Code",
                table: "SupplierProfiles",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_IsDeleted",
                table: "SupplierProfiles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProfiles_AccountId",
                table: "SupplierProfiles",
                column: "AccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseRequestItems");

            migrationBuilder.DropTable(
                name: "SupplierProfiles");

            migrationBuilder.DropTable(
                name: "PurchaseRequests");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
