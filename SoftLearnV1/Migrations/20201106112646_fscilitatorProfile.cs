using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class fscilitatorProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacilitatorOtherInfo");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Facilitators",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificateObtained",
                table: "Facilitators",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseOfStudy",
                table: "Facilitators",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstitutionAttended",
                table: "Facilitators",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Profession",
                table: "Facilitators",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "Facilitators",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "CourseTopicVideos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoOfPages",
                table: "CourseTopicMaterials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseSubTitle",
                table: "Courses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Facilitators");

            migrationBuilder.DropColumn(
                name: "CertificateObtained",
                table: "Facilitators");

            migrationBuilder.DropColumn(
                name: "CourseOfStudy",
                table: "Facilitators");

            migrationBuilder.DropColumn(
                name: "InstitutionAttended",
                table: "Facilitators");

            migrationBuilder.DropColumn(
                name: "Profession",
                table: "Facilitators");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "Facilitators");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "CourseTopicVideos");

            migrationBuilder.DropColumn(
                name: "NoOfPages",
                table: "CourseTopicMaterials");

            migrationBuilder.DropColumn(
                name: "CourseSubTitle",
                table: "Courses");

            migrationBuilder.CreateTable(
                name: "FacilitatorOtherInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Bio = table.Column<string>(nullable: true),
                    CertificateObtained = table.Column<string>(nullable: true),
                    CourseOfStudy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    FacilitatorId = table.Column<Guid>(nullable: false),
                    InstitutionAttended = table.Column<string>(nullable: true),
                    Profession = table.Column<string>(nullable: true),
                    ProfilePictureUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitatorOtherInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilitatorOtherInfo_Facilitators_FacilitatorId",
                        column: x => x.FacilitatorId,
                        principalTable: "Facilitators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilitatorOtherInfo_FacilitatorId",
                table: "FacilitatorOtherInfo",
                column: "FacilitatorId");
        }
    }
}
