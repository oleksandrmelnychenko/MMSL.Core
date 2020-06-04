using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedProductPermissionSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductPermissionSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProductCategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPermissionSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPermissionSettings_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    IsAllow = table.Column<bool>(nullable: false),
                    ProductPermissionSettingsId = table.Column<long>(nullable: false),
                    OptionGroupId = table.Column<long>(nullable: false),
                    OptionUnitId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionSettings_OptionGroups_OptionGroupId",
                        column: x => x.OptionGroupId,
                        principalTable: "OptionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionSettings_OptionUnits_OptionUnitId",
                        column: x => x.OptionUnitId,
                        principalTable: "OptionUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionSettings_ProductPermissionSettings_ProductPermissionSettingsId",
                        column: x => x.ProductPermissionSettingsId,
                        principalTable: "ProductPermissionSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionSettings_OptionGroupId",
                table: "PermissionSettings",
                column: "OptionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionSettings_OptionUnitId",
                table: "PermissionSettings",
                column: "OptionUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionSettings_ProductPermissionSettingsId",
                table: "PermissionSettings",
                column: "ProductPermissionSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPermissionSettings_ProductCategoryId",
                table: "ProductPermissionSettings",
                column: "ProductCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionSettings");

            migrationBuilder.DropTable(
                name: "ProductPermissionSettings");
        }
    }
}
