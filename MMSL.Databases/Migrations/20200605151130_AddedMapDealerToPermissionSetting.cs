using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedMapDealerToPermissionSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DealerMapProductPermissionSettingsMap",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    DealerAccountId = table.Column<long>(nullable: false),
                    ProductPermissionSettingsId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerMapProductPermissionSettingsMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerMapProductPermissionSettingsMap_DealerAccount_DealerAccountId",
                        column: x => x.DealerAccountId,
                        principalTable: "DealerAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerMapProductPermissionSettingsMap_ProductPermissionSettings_ProductPermissionSettingsId",
                        column: x => x.ProductPermissionSettingsId,
                        principalTable: "ProductPermissionSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealerMapProductPermissionSettingsMap_DealerAccountId",
                table: "DealerMapProductPermissionSettingsMap",
                column: "DealerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerMapProductPermissionSettingsMap_ProductPermissionSettingsId",
                table: "DealerMapProductPermissionSettingsMap",
                column: "ProductPermissionSettingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealerMapProductPermissionSettingsMap");
        }
    }
}
