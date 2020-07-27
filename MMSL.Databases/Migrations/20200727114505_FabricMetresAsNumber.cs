using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class FabricMetresAsNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [Fabrics] SET [Metres] = 0 WHERE ISNUMERIC([Metres]) = 0  AND [Metres] IS NOT NULL");
            migrationBuilder.AlterColumn<float>(
                name: "Metres",
                table: "Fabrics",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Metres",
                table: "Fabrics",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);
        }
    }
}
