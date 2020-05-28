using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class CreateFittingTypeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "MeasurementSizeId",
                table: "MeasurementMapValues",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "MeasurementId",
                table: "MeasurementMapValues",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "FittingTypeId",
                table: "MeasurementMapValues",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FittingTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    DealerAccountId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FittingTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FittingTypes_DealerAccount_DealerAccountId",
                        column: x => x.DealerAccountId,
                        principalTable: "DealerAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementMapValues_FittingTypeId",
                table: "MeasurementMapValues",
                column: "FittingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FittingTypes_DealerAccountId",
                table: "FittingTypes",
                column: "DealerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeasurementMapValues_FittingTypes_FittingTypeId",
                table: "MeasurementMapValues",
                column: "FittingTypeId",
                principalTable: "FittingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeasurementMapValues_FittingTypes_FittingTypeId",
                table: "MeasurementMapValues");

            migrationBuilder.DropTable(
                name: "FittingTypes");

            migrationBuilder.DropIndex(
                name: "IX_MeasurementMapValues_FittingTypeId",
                table: "MeasurementMapValues");

            migrationBuilder.DropColumn(
                name: "FittingTypeId",
                table: "MeasurementMapValues");

            migrationBuilder.AlterColumn<long>(
                name: "MeasurementSizeId",
                table: "MeasurementMapValues",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MeasurementId",
                table: "MeasurementMapValues",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
