using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedCustomerProfileWithMeasurementValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerProductProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DealerAccountId = table.Column<long>(nullable: false),
                    StoreCustomerId = table.Column<long>(nullable: false),
                    ProductCategoryId = table.Column<long>(nullable: false),
                    MeasurementId = table.Column<long>(nullable: true),
                    FittingTypeId = table.Column<long>(nullable: true),
                    MeasurementSizeId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProductProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProductProfiles_DealerAccount_DealerAccountId",
                        column: x => x.DealerAccountId,
                        principalTable: "DealerAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProductProfiles_FittingTypes_FittingTypeId",
                        column: x => x.FittingTypeId,
                        principalTable: "FittingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerProductProfiles_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerProductProfiles_MeasurementSizes_MeasurementSizeId",
                        column: x => x.MeasurementSizeId,
                        principalTable: "MeasurementSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerProductProfiles_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProductProfiles_StoreCustomers_StoreCustomerId",
                        column: x => x.StoreCustomerId,
                        principalTable: "StoreCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProfileSizeValues",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Value = table.Column<float>(nullable: true),
                    FittingValue = table.Column<float>(nullable: true),
                    MeasurementDefinitionId = table.Column<long>(nullable: false),
                    CustomerProductProfileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProfileSizeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProfileSizeValues_CustomerProductProfiles_CustomerProductProfileId",
                        column: x => x.CustomerProductProfileId,
                        principalTable: "CustomerProductProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProfileSizeValues_MeasurementDefinitions_MeasurementDefinitionId",
                        column: x => x.MeasurementDefinitionId,
                        principalTable: "MeasurementDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductProfiles_DealerAccountId",
                table: "CustomerProductProfiles",
                column: "DealerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductProfiles_FittingTypeId",
                table: "CustomerProductProfiles",
                column: "FittingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductProfiles_MeasurementId",
                table: "CustomerProductProfiles",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductProfiles_MeasurementSizeId",
                table: "CustomerProductProfiles",
                column: "MeasurementSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductProfiles_ProductCategoryId",
                table: "CustomerProductProfiles",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductProfiles_StoreCustomerId",
                table: "CustomerProductProfiles",
                column: "StoreCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfileSizeValues_CustomerProductProfileId",
                table: "CustomerProfileSizeValues",
                column: "CustomerProductProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfileSizeValues_MeasurementDefinitionId",
                table: "CustomerProfileSizeValues",
                column: "MeasurementDefinitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProfileSizeValues");

            migrationBuilder.DropTable(
                name: "CustomerProductProfiles");
        }
    }
}
