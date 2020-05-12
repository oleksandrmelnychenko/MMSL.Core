using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedDealerAccountEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    AccountNo = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    SwiftBic = table.Column<string>(nullable: false),
                    Iban = table.Column<string>(nullable: false),
                    VatNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealerAccount",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    AlternateEmail = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    TaxNumber = table.Column<string>(nullable: true),
                    IsVatApplicable = table.Column<bool>(nullable: false),
                    Currency = table.Column<int>(nullable: false),
                    PaymentType = table.Column<int>(nullable: false),
                    IsCreditAllowed = table.Column<bool>(nullable: false),
                    BillingAddressId = table.Column<long>(nullable: true),
                    UseBillingAsShipping = table.Column<bool>(nullable: false),
                    ShippingAddressId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerAccount_Address_BillingAddressId",
                        column: x => x.BillingAddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealerAccount_Address_ShippingAddressId",
                        column: x => x.ShippingAddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealerAccount_BillingAddressId",
                table: "DealerAccount",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerAccount_ShippingAddressId",
                table: "DealerAccount",
                column: "ShippingAddressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankDetails");

            migrationBuilder.DropTable(
                name: "DealerAccount");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
