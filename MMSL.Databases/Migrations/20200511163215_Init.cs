using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountTypeCompanyInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypeCompanyInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserIdentities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: false),
                    PasswordSalt = table.Column<string>(nullable: false),
                    LastLoggedIn = table.Column<DateTime>(nullable: true),
                    IsPasswordExpired = table.Column<bool>(nullable: false),
                    ForceChangePassword = table.Column<bool>(nullable: false),
                    CanUserResetExpiredPassword = table.Column<bool>(nullable: false),
                    PasswordExpiresAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    UserIdentityId = table.Column<long>(nullable: false),
                    CompanyInfoId = table.Column<long>(nullable: true),
                    UserAccountType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountType_AccountTypeCompanyInfo_CompanyInfoId",
                        column: x => x.CompanyInfoId,
                        principalTable: "AccountTypeCompanyInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountType_UserIdentities_UserIdentityId",
                        column: x => x.UserIdentityId,
                        principalTable: "UserIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountType_CompanyInfoId",
                table: "AccountType",
                column: "CompanyInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountType_UserIdentityId",
                table: "AccountType",
                column: "UserIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentities_Email",
                table: "UserIdentities",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountType");

            migrationBuilder.DropTable(
                name: "AccountTypeCompanyInfo");

            migrationBuilder.DropTable(
                name: "UserIdentities");
        }
    }
}
