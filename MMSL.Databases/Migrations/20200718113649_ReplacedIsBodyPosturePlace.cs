using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class ReplacedIsBodyPosturePlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBodyPosture",
                table: "OptionUnits");

            migrationBuilder.AddColumn<bool>(
                name: "IsBodyPosture",
                table: "OptionGroups",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBodyPosture",
                table: "OptionGroups");

            migrationBuilder.AddColumn<bool>(
                name: "IsBodyPosture",
                table: "OptionUnits",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
