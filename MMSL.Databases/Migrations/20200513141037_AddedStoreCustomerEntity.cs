using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedStoreCustomerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreCustomers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    UserName = table.Column<string>(nullable: false),
                    CustomerName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    UseBillingAsDeliveryAddress = table.Column<bool>(nullable: false),
                    BillingAddressId = table.Column<long>(nullable: true),
                    DeliveryAddressId = table.Column<long>(nullable: true),
                    StoreId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreCustomers_Address_BillingAddressId",
                        column: x => x.BillingAddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreCustomers_Address_DeliveryAddressId",
                        column: x => x.DeliveryAddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreCustomers_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreCustomers_BillingAddressId",
                table: "StoreCustomers",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCustomers_DeliveryAddressId",
                table: "StoreCustomers",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCustomers_StoreId",
                table: "StoreCustomers",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreCustomers");
        }
    }
}
