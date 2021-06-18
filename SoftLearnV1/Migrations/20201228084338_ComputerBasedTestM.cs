using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ComputerBasedTestM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveInActiveStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveInActiveStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CbtCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CbtCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CbtTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CbtTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComputerBasedTest",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SubjectId = table.Column<long>(nullable: false),
                    TeacherId = table.Column<Guid>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    TypeId = table.Column<long>(nullable: false),
                    CategoryId = table.Column<long>(nullable: false),
                    Duration = table.Column<long>(nullable: false),
                    PassMark = table.Column<long>(nullable: false),
                    TermsAndConditions = table.Column<string>(nullable: true),
                    StatusId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerBasedTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComputerBasedTest_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerBasedTest_CbtCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CbtCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerBasedTest_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerBasedTest_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerBasedTest_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerBasedTest_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerBasedTest_ActiveInActiveStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ActiveInActiveStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerBasedTest_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerBasedTest_SchoolUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerBasedTest_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerBasedTest_CbtTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CbtTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CbtQuestions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CbtId = table.Column<long>(nullable: false),
                    QuestionTypeId = table.Column<long>(nullable: false),
                    Question = table.Column<string>(nullable: true),
                    Option1 = table.Column<string>(nullable: true),
                    Option2 = table.Column<string>(nullable: true),
                    Option3 = table.Column<string>(nullable: true),
                    Option4 = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CbtQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CbtQuestions_ComputerBasedTest_CbtId",
                        column: x => x.CbtId,
                        principalTable: "ComputerBasedTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CbtQuestions_QuestionTypes_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CbtResults",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<Guid>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    CbtId = table.Column<long>(nullable: false),
                    TypeId = table.Column<long>(nullable: false),
                    CategoryId = table.Column<long>(nullable: false),
                    NoOfQuestion = table.Column<long>(nullable: false),
                    RightAnswers = table.Column<long>(nullable: false),
                    WrongAnswers = table.Column<long>(nullable: false),
                    TotalScore = table.Column<long>(nullable: false),
                    PercentageScore = table.Column<decimal>(nullable: false),
                    StatuId = table.Column<long>(nullable: false),
                    DateTaken = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CbtResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CbtResults_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CbtResults_CbtCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CbtCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CbtResults_ComputerBasedTest_CbtId",
                        column: x => x.CbtId,
                        principalTable: "ComputerBasedTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CbtResults_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CbtResults_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CbtResults_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CbtResults_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CbtResults_ScoreStatus_StatuId",
                        column: x => x.StatuId,
                        principalTable: "ScoreStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CbtResults_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CbtResults_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CbtResults_CbtTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CbtTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ActiveInActiveStatus",
                columns: new[] { "Id", "StatusName" },
                values: new object[,]
                {
                    { 1L, "Active" },
                    { 2L, "InActive" }
                });

            migrationBuilder.InsertData(
                table: "CbtCategory",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { 1L, "Exam" },
                    { 2L, "CA" }
                });

            migrationBuilder.InsertData(
                table: "CbtTypes",
                columns: new[] { "Id", "TypeName" },
                values: new object[,]
                {
                    { 1L, "Practice" },
                    { 2L, "Schedule" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CbtQuestions_CbtId",
                table: "CbtQuestions",
                column: "CbtId");

            migrationBuilder.CreateIndex(
                name: "IX_CbtQuestions_QuestionTypeId",
                table: "CbtQuestions",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CbtResults_CampusId",
                table: "CbtResults",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_CbtResults_CategoryId",
                table: "CbtResults",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CbtResults_CbtId",
                table: "CbtResults",
                column: "CbtId");

            migrationBuilder.CreateIndex(
                name: "IX_CbtResults_ClassGradeId",
                table: "CbtResults",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_CbtResults_ClassId",
                table: "CbtResults",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CbtResults_SchoolId",
                table: "CbtResults",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_CbtResults_SessionId",
                table: "CbtResults",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CbtResults_StatuId",
                table: "CbtResults",
                column: "StatuId");

            migrationBuilder.CreateIndex(
                name: "IX_CbtResults_StudentId",
                table: "CbtResults",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CbtResults_TermId",
                table: "CbtResults",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_CbtResults_TypeId",
                table: "CbtResults",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerBasedTest_CampusId",
                table: "ComputerBasedTest",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerBasedTest_CategoryId",
                table: "ComputerBasedTest",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerBasedTest_ClassGradeId",
                table: "ComputerBasedTest",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerBasedTest_ClassId",
                table: "ComputerBasedTest",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerBasedTest_SchoolId",
                table: "ComputerBasedTest",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerBasedTest_SessionId",
                table: "ComputerBasedTest",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerBasedTest_StatusId",
                table: "ComputerBasedTest",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerBasedTest_SubjectId",
                table: "ComputerBasedTest",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerBasedTest_TeacherId",
                table: "ComputerBasedTest",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerBasedTest_TermId",
                table: "ComputerBasedTest",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerBasedTest_TypeId",
                table: "ComputerBasedTest",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CbtQuestions");

            migrationBuilder.DropTable(
                name: "CbtResults");

            migrationBuilder.DropTable(
                name: "ComputerBasedTest");

            migrationBuilder.DropTable(
                name: "CbtCategory");

            migrationBuilder.DropTable(
                name: "ActiveInActiveStatus");

            migrationBuilder.DropTable(
                name: "CbtTypes");
        }
    }
}
