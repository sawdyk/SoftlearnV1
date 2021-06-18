using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class courseRefundMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    Platform = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    ActivityDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseRefund",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LearnerId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    ReasonForReturn = table.Column<string>(nullable: true),
                    ReviewStatus = table.Column<bool>(nullable: false),
                    IsSettled = table.Column<bool>(nullable: false),
                    DateReturned = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseRefund", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseRefund_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseRefund_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseRefund_CourseId",
                table: "CourseRefund",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRefund_LearnerId",
                table: "CourseRefund",
                column: "LearnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLog");

            migrationBuilder.DropTable(
                name: "CourseRefund");
        }
    }
}
