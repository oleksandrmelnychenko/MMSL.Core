using Microsoft.EntityFrameworkCore.Migrations;

namespace MMSL.Databases.Migrations
{
    public partial class AddedUserIdentityForFabric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserIdentityId",
                table: "Fabrics",
                nullable: false,
                defaultValue: 2L);

            migrationBuilder.CreateIndex(
                name: "IX_Fabrics_UserIdentityId",
                table: "Fabrics",
                column: "UserIdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fabrics_UserIdentities_UserIdentityId",
                table: "Fabrics",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fabrics_UserIdentities_UserIdentityId",
                table: "Fabrics");

            migrationBuilder.DropIndex(
                name: "IX_Fabrics_UserIdentityId",
                table: "Fabrics");

            migrationBuilder.DropColumn(
                name: "UserIdentityId",
                table: "Fabrics");
        }
    }
}
