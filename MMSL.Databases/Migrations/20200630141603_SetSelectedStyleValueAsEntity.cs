using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class SetSelectedStyleValueAsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedValue",
                table: "CustomerProfileStyleConfigurations");

            migrationBuilder.AddColumn<long>(
                name: "UnitValueId",
                table: "CustomerProfileStyleConfigurations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfileStyleConfigurations_UnitValueId",
                table: "CustomerProfileStyleConfigurations",
                column: "UnitValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerProfileStyleConfigurations_UnitValues_UnitValueId",
                table: "CustomerProfileStyleConfigurations",
                column: "UnitValueId",
                principalTable: "UnitValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerProfileStyleConfigurations_UnitValues_UnitValueId",
                table: "CustomerProfileStyleConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_CustomerProfileStyleConfigurations_UnitValueId",
                table: "CustomerProfileStyleConfigurations");

            migrationBuilder.DropColumn(
                name: "UnitValueId",
                table: "CustomerProfileStyleConfigurations");

            migrationBuilder.AddColumn<string>(
                name: "SelectedValue",
                table: "CustomerProfileStyleConfigurations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
