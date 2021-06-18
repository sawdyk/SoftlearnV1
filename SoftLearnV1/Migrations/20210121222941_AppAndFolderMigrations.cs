using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class AppAndFolderMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FolderTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppId = table.Column<long>(nullable: false),
                    FolderName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FolderTypes_AppTypes_AppId",
                        column: x => x.AppId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppTypes",
                columns: new[] { "Id", "AppName" },
                values: new object[,]
                {
                    { 1L, "CourseApp" },
                    { 2L, "SchoolApp" }
                });

            migrationBuilder.InsertData(
                table: "SchoolRoles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 5L, "Finance Officer" },
                    { 6L, "Principal" },
                    { 7L, "Vice Principal" }
                });

            migrationBuilder.InsertData(
                table: "FolderTypes",
                columns: new[] { "Id", "AppId", "FolderName" },
                values: new object[,]
                {
                    { 1L, 1L, "CourseCategoryImages" },
                    { 2L, 1L, "CourseImages" },
                    { 3L, 1L, "Documents" },
                    { 4L, 1L, "ProfilePictures" },
                    { 5L, 1L, "Videos" },
                    { 6L, 2L, "Assignments" },
                    { 7L, 2L, "LessonNotes" },
                    { 8L, 2L, "SchoolLogos" },
                    { 9L, 2L, "Signatures" },
                    { 10L, 2L, "StudentPassports" },
                    { 11L, 2L, "SubjectNotes" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FolderTypes_AppId",
                table: "FolderTypes",
                column: "AppId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FolderTypes");

            migrationBuilder.DropTable(
                name: "AppTypes");

            migrationBuilder.DeleteData(
                table: "SchoolRoles",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "SchoolRoles",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "SchoolRoles",
                keyColumn: "Id",
                keyValue: 7L);
        }
    }
}
