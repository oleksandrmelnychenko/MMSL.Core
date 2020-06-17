using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class FittingTypeRelatedToMeasurement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MeasurementId",
                table: "FittingTypes",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_FittingTypes_MeasurementId",
                table: "FittingTypes",
                column: "MeasurementId");

            migrationBuilder.AddForeignKey(
                name: "FK_FittingTypes_Measurements_MeasurementId",
                table: "FittingTypes",
                column: "MeasurementId",
                principalTable: "Measurements",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FittingTypes_Measurements_MeasurementId",
                table: "FittingTypes");

            migrationBuilder.DropIndex(
                name: "IX_FittingTypes_MeasurementId",
                table: "FittingTypes");

            migrationBuilder.DropColumn(
                name: "MeasurementId",
                table: "FittingTypes");
        }
    }
}
