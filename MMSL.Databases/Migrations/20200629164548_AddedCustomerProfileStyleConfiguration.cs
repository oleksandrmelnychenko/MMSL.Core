using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedCustomerProfileStyleConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerProfileStyleConfigurations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    SelectedValue = table.Column<string>(nullable: true),
                    OptionUnitId = table.Column<long>(nullable: false),
                    CustomerProductProfileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProfileStyleConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProfileStyleConfigurations_CustomerProductProfiles_CustomerProductProfileId",
                        column: x => x.CustomerProductProfileId,
                        principalTable: "CustomerProductProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProfileStyleConfigurations_OptionUnits_OptionUnitId",
                        column: x => x.OptionUnitId,
                        principalTable: "OptionUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfileStyleConfigurations_CustomerProductProfileId",
                table: "CustomerProfileStyleConfigurations",
                column: "CustomerProductProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfileStyleConfigurations_OptionUnitId",
                table: "CustomerProfileStyleConfigurations",
                column: "OptionUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProfileStyleConfigurations");
        }
    }
}
