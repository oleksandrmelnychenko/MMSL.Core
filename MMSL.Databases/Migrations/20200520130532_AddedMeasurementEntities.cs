using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedMeasurementEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeasurementDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
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
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementMapDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    MeasurementId = table.Column<long>(nullable: false),
                    MeasurementDefinitionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementMapDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeasurementMapDefinitions_MeasurementDefinitions_MeasurementDefinitionId",
                        column: x => x.MeasurementDefinitionId,
                        principalTable: "MeasurementDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeasurementMapDefinitions_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementSizes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MeasurementId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeasurementSizes_Measurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementValues",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Value = table.Column<float>(nullable: false),
                    MeasurementDefinitionId = table.Column<long>(nullable: false),
                    MeasurementSizeId = table.Column<long>(nullable: true)
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
                name: "IX_MeasurementMapDefinitions_MeasurementDefinitionId",
                table: "MeasurementMapDefinitions",
                column: "MeasurementDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementMapDefinitions_MeasurementId",
                table: "MeasurementMapDefinitions",
                column: "MeasurementId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeasurementMapDefinitions");

            migrationBuilder.DropTable(
                name: "MeasurementValues");

            migrationBuilder.DropTable(
                name: "MeasurementDefinitions");

            migrationBuilder.DropTable(
                name: "MeasurementSizes");

            migrationBuilder.DropTable(
                name: "Measurements");
        }
    }
}
