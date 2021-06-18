using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class schoolFee3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeTemplateList_FeeCategory_FeeCategoryId",
                table: "FeeTemplateList");

            migrationBuilder.DropIndex(
                name: "IX_FeeTemplateList_FeeCategoryId",
                table: "FeeTemplateList");

            migrationBuilder.DropColumn(
                name: "FeeCategoryId",
                table: "FeeTemplateList");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FeeCategoryId",
                table: "FeeTemplateList",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplateList_FeeCategoryId",
                table: "FeeTemplateList",
                column: "FeeCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeeTemplateList_FeeCategory_FeeCategoryId",
                table: "FeeTemplateList",
                column: "FeeCategoryId",
                principalTable: "FeeCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
