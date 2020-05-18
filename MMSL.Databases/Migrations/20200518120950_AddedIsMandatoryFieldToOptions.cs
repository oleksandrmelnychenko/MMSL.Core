using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedIsMandatoryFieldToOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "OptionUnits");

            migrationBuilder.AddColumn<bool>(
                name: "IsMandatory",
                table: "OptionUnits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMandatory",
                table: "OptionGroups",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMandatory",
                table: "OptionUnits");

            migrationBuilder.DropColumn(
                name: "IsMandatory",
                table: "OptionGroups");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OptionUnits",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
