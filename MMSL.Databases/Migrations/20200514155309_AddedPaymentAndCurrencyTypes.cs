using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedPaymentAndCurrencyTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "DealerAccount");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "DealerAccount");

            migrationBuilder.AddColumn<long>(
                name: "CurrencyTypeId",
                table: "DealerAccount",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PaymentTypeId",
                table: "DealerAccount",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CurrencyTypes",
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
                    table.PrimaryKey("PK_CurrencyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
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
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealerAccount_CurrencyTypeId",
                table: "DealerAccount",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerAccount_PaymentTypeId",
                table: "DealerAccount",
                column: "PaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerAccount_CurrencyTypes_CurrencyTypeId",
                table: "DealerAccount",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DealerAccount_PaymentTypes_PaymentTypeId",
                table: "DealerAccount",
                column: "PaymentTypeId",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerAccount_CurrencyTypes_CurrencyTypeId",
                table: "DealerAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_DealerAccount_PaymentTypes_PaymentTypeId",
                table: "DealerAccount");

            migrationBuilder.DropTable(
                name: "CurrencyTypes");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_DealerAccount_CurrencyTypeId",
                table: "DealerAccount");

            migrationBuilder.DropIndex(
                name: "IX_DealerAccount_PaymentTypeId",
                table: "DealerAccount");

            migrationBuilder.DropColumn(
                name: "CurrencyTypeId",
                table: "DealerAccount");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "DealerAccount");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "DealerAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "DealerAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
