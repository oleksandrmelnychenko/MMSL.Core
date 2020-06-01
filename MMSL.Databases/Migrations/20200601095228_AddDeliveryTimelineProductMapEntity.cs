using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddDeliveryTimelineProductMapEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "DeliveryTimelines",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DeliveryTimelineProductMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    DeliveryTimelineId = table.Column<long>(nullable: false),
                    ProductCategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTimelineProductMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryTimelineProductMaps_DeliveryTimelines_DeliveryTimelineId",
                        column: x => x.DeliveryTimelineId,
                        principalTable: "DeliveryTimelines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryTimelineProductMaps_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTimelineProductMaps_DeliveryTimelineId",
                table: "DeliveryTimelineProductMaps",
                column: "DeliveryTimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTimelineProductMaps_ProductCategoryId",
                table: "DeliveryTimelineProductMaps",
                column: "ProductCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryTimelineProductMaps");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "DeliveryTimelines");
        }
    }
}
