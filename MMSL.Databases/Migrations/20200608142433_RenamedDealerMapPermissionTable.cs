using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class RenamedDealerMapPermissionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerMapProductPermissionSettingsMap_DealerAccount_DealerAccountId",
                table: "DealerMapProductPermissionSettingsMap");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerMapProductPermissionSettingsMap_ProductPermissionSettings_ProductPermissionSettingsId",
                table: "DealerMapProductPermissionSettingsMap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DealerMapProductPermissionSettingsMap",
                table: "DealerMapProductPermissionSettingsMap");

            migrationBuilder.RenameTable(
                name: "DealerMapProductPermissionSettingsMap",
                newName: "DealerMapProductPermissionSettings");

            migrationBuilder.RenameIndex(
                name: "IX_DealerMapProductPermissionSettingsMap_ProductPermissionSettingsId",
                table: "DealerMapProductPermissionSettings",
                newName: "IX_DealerMapProductPermissionSettings_ProductPermissionSettingsId");

            migrationBuilder.RenameIndex(
                name: "IX_DealerMapProductPermissionSettingsMap_DealerAccountId",
                table: "DealerMapProductPermissionSettings",
                newName: "IX_DealerMapProductPermissionSettings_DealerAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DealerMapProductPermissionSettings",
                table: "DealerMapProductPermissionSettings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerMapProductPermissionSettings_DealerAccount_DealerAccountId",
                table: "DealerMapProductPermissionSettings",
                column: "DealerAccountId",
                principalTable: "DealerAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerMapProductPermissionSettings_ProductPermissionSettings_ProductPermissionSettingsId",
                table: "DealerMapProductPermissionSettings",
                column: "ProductPermissionSettingsId",
                principalTable: "ProductPermissionSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerMapProductPermissionSettings_DealerAccount_DealerAccountId",
                table: "DealerMapProductPermissionSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerMapProductPermissionSettings_ProductPermissionSettings_ProductPermissionSettingsId",
                table: "DealerMapProductPermissionSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DealerMapProductPermissionSettings",
                table: "DealerMapProductPermissionSettings");

            migrationBuilder.RenameTable(
                name: "DealerMapProductPermissionSettings",
                newName: "DealerMapProductPermissionSettingsMap");

            migrationBuilder.RenameIndex(
                name: "IX_DealerMapProductPermissionSettings_ProductPermissionSettingsId",
                table: "DealerMapProductPermissionSettingsMap",
                newName: "IX_DealerMapProductPermissionSettingsMap_ProductPermissionSettingsId");

            migrationBuilder.RenameIndex(
                name: "IX_DealerMapProductPermissionSettings_DealerAccountId",
                table: "DealerMapProductPermissionSettingsMap",
                newName: "IX_DealerMapProductPermissionSettingsMap_DealerAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DealerMapProductPermissionSettingsMap",
                table: "DealerMapProductPermissionSettingsMap",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerMapProductPermissionSettingsMap_DealerAccount_DealerAccountId",
                table: "DealerMapProductPermissionSettingsMap",
                column: "DealerAccountId",
                principalTable: "DealerAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerMapProductPermissionSettingsMap_ProductPermissionSettings_ProductPermissionSettingsId",
                table: "DealerMapProductPermissionSettingsMap",
                column: "ProductPermissionSettingsId",
                principalTable: "ProductPermissionSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
