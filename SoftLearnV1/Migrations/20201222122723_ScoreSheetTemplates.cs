using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ScoreSheetTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumni_ClassGrades_CampusId",
                table: "Alumni");

            migrationBuilder.CreateTable(
                name: "ScoreUploadSheetTemplates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    TeacherId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    SubjectId = table.Column<string>(nullable: true),
                    TotalNumberOfSubjects = table.Column<long>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false),
                    DateGenerated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreUploadSheetTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreUploadSheetTemplates_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreUploadSheetTemplates_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreUploadSheetTemplates_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreUploadSheetTemplates_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreUploadSheetTemplates_SchoolUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_ClassGradeId",
                table: "Alumni",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreUploadSheetTemplates_CampusId",
                table: "ScoreUploadSheetTemplates",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreUploadSheetTemplates_ClassGradeId",
                table: "ScoreUploadSheetTemplates",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreUploadSheetTemplates_ClassId",
                table: "ScoreUploadSheetTemplates",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreUploadSheetTemplates_SchoolId",
                table: "ScoreUploadSheetTemplates",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreUploadSheetTemplates_TeacherId",
                table: "ScoreUploadSheetTemplates",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumni_ClassGrades_ClassGradeId",
                table: "Alumni",
                column: "ClassGradeId",
                principalTable: "ClassGrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumni_ClassGrades_ClassGradeId",
                table: "Alumni");

            migrationBuilder.DropTable(
                name: "ScoreUploadSheetTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Alumni_ClassGradeId",
                table: "Alumni");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumni_ClassGrades_CampusId",
                table: "Alumni",
                column: "CampusId",
                principalTable: "ClassGrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
