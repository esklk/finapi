using Microsoft.EntityFrameworkCore.Migrations;

namespace Finance.Data.Migrations
{
    public partial class AddCategoryIdtoOperations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_OperationCategories_CategoryId",
                table: "Operations");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Operations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_OperationCategories_CategoryId",
                table: "Operations",
                column: "CategoryId",
                principalTable: "OperationCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_OperationCategories_CategoryId",
                table: "Operations");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Operations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_OperationCategories_CategoryId",
                table: "Operations",
                column: "CategoryId",
                principalTable: "OperationCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
