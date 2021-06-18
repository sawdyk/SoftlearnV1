using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class learnerMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearnersOtherInfo");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Learners",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "Learners",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Learners");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "Learners");

            migrationBuilder.CreateTable(
                name: "LearnersOtherInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Bio = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    LearnerId = table.Column<Guid>(nullable: false),
                    ProfilePictureUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnersOtherInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearnersOtherInfo_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearnersOtherInfo_LearnerId",
                table: "LearnersOtherInfo",
                column: "LearnerId");
        }
    }
}
