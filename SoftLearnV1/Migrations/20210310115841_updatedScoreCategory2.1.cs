using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class updatedScoreCategory21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategoryConfig_CategoryConfigId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.DropIndex(
                name: "IX_ScoreSubCategoryConfig_CategoryConfigId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.DropColumn(
                name: "CategoryConfigId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_CategoryId",
                table: "ScoreSubCategoryConfig",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategory_CategoryId",
                table: "ScoreSubCategoryConfig",
                column: "CategoryId",
                principalTable: "ScoreCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategory_CategoryId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.DropIndex(
                name: "IX_ScoreSubCategoryConfig_CategoryId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.AddColumn<long>(
                name: "CategoryConfigId",
                table: "ScoreSubCategoryConfig",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_CategoryConfigId",
                table: "ScoreSubCategoryConfig",
                column: "CategoryConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategoryConfig_CategoryConfigId",
                table: "ScoreSubCategoryConfig",
                column: "CategoryConfigId",
                principalTable: "ScoreCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
