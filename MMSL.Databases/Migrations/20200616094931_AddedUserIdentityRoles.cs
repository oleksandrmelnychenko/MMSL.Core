using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations {
    public partial class AddedUserIdentityRoles : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "UserIdentityRoleTypes",
                columns: table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    RoleType = table.Column<int>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_UserIdentityRoleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: true, defaultValueSql: "getutcdate()"),
                    LastModified = table.Column<DateTime>(nullable: true),
                    UserRoleTypeId = table.Column<long>(nullable: false),
                    UserIdentityId = table.Column<long>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_UserIdentities_UserIdentityId",
                        column: x => x.UserIdentityId,
                        principalTable: "UserIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_UserIdentityRoleTypes_UserRoleTypeId",
                        column: x => x.UserRoleTypeId,
                        principalTable: "UserIdentityRoleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserIdentityId",
                table: "UserRoles",
                column: "UserIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserRoleTypeId",
                table: "UserRoles",
                column: "UserRoleTypeId");

            migrationBuilder.Sql(
                @"INSERT [UserIdentityRoleTypes] ([IsDeleted], [Created], [Name], [RoleType]) 
                    VALUES (0, GETUTCDATE(), N'Administrator', 0)
                INSERT [UserIdentityRoleTypes] ([IsDeleted], [Created], [Name], [RoleType]) 
                    VALUES (0, GETUTCDATE(), N'Manufacturer', 1)
                INSERT [UserIdentityRoleTypes] ([IsDeleted], [Created], [Name], [RoleType]) 
                    VALUES (0, GETUTCDATE(), N'Dealer', 2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserIdentityRoleTypes");
        }
    }
}
