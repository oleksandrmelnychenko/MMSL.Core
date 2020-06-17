using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations {
    public partial class AddedUserIdentityRefToStoreCustomerEntity : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerAccount_UserIdentities_UserIdentityId",
                table: "DealerAccount");

            migrationBuilder.AddColumn<long>(
                name: "UserIdentityId",
                table: "StoreCustomers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "UserIdentityId",
                table: "DealerAccount",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreCustomers_UserIdentityId",
                table: "StoreCustomers",
                column: "UserIdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerAccount_UserIdentities_UserIdentityId",
                table: "DealerAccount",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCustomers_UserIdentities_UserIdentityId",
                table: "StoreCustomers",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql(
                @"INSERT [UserIdentityRoleTypes] ([IsDeleted], [Created], [Name], [RoleType]) 
                    VALUES (0, GETUTCDATE(), N'Customer', 3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerAccount_UserIdentities_UserIdentityId",
                table: "DealerAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreCustomers_UserIdentities_UserIdentityId",
                table: "StoreCustomers");

            migrationBuilder.DropIndex(
                name: "IX_StoreCustomers_UserIdentityId",
                table: "StoreCustomers");

            migrationBuilder.DropColumn(
                name: "UserIdentityId",
                table: "StoreCustomers");

            migrationBuilder.AlterColumn<long>(
                name: "UserIdentityId",
                table: "DealerAccount",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_DealerAccount_UserIdentities_UserIdentityId",
                table: "DealerAccount",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
