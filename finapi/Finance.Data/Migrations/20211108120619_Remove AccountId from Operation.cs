using Microsoft.EntityFrameworkCore.Migrations;

namespace Finance.Data.Migrations
{
    public partial class RemoveAccountIdfromOperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Accounts_AccountId",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_AccountId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Operations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Operations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_AccountId",
                table: "Operations",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Accounts_AccountId",
                table: "Operations",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
