using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class completedVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseEnrolleeCompletedVideos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LearnerId = table.Column<Guid>(nullable: false),
                    CourseEnrolleeId = table.Column<long>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    CourseTopicId = table.Column<long>(nullable: false),
                    CourseTopicVideoId = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEnrolleeCompletedVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseEnrolleeCompletedVideos_CourseEnrollees_CourseEnrollee~",
                        column: x => x.CourseEnrolleeId,
                        principalTable: "CourseEnrollees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrolleeCompletedVideos_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrolleeCompletedVideos_CourseTopics_CourseTopicId",
                        column: x => x.CourseTopicId,
                        principalTable: "CourseTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrolleeCompletedVideos_CourseTopicVideos_CourseTopicV~",
                        column: x => x.CourseTopicVideoId,
                        principalTable: "CourseTopicVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrolleeCompletedVideos_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrolleeCompletedVideos_CourseEnrolleeId",
                table: "CourseEnrolleeCompletedVideos",
                column: "CourseEnrolleeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrolleeCompletedVideos_CourseId",
                table: "CourseEnrolleeCompletedVideos",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrolleeCompletedVideos_CourseTopicId",
                table: "CourseEnrolleeCompletedVideos",
                column: "CourseTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrolleeCompletedVideos_CourseTopicVideoId",
                table: "CourseEnrolleeCompletedVideos",
                column: "CourseTopicVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrolleeCompletedVideos_LearnerId",
                table: "CourseEnrolleeCompletedVideos",
                column: "LearnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseEnrolleeCompletedVideos");
        }
    }
}
