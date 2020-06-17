using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedUserIdentityRefToDealerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserIdentityId",
                table: "DealerAccount",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealerAccount_UserIdentityId",
                table: "DealerAccount",
                column: "UserIdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealerAccount_UserIdentities_UserIdentityId",
                table: "DealerAccount",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DealerAccount_UserIdentities_UserIdentityId",
                table: "DealerAccount");

            migrationBuilder.DropIndex(
                name: "IX_DealerAccount_UserIdentityId",
                table: "DealerAccount");

            migrationBuilder.DropColumn(
                name: "UserIdentityId",
                table: "DealerAccount");
        }
    }
}
