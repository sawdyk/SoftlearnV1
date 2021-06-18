using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ReportCardConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportCardCommentConfig",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CommentBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardCommentConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportCardCommentsList",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    UploadedById = table.Column<Guid>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardCommentsList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardCommentsList_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardCommentsList_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardCommentsList_SchoolUsers_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportCardNextTermBegins",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    UploadedById = table.Column<Guid>(nullable: false),
                    NextTermBeginsDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardNextTermBegins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardNextTermBegins_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardNextTermBegins_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardNextTermBegins_SchoolUsers_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportCardSignature",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    UploadedById = table.Column<Guid>(nullable: false),
                    SignatureUrl = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardSignature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardSignature_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardSignature_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardSignature_SchoolUsers_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportCardComments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    AdmissionNumber = table.Column<string>(nullable: true),
                    CommentConfigId = table.Column<long>(nullable: false),
                    UploadedById = table.Column<Guid>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardComments_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardComments_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardComments_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardComments_ReportCardCommentConfig_CommentConfigId",
                        column: x => x.CommentConfigId,
                        principalTable: "ReportCardCommentConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardComments_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardComments_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardComments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardComments_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardComments_SchoolUsers_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ReportCardCommentConfig",
                columns: new[] { "Id", "CommentBy" },
                values: new object[] { 1L, "Examiner" });

            migrationBuilder.InsertData(
                table: "ReportCardCommentConfig",
                columns: new[] { "Id", "CommentBy" },
                values: new object[] { 2L, "Class Teacher" });

            migrationBuilder.InsertData(
                table: "ReportCardCommentConfig",
                columns: new[] { "Id", "CommentBy" },
                values: new object[] { 3L, "Head Teacher" });

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardComments_CampusId",
                table: "ReportCardComments",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardComments_ClassGradeId",
                table: "ReportCardComments",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardComments_ClassId",
                table: "ReportCardComments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardComments_CommentConfigId",
                table: "ReportCardComments",
                column: "CommentConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardComments_SchoolId",
                table: "ReportCardComments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardComments_SessionId",
                table: "ReportCardComments",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardComments_StudentId",
                table: "ReportCardComments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardComments_TermId",
                table: "ReportCardComments",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardComments_UploadedById",
                table: "ReportCardComments",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardCommentsList_CampusId",
                table: "ReportCardCommentsList",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardCommentsList_SchoolId",
                table: "ReportCardCommentsList",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardCommentsList_UploadedById",
                table: "ReportCardCommentsList",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardNextTermBegins_CampusId",
                table: "ReportCardNextTermBegins",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardNextTermBegins_SchoolId",
                table: "ReportCardNextTermBegins",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardNextTermBegins_UploadedById",
                table: "ReportCardNextTermBegins",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardSignature_CampusId",
                table: "ReportCardSignature",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardSignature_SchoolId",
                table: "ReportCardSignature",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardSignature_UploadedById",
                table: "ReportCardSignature",
                column: "UploadedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportCardComments");

            migrationBuilder.DropTable(
                name: "ReportCardCommentsList");

            migrationBuilder.DropTable(
                name: "ReportCardNextTermBegins");

            migrationBuilder.DropTable(
                name: "ReportCardSignature");

            migrationBuilder.DropTable(
                name: "ReportCardCommentConfig");
        }
    }
}
