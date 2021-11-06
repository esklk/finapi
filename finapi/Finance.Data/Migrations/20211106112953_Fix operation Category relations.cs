using Microsoft.EntityFrameworkCore.Migrations;

namespace Finance.Data.Migrations
{
    public partial class FixoperationCategoryrelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountOperationCategory");

            migrationBuilder.DropTable(
                name: "OperationCategoryUser");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "OperationCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OperationCategories_AccountId",
                table: "OperationCategories",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationCategories_Accounts_AccountId",
                table: "OperationCategories",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationCategories_Accounts_AccountId",
                table: "OperationCategories");

            migrationBuilder.DropIndex(
                name: "IX_OperationCategories_AccountId",
                table: "OperationCategories");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "OperationCategories");

            migrationBuilder.CreateTable(
                name: "AccountOperationCategory",
                columns: table => new
                {
                    AccountsId = table.Column<int>(type: "int", nullable: false),
                    OperationCategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountOperationCategory", x => new { x.AccountsId, x.OperationCategoriesId });
                    table.ForeignKey(
                        name: "FK_AccountOperationCategory_Accounts_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountOperationCategory_OperationCategories_OperationCatego~",
                        column: x => x.OperationCategoriesId,
                        principalTable: "OperationCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OperationCategoryUser",
                columns: table => new
                {
                    OperationCategoriesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationCategoryUser", x => new { x.OperationCategoriesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_OperationCategoryUser_OperationCategories_OperationCategorie~",
                        column: x => x.OperationCategoriesId,
                        principalTable: "OperationCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationCategoryUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AccountOperationCategory_OperationCategoriesId",
                table: "AccountOperationCategory",
                column: "OperationCategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationCategoryUser_UsersId",
                table: "OperationCategoryUser",
                column: "UsersId");
        }
    }
}
