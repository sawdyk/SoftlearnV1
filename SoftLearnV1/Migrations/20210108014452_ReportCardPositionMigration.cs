using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ReportCardPositionMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpened",
                table: "AcademicSessions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ReportCardData",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CAScore = table.Column<long>(nullable: false),
                    ExamScore = table.Column<long>(nullable: false),
                    TotalScore = table.Column<long>(nullable: false),
                    Position = table.Column<long>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    AdmissionNumber = table.Column<string>(nullable: true),
                    SubjectId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    AverageScore = table.Column<long>(nullable: false),
                    AveragePosition = table.Column<long>(nullable: false),
                    DateComputed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardData_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardData_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardData_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardData_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardData_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardData_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardData_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardData_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportCardPosition",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<Guid>(nullable: false),
                    AdmissionNumber = table.Column<string>(nullable: true),
                    TotalScore = table.Column<long>(nullable: false),
                    Position = table.Column<long>(nullable: false),
                    SubjectComputed = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    DateComputed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardPosition_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardPosition_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardPosition_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardPosition_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardPosition_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardPosition_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardPosition_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardData_CampusId",
                table: "ReportCardData",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardData_ClassGradeId",
                table: "ReportCardData",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardData_ClassId",
                table: "ReportCardData",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardData_SchoolId",
                table: "ReportCardData",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardData_SessionId",
                table: "ReportCardData",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardData_StudentId",
                table: "ReportCardData",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardData_SubjectId",
                table: "ReportCardData",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardData_TermId",
                table: "ReportCardData",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardPosition_CampusId",
                table: "ReportCardPosition",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardPosition_ClassGradeId",
                table: "ReportCardPosition",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardPosition_ClassId",
                table: "ReportCardPosition",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardPosition_SchoolId",
                table: "ReportCardPosition",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardPosition_SessionId",
                table: "ReportCardPosition",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardPosition_StudentId",
                table: "ReportCardPosition",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardPosition_TermId",
                table: "ReportCardPosition",
                column: "TermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportCardData");

            migrationBuilder.DropTable(
                name: "ReportCardPosition");

            migrationBuilder.DropColumn(
                name: "IsOpened",
                table: "AcademicSessions");
        }
    }
}
