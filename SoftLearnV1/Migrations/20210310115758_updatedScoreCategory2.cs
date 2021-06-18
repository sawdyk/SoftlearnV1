using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class updatedScoreCategory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategoryConfig_CategoryConfigId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryConfigId",
                table: "ScoreSubCategoryConfig",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "ScoreSubCategoryConfig",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategoryConfig_CategoryConfigId",
                table: "ScoreSubCategoryConfig",
                column: "CategoryConfigId",
                principalTable: "ScoreCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategoryConfig_CategoryConfigId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryConfigId",
                table: "ScoreSubCategoryConfig",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategoryConfig_CategoryConfigId",
                table: "ScoreSubCategoryConfig",
                column: "CategoryConfigId",
                principalTable: "ScoreCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
