using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedBodyPosturePropForOptionUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBodyPosture",
                table: "OptionUnits",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBodyPosture",
                table: "OptionUnits");
        }
    }
}
