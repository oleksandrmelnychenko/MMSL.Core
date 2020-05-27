using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class ChangedMeasurementEntitiesToAllowBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_ProductCategories_ProductCategoryId",
                table: "Measurements");

            migrationBuilder.DropForeignKey(
                name: "FK_MeasurementSizes_Measurements_MeasurementId",
                table: "MeasurementSizes");

            migrationBuilder.DropTable(
                name: "MeasurementValues");

            migrationBuilder.DropIndex(
                name: "IX_MeasurementSizes_MeasurementId",
                table: "MeasurementSizes");

            migrationBuilder.DropColumn(
                name: "MeasurementId",
                table: "MeasurementSizes");

            migrationBuilder.AlterColumn<long>(
                name: "ProductCategoryId",
                table: "Measurements",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ParentMeasurementId",
                table: "Measurements",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MeasurementMapSizes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    MeasurementId = table.Column<long>(nullable: false),
                    MeasurementSizeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementMapSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeasurementMapSizes_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeasurementMapSizes_MeasurementSizes_MeasurementSizeId",
                        column: x => x.MeasurementSizeId,
                        principalTable: "MeasurementSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementMapValues",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    MeasurementId = table.Column<long>(nullable: false),
                    MeasurementSizeId = table.Column<long>(nullable: false),
                    MeasurementDefinitionId = table.Column<long>(nullable: false),
                    Value = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementMapValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeasurementMapValues_MeasurementDefinitions_MeasurementDefinitionId",
                        column: x => x.MeasurementDefinitionId,
                        principalTable: "MeasurementDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeasurementMapValues_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeasurementMapValues_MeasurementSizes_MeasurementSizeId",
                        column: x => x.MeasurementSizeId,
                        principalTable: "MeasurementSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_ParentMeasurementId",
                table: "Measurements",
                column: "ParentMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementMapSizes_MeasurementId",
                table: "MeasurementMapSizes",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementMapSizes_MeasurementSizeId",
                table: "MeasurementMapSizes",
                column: "MeasurementSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementMapValues_MeasurementDefinitionId",
                table: "MeasurementMapValues",
                column: "MeasurementDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementMapValues_MeasurementId",
                table: "MeasurementMapValues",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementMapValues_MeasurementSizeId",
                table: "MeasurementMapValues",
                column: "MeasurementSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_Measurements_ParentMeasurementId",
                table: "Measurements",
                column: "ParentMeasurementId",
                principalTable: "Measurements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_ProductCategories_ProductCategoryId",
                table: "Measurements",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_Measurements_ParentMeasurementId",
                table: "Measurements");

            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_ProductCategories_ProductCategoryId",
                table: "Measurements");

            migrationBuilder.DropTable(
                name: "MeasurementMapSizes");

            migrationBuilder.DropTable(
                name: "MeasurementMapValues");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_ParentMeasurementId",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "ParentMeasurementId",
                table: "Measurements");

            migrationBuilder.AddColumn<long>(
                name: "MeasurementId",
                table: "MeasurementSizes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "ProductCategoryId",
                table: "Measurements",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MeasurementValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MeasurementDefinitionId = table.Column<long>(type: "bigint", nullable: false),
                    MeasurementSizeId = table.Column<long>(type: "bigint", nullable: true),
                    Value = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeasurementValues_MeasurementDefinitions_MeasurementDefinitionId",
                        column: x => x.MeasurementDefinitionId,
                        principalTable: "MeasurementDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeasurementValues_MeasurementSizes_MeasurementSizeId",
                        column: x => x.MeasurementSizeId,
                        principalTable: "MeasurementSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementSizes_MeasurementId",
                table: "MeasurementSizes",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementValues_MeasurementDefinitionId",
                table: "MeasurementValues",
                column: "MeasurementDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementValues_MeasurementSizeId",
                table: "MeasurementValues",
                column: "MeasurementSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_ProductCategories_ProductCategoryId",
                table: "Measurements",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeasurementSizes_Measurements_MeasurementId",
                table: "MeasurementSizes",
                column: "MeasurementId",
                principalTable: "Measurements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
