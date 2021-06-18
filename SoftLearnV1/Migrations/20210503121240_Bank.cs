using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class Bank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BankName = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseTopicVideoMaterials",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacilitatorId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    CourseTopicId = table.Column<long>(nullable: false),
                    CourseTopicVideoId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    NoOfPages = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    IsAvailable = table.Column<bool>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTopicVideoMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTopicVideoMaterials_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopicVideoMaterials_CourseTopics_CourseTopicId",
                        column: x => x.CourseTopicId,
                        principalTable: "CourseTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopicVideoMaterials_CourseTopicVideos_CourseTopicVideo~",
                        column: x => x.CourseTopicVideoId,
                        principalTable: "CourseTopicVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopicVideoMaterials_Facilitators_FacilitatorId",
                        column: x => x.FacilitatorId,
                        principalTable: "Facilitators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicVideoMaterials_CourseId",
                table: "CourseTopicVideoMaterials",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicVideoMaterials_CourseTopicId",
                table: "CourseTopicVideoMaterials",
                column: "CourseTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicVideoMaterials_CourseTopicVideoId",
                table: "CourseTopicVideoMaterials",
                column: "CourseTopicVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicVideoMaterials_FacilitatorId",
                table: "CourseTopicVideoMaterials",
                column: "FacilitatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "CourseTopicVideoMaterials");
        }
    }
}
