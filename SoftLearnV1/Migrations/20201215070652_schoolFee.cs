using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class schoolFee : Migration
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

            migrationBuilder.DropColumn(
                name: "FeeAmount",
                table: "FeeSubCategory");

            migrationBuilder.DropColumn(
                name: "IsMandatory",
                table: "FeeSubCategory");

            migrationBuilder.AlterColumn<long>(
                name: "TermId",
                table: "FeeTemplate",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "SessionId",
                table: "FeeTemplate",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "ClassId",
                table: "FeeTemplate",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.CreateTable(
                name: "SchoolFees",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    FeeSubCategoryId = table.Column<long>(nullable: false),
                    IsMandatory = table.Column<bool>(nullable: false),
                    TemplateId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolFees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolFees_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFees_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFees_FeeSubCategory_FeeSubCategoryId",
                        column: x => x.FeeSubCategoryId,
                        principalTable: "FeeSubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFees_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFees_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFees_FeeTemplate_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "FeeTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFees_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFees_CampusId",
                table: "SchoolFees",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFees_ClassId",
                table: "SchoolFees",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFees_FeeSubCategoryId",
                table: "SchoolFees",
                column: "FeeSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFees_SchoolId",
                table: "SchoolFees",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFees_SessionId",
                table: "SchoolFees",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFees_TemplateId",
                table: "SchoolFees",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFees_TermId",
                table: "SchoolFees",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "SchoolFees");

            migrationBuilder.AlterColumn<long>(
                name: "TermId",
                table: "FeeTemplate",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SessionId",
                table: "FeeTemplate",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ClassId",
                table: "FeeTemplate",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FeeAmount",
                table: "FeeSubCategory",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsMandatory",
                table: "FeeSubCategory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeTemplate_Classes_ClassId",
                table: "FeeTemplate",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeTemplate_Sessions_SessionId",
                table: "FeeTemplate",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeTemplate_Terms_TermId",
                table: "FeeTemplate",
                column: "TermId",
                principalTable: "Terms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
