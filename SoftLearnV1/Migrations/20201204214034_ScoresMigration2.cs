using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ScoresMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ScoreGrading",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    LowestRange = table.Column<long>(nullable: false),
                    HighestRange = table.Column<long>(nullable: false),
                    Grade = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreGrading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreGrading_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreGrading_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreGrading_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoreSubCategoryConfig",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<long>(nullable: false),
                    ScoreCategoryId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    SubCategory = table.Column<string>(nullable: true),
                    ScoreObtainable = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreSubCategoryConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_ScoreCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ScoreCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_ScoreCategoryConfig_ScoreCategoryId",
                        column: x => x.ScoreCategoryId,
                        principalTable: "ScoreCategoryConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BehavioralScores",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    AdmissionNumber = table.Column<string>(nullable: true),
                    MarkObtainable = table.Column<long>(nullable: false),
                    MarkObtained = table.Column<long>(nullable: false),
                    ScoreCategoryId = table.Column<long>(nullable: false),
                    ScoreSubCategoryId = table.Column<long>(nullable: false),
                    UploadedById = table.Column<Guid>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehavioralScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BehavioralScores_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BehavioralScores_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BehavioralScores_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BehavioralScores_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BehavioralScores_ScoreCategoryConfig_ScoreCategoryId",
                        column: x => x.ScoreCategoryId,
                        principalTable: "ScoreCategoryConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BehavioralScores_ScoreSubCategoryConfig_ScoreSubCategoryId",
                        column: x => x.ScoreSubCategoryId,
                        principalTable: "ScoreSubCategoryConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BehavioralScores_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BehavioralScores_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BehavioralScores_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BehavioralScores_SchoolUsers_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContinousAssessmentScores",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    SubjectId = table.Column<long>(nullable: false),
                    DepartmentId = table.Column<long>(nullable: true),
                    StudentId = table.Column<Guid>(nullable: false),
                    AdmissionNumber = table.Column<string>(nullable: true),
                    MarkObtainable = table.Column<long>(nullable: false),
                    MarkObtained = table.Column<long>(nullable: false),
                    ScoreCategoryId = table.Column<long>(nullable: false),
                    ScoreSubCategoryId = table.Column<long>(nullable: false),
                    UploadedById = table.Column<Guid>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContinousAssessmentScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContinousAssessmentScores_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContinousAssessmentScores_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContinousAssessmentScores_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContinousAssessmentScores_SubjectDepartment_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "SubjectDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContinousAssessmentScores_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContinousAssessmentScores_ScoreCategoryConfig_ScoreCategoryId",
                        column: x => x.ScoreCategoryId,
                        principalTable: "ScoreCategoryConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContinousAssessmentScores_ScoreSubCategoryConfig_ScoreSubCat~",
                        column: x => x.ScoreSubCategoryId,
                        principalTable: "ScoreSubCategoryConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContinousAssessmentScores_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContinousAssessmentScores_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContinousAssessmentScores_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContinousAssessmentScores_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContinousAssessmentScores_SchoolUsers_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExaminationScores",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    SubjectId = table.Column<long>(nullable: false),
                    DepartmentId = table.Column<long>(nullable: true),
                    StudentId = table.Column<Guid>(nullable: false),
                    AdmissionNumber = table.Column<string>(nullable: true),
                    MarkObtainable = table.Column<long>(nullable: false),
                    MarkObtained = table.Column<long>(nullable: false),
                    ScoreCategoryId = table.Column<long>(nullable: false),
                    ScoreSubCategoryId = table.Column<long>(nullable: false),
                    UploadedById = table.Column<Guid>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExaminationScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExaminationScores_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExaminationScores_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExaminationScores_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExaminationScores_SubjectDepartment_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "SubjectDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExaminationScores_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExaminationScores_ScoreCategoryConfig_ScoreCategoryId",
                        column: x => x.ScoreCategoryId,
                        principalTable: "ScoreCategoryConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExaminationScores_ScoreSubCategoryConfig_ScoreSubCategoryId",
                        column: x => x.ScoreSubCategoryId,
                        principalTable: "ScoreSubCategoryConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExaminationScores_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExaminationScores_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExaminationScores_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExaminationScores_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExaminationScores_SchoolUsers_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtraCurricularScores",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    AdmissionNumber = table.Column<string>(nullable: true),
                    MarkObtainable = table.Column<long>(nullable: false),
                    MarkObtained = table.Column<long>(nullable: false),
                    ScoreCategoryId = table.Column<long>(nullable: false),
                    ScoreSubCategoryId = table.Column<long>(nullable: false),
                    UploadedById = table.Column<Guid>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraCurricularScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtraCurricularScores_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraCurricularScores_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraCurricularScores_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraCurricularScores_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraCurricularScores_ScoreCategoryConfig_ScoreCategoryId",
                        column: x => x.ScoreCategoryId,
                        principalTable: "ScoreCategoryConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraCurricularScores_ScoreSubCategoryConfig_ScoreSubCategor~",
                        column: x => x.ScoreSubCategoryId,
                        principalTable: "ScoreSubCategoryConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraCurricularScores_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraCurricularScores_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraCurricularScores_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraCurricularScores_SchoolUsers_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BehavioralScores_CampusId",
                table: "BehavioralScores",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_BehavioralScores_ClassGradeId",
                table: "BehavioralScores",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_BehavioralScores_ClassId",
                table: "BehavioralScores",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_BehavioralScores_SchoolId",
                table: "BehavioralScores",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_BehavioralScores_ScoreCategoryId",
                table: "BehavioralScores",
                column: "ScoreCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BehavioralScores_ScoreSubCategoryId",
                table: "BehavioralScores",
                column: "ScoreSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BehavioralScores_SessionId",
                table: "BehavioralScores",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_BehavioralScores_StudentId",
                table: "BehavioralScores",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_BehavioralScores_TermId",
                table: "BehavioralScores",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_BehavioralScores_UploadedById",
                table: "BehavioralScores",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_ContinousAssessmentScores_CampusId",
                table: "ContinousAssessmentScores",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ContinousAssessmentScores_ClassGradeId",
                table: "ContinousAssessmentScores",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContinousAssessmentScores_ClassId",
                table: "ContinousAssessmentScores",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ContinousAssessmentScores_DepartmentId",
                table: "ContinousAssessmentScores",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContinousAssessmentScores_SchoolId",
                table: "ContinousAssessmentScores",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ContinousAssessmentScores_ScoreCategoryId",
                table: "ContinousAssessmentScores",
                column: "ScoreCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContinousAssessmentScores_ScoreSubCategoryId",
                table: "ContinousAssessmentScores",
                column: "ScoreSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContinousAssessmentScores_SessionId",
                table: "ContinousAssessmentScores",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContinousAssessmentScores_StudentId",
                table: "ContinousAssessmentScores",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContinousAssessmentScores_SubjectId",
                table: "ContinousAssessmentScores",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ContinousAssessmentScores_TermId",
                table: "ContinousAssessmentScores",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ContinousAssessmentScores_UploadedById",
                table: "ContinousAssessmentScores",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationScores_CampusId",
                table: "ExaminationScores",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationScores_ClassGradeId",
                table: "ExaminationScores",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationScores_ClassId",
                table: "ExaminationScores",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationScores_DepartmentId",
                table: "ExaminationScores",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationScores_SchoolId",
                table: "ExaminationScores",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationScores_ScoreCategoryId",
                table: "ExaminationScores",
                column: "ScoreCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationScores_ScoreSubCategoryId",
                table: "ExaminationScores",
                column: "ScoreSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationScores_SessionId",
                table: "ExaminationScores",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationScores_StudentId",
                table: "ExaminationScores",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationScores_SubjectId",
                table: "ExaminationScores",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationScores_TermId",
                table: "ExaminationScores",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationScores_UploadedById",
                table: "ExaminationScores",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraCurricularScores_CampusId",
                table: "ExtraCurricularScores",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraCurricularScores_ClassGradeId",
                table: "ExtraCurricularScores",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraCurricularScores_ClassId",
                table: "ExtraCurricularScores",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraCurricularScores_SchoolId",
                table: "ExtraCurricularScores",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraCurricularScores_ScoreCategoryId",
                table: "ExtraCurricularScores",
                column: "ScoreCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraCurricularScores_ScoreSubCategoryId",
                table: "ExtraCurricularScores",
                column: "ScoreSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraCurricularScores_SessionId",
                table: "ExtraCurricularScores",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraCurricularScores_StudentId",
                table: "ExtraCurricularScores",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraCurricularScores_TermId",
                table: "ExtraCurricularScores",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraCurricularScores_UploadedById",
                table: "ExtraCurricularScores",
                column: "UploadedById");

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

            migrationBuilder.CreateIndex(
                name: "IX_ScoreGrading_CampusId",
                table: "ScoreGrading",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreGrading_ClassId",
                table: "ScoreGrading",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreGrading_SchoolId",
                table: "ScoreGrading",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_CampusId",
                table: "ScoreSubCategoryConfig",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_CategoryId",
                table: "ScoreSubCategoryConfig",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_ClassId",
                table: "ScoreSubCategoryConfig",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_SchoolId",
                table: "ScoreSubCategoryConfig",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_ScoreCategoryId",
                table: "ScoreSubCategoryConfig",
                column: "ScoreCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_SessionId",
                table: "ScoreSubCategoryConfig",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_TermId",
                table: "ScoreSubCategoryConfig",
                column: "TermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BehavioralScores");

            migrationBuilder.DropTable(
                name: "ContinousAssessmentScores");

            migrationBuilder.DropTable(
                name: "ExaminationScores");

            migrationBuilder.DropTable(
                name: "ExtraCurricularScores");

            migrationBuilder.DropTable(
                name: "ScoreGrading");

            migrationBuilder.DropTable(
                name: "ScoreSubCategoryConfig");

            migrationBuilder.DropTable(
                name: "ScoreCategoryConfig");
        }
    }
}
