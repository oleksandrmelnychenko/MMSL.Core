using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class RemoveStoreCustomerIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreCustomers_UserIdentities_UserIdentityId",
                table: "StoreCustomers");

            migrationBuilder.DropIndex(
                name: "IX_StoreCustomers_UserIdentityId",
                table: "StoreCustomers");

            migrationBuilder.DropColumn(
                name: "UserIdentityId",
                table: "StoreCustomers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserIdentityId",
                table: "StoreCustomers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_StoreCustomers_UserIdentityId",
                table: "StoreCustomers",
                column: "UserIdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreCustomers_UserIdentities_UserIdentityId",
                table: "StoreCustomers",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
