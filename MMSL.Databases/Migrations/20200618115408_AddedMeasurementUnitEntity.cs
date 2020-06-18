using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedMeasurementUnitEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FittingTypes_DealerAccount_DealerAccountId",
                table: "FittingTypes");

            migrationBuilder.DropIndex(
                name: "IX_FittingTypes_DealerAccountId",
                table: "FittingTypes");

            migrationBuilder.DropColumn(
                name: "DealerAccountId",
                table: "FittingTypes");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "FittingTypes");

            migrationBuilder.AddColumn<long>(
                name: "MeasurementUnitId",
                table: "FittingTypes",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "MeasurementUnits",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementUnits", x => x.Id);
                });

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

            migrationBuilder.Sql(@"
INSERT [MeasurementUnits]
([IsDeleted], [Created], [Name], [Description])
VALUES(0, GETUTCDATE(), N'cm', N'centimeter')
INSERT [MeasurementUnits]
([IsDeleted], [Created], [Name], [Description])
VALUES(0, GETUTCDATE(), N'in', N'inch')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FittingTypes_MeasurementUnits_MeasurementUnitId",
                table: "FittingTypes");

            migrationBuilder.DropTable(
                name: "MeasurementUnits");

            migrationBuilder.DropIndex(
                name: "IX_FittingTypes_MeasurementUnitId",
                table: "FittingTypes");

            migrationBuilder.DropColumn(
                name: "MeasurementUnitId",
                table: "FittingTypes");

            migrationBuilder.AddColumn<long>(
                name: "DealerAccountId",
                table: "FittingTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "FittingTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FittingTypes_DealerAccountId",
                table: "FittingTypes",
                column: "DealerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_FittingTypes_DealerAccount_DealerAccountId",
                table: "FittingTypes",
                column: "DealerAccountId",
                principalTable: "DealerAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
