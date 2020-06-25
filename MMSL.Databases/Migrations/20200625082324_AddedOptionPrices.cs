using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedOptionPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OptionPrices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    CurrencyTypeId = table.Column<long>(nullable: false),
                    OptionGroupId = table.Column<long>(nullable: true),
                    OptionUnitId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionPrices_CurrencyTypes_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalTable: "CurrencyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OptionPrices_OptionGroups_OptionGroupId",
                        column: x => x.OptionGroupId,
                        principalTable: "OptionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OptionPrices_OptionUnits_OptionUnitId",
                        column: x => x.OptionUnitId,
                        principalTable: "OptionUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OptionPrices_CurrencyTypeId",
                table: "OptionPrices",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionPrices_OptionGroupId",
                table: "OptionPrices",
                column: "OptionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionPrices_OptionUnitId",
                table: "OptionPrices",
                column: "OptionUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OptionPrices");
        }
    }
}
