using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ReportCardConfig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "ReportCardData",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "ReportCardData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "ReportCardData",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReportCardConfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShowSubject = table.Column<bool>(nullable: false),
                    ShowDepartment = table.Column<bool>(nullable: false),
                    ShowCAScore = table.Column<bool>(nullable: false),
                    ShowExamScore = table.Column<bool>(nullable: false),
                    ComputeCA_Average = table.Column<bool>(nullable: false),
                    MultipleLegend = table.Column<bool>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<Guid>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    RefFirstTermScoreCompute = table.Column<bool>(nullable: false),
                    RefFirstTermScoreShow = table.Column<bool>(nullable: false),
                    RefSecondTermScoreCompute = table.Column<bool>(nullable: false),
                    RefSecondTermScoreShow = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardConfiguration_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardConfiguration_SchoolUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardConfiguration_SchoolUsers_LastUpdatedBy",
                        column: x => x.LastUpdatedBy,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardConfiguration_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardConfiguration_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportCardConfigurationLegend",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LegendName = table.Column<string>(nullable: true),
                    ReferenceRange = table.Column<string>(nullable: true),
                    ReferenceValue = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<Guid>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    RefFirstTermScoreCompute = table.Column<bool>(nullable: false),
                    RefFirstTermScoreShow = table.Column<bool>(nullable: false),
                    RefSecondTermScoreCompute = table.Column<bool>(nullable: false),
                    RefSecondTermScoreShow = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardConfigurationLegend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardConfigurationLegend_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardConfigurationLegend_SchoolUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardConfigurationLegend_SchoolUsers_LastUpdatedBy",
                        column: x => x.LastUpdatedBy,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardConfigurationLegend_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardConfigurationLegend_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportCardStudentPerformance",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TotalScoreObtained = table.Column<string>(nullable: true),
                    TotalScoreObtainable = table.Column<string>(nullable: true),
                    NoOfSubjects = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    DateComputed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardStudentPerformance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardStudentPerformance_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardStudentPerformance_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardStudentPerformance_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardStudentPerformance_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardStudentPerformance_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardStudentPerformance_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardStudentPerformance_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfiguration_CampusId",
                table: "ReportCardConfiguration",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfiguration_CreatedBy",
                table: "ReportCardConfiguration",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfiguration_LastUpdatedBy",
                table: "ReportCardConfiguration",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfiguration_SchoolId",
                table: "ReportCardConfiguration",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfiguration_TermId",
                table: "ReportCardConfiguration",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfigurationLegend_CampusId",
                table: "ReportCardConfigurationLegend",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfigurationLegend_CreatedBy",
                table: "ReportCardConfigurationLegend",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfigurationLegend_LastUpdatedBy",
                table: "ReportCardConfigurationLegend",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfigurationLegend_SchoolId",
                table: "ReportCardConfigurationLegend",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfigurationLegend_TermId",
                table: "ReportCardConfigurationLegend",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardStudentPerformance_CampusId",
                table: "ReportCardStudentPerformance",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardStudentPerformance_ClassGradeId",
                table: "ReportCardStudentPerformance",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardStudentPerformance_ClassId",
                table: "ReportCardStudentPerformance",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardStudentPerformance_SchoolId",
                table: "ReportCardStudentPerformance",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardStudentPerformance_SessionId",
                table: "ReportCardStudentPerformance",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardStudentPerformance_StudentId",
                table: "ReportCardStudentPerformance",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardStudentPerformance_TermId",
                table: "ReportCardStudentPerformance",
                column: "TermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportCardConfiguration");

            migrationBuilder.DropTable(
                name: "ReportCardConfigurationLegend");

            migrationBuilder.DropTable(
                name: "ReportCardStudentPerformance");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ReportCardData");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "ReportCardData");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "ReportCardData");
        }
    }
}
