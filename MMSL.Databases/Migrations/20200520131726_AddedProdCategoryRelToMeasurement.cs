using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedProdCategoryRelToMeasurement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductCategoryId",
                table: "Measurements",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_ProductCategoryId",
                table: "Measurements",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_ProductCategories_ProductCategoryId",
                table: "Measurements",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_ProductCategories_ProductCategoryId",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_ProductCategoryId",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId",
                table: "Measurements");
        }
    }
}
