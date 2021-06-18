using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class schoolFee2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeTemplate_Classes_ClassId",
                table: "FeeTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_FeeTemplate_Sessions_SessionId",
                table: "FeeTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_FeeTemplate_Terms_TermId",
                table: "FeeTemplate");

            migrationBuilder.DropIndex(
                name: "IX_FeeTemplate_ClassId",
                table: "FeeTemplate");

            migrationBuilder.DropIndex(
                name: "IX_FeeTemplate_SessionId",
                table: "FeeTemplate");

            migrationBuilder.DropIndex(
                name: "IX_FeeTemplate_TermId",
                table: "FeeTemplate");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "FeeTemplate");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "FeeTemplate");

            migrationBuilder.DropColumn(
                name: "TermId",
                table: "FeeTemplate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ClassId",
                table: "FeeTemplate",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SessionId",
                table: "FeeTemplate",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TermId",
                table: "FeeTemplate",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplate_ClassId",
                table: "FeeTemplate",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplate_SessionId",
                table: "FeeTemplate",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplate_TermId",
                table: "FeeTemplate",
                column: "TermId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeeTemplate_Classes_ClassId",
                table: "FeeTemplate",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeTemplate_Sessions_SessionId",
                table: "FeeTemplate",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeTemplate_Terms_TermId",
                table: "FeeTemplate",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
