using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ReportCardConfig31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "ReportCardData",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardData_DepartmentId",
                table: "ReportCardData",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportCardData_SubjectDepartment_DepartmentId",
                table: "ReportCardData",
                column: "DepartmentId",
                principalTable: "SubjectDepartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportCardData_SubjectDepartment_DepartmentId",
                table: "ReportCardData");

            migrationBuilder.DropIndex(
                name: "IX_ReportCardData_DepartmentId",
                table: "ReportCardData");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ReportCardData");
        }
    }
}
