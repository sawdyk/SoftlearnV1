using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class newMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolUserRoles_SchoolAdmin_UserId",
                table: "SchoolUserRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolUserRoles_SchoolUsers_UserId",
                table: "SchoolUserRoles",
                column: "UserId",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolUserRoles_SchoolUsers_UserId",
                table: "SchoolUserRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolUserRoles_SchoolAdmin_UserId",
                table: "SchoolUserRoles",
                column: "UserId",
                principalTable: "SchoolAdmin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
