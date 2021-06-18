using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class courseCertificateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCompleted",
                table: "CourseEnrollees",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageCompletion",
                table: "CourseEnrollees",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LearnerId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    CertificateNumber = table.Column<string>(nullable: true),
                    DateGenerated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Certificates_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ReportCardCommentConfig",
                columns: new[] { "Id", "CommentBy" },
                values: new object[] { 4L, "Principal" });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_CourseId",
                table: "Certificates",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_LearnerId",
                table: "Certificates",
                column: "LearnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DeleteData(
                table: "ReportCardCommentConfig",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DropColumn(
                name: "DateCompleted",
                table: "CourseEnrollees");

            migrationBuilder.DropColumn(
                name: "PercentageCompletion",
                table: "CourseEnrollees");
        }
    }
}
