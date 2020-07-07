using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedFabricsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fabrics",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    FabricCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Metres = table.Column<string>(nullable: true),
                    IsMetresVisible = table.Column<bool>(nullable: false),
                    Mill = table.Column<string>(nullable: true),
                    IsMillVisible = table.Column<bool>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    IsColorVisible = table.Column<bool>(nullable: false),
                    Composition = table.Column<string>(nullable: true),
                    IsCompositionVisible = table.Column<bool>(nullable: false),
                    GSM = table.Column<string>(nullable: true),
                    IsGSMVisible = table.Column<bool>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    IsCountVisible = table.Column<bool>(nullable: false),
                    Weave = table.Column<string>(nullable: true),
                    IsWeaveVisible = table.Column<bool>(nullable: false),
                    Pattern = table.Column<string>(nullable: true),
                    IsPatternVisible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fabrics", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fabrics");
        }
    }
}
