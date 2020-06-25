using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class MovedMeasurementUnitToMeasurement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FittingTypes_MeasurementUnits_MeasurementUnitId",
                table: "FittingTypes");

            migrationBuilder.DropIndex(
                name: "IX_FittingTypes_MeasurementUnitId",
                table: "FittingTypes");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "FittingTypes");

            migrationBuilder.AddColumn<long>(
                name: "MeasurementUnitId",
                table: "Measurements",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_MeasurementUnitId",
                table: "Measurements",
                column: "MeasurementUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_MeasurementUnits_MeasurementUnitId",
                table: "Measurements",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_MeasurementUnits_MeasurementUnitId",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_MeasurementUnitId",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "Measurements");

            migrationBuilder.AddColumn<long>(
                name: "MeasurementUnitId",
                table: "FittingTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.CreateIndex(
                name: "IX_FittingTypes_MeasurementUnitId",
                table: "FittingTypes",
                column: "MeasurementUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_FittingTypes_MeasurementUnits_MeasurementUnitId",
                table: "FittingTypes",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
