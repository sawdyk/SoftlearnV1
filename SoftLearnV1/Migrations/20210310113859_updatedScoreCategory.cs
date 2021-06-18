using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class updatedScoreCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategory_CategoryId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.RenameColumn(
                name: "SubCategory",
                table: "ScoreSubCategoryConfig",
                newName: "SubCategoryName");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ScoreSubCategoryConfig",
                newName: "CategoryConfigId");

            migrationBuilder.RenameIndex(
                name: "IX_ScoreSubCategoryConfig_CategoryId",
                table: "ScoreSubCategoryConfig",
                newName: "IX_ScoreSubCategoryConfig_CategoryConfigId");

            migrationBuilder.CreateTable(
                name: "ScoreCategoryConfig",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    Percentage = table.Column<decimal>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreCategoryConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_ScoreCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ScoreCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_CampusId",
                table: "ScoreCategoryConfig",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_CategoryId",
                table: "ScoreCategoryConfig",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_ClassId",
                table: "ScoreCategoryConfig",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_SchoolId",
                table: "ScoreCategoryConfig",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_SessionId",
                table: "ScoreCategoryConfig",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_TermId",
                table: "ScoreCategoryConfig",
                column: "TermId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategoryConfig_CategoryConfigId",
                table: "ScoreSubCategoryConfig",
                column: "CategoryConfigId",
                principalTable: "ScoreCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategoryConfig_CategoryConfigId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.DropTable(
                name: "ScoreCategoryConfig");

            migrationBuilder.RenameColumn(
                name: "SubCategoryName",
                table: "ScoreSubCategoryConfig",
                newName: "SubCategory");

            migrationBuilder.RenameColumn(
                name: "CategoryConfigId",
                table: "ScoreSubCategoryConfig",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ScoreSubCategoryConfig_CategoryConfigId",
                table: "ScoreSubCategoryConfig",
                newName: "IX_ScoreSubCategoryConfig_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategory_CategoryId",
                table: "ScoreSubCategoryConfig",
                column: "CategoryId",
                principalTable: "ScoreCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
