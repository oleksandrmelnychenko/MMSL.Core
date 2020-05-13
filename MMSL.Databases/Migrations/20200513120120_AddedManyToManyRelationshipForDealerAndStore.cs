using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedManyToManyRelationshipForDealerAndStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_DealerAccount_DealerAccountId",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_DealerAccountId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "DealerAccountId",
                table: "Stores");

            migrationBuilder.CreateTable(
                name: "StoreMapDealerAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    DealerAccountId = table.Column<long>(nullable: false),
                    StoreId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMapDealerAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreMapDealerAccounts_DealerAccount_DealerAccountId",
                        column: x => x.DealerAccountId,
                        principalTable: "DealerAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreMapDealerAccounts_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreMapDealerAccounts_DealerAccountId",
                table: "StoreMapDealerAccounts",
                column: "DealerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMapDealerAccounts_StoreId",
                table: "StoreMapDealerAccounts",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreMapDealerAccounts");

            migrationBuilder.AddColumn<long>(
                name: "DealerAccountId",
                table: "Stores",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_DealerAccountId",
                table: "Stores",
                column: "DealerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_DealerAccount_DealerAccountId",
                table: "Stores",
                column: "DealerAccountId",
                principalTable: "DealerAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
