using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class AcademicSessionMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchoolSuperAdmin");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "SchoolInformation");

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SessionName = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TermName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AcademicSessions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsCurrent = table.Column<bool>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicSessions_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicSessions_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicSessions_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicSessions_SchoolUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "SchoolRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RoleName",
                value: "Super Administrator");

            migrationBuilder.UpdateData(
                table: "SchoolRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RoleName",
                value: "Administrator");

            migrationBuilder.UpdateData(
                table: "SchoolRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RoleName",
                value: "Class Teacher");

            migrationBuilder.InsertData(
                table: "SchoolRoles",
                columns: new[] { "Id", "RoleName" },
                values: new object[] { 4L, "Subject Teacher" });

            migrationBuilder.InsertData(
                table: "Terms",
                columns: new[] { "Id", "TermName" },
                values: new object[,]
                {
                    { 1L, "1st Term" },
                    { 2L, "2nd Term" },
                    { 3L, "3rd Term" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessions_SchoolId",
                table: "AcademicSessions",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessions_SessionId",
                table: "AcademicSessions",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessions_TermId",
                table: "AcademicSessions",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessions_UserId",
                table: "AcademicSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SchoolId",
                table: "Sessions",
                column: "SchoolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicSessions");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Terms");

            migrationBuilder.DeleteData(
                table: "SchoolRoles",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "SchoolInformation",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SchoolSuperAdmin",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolSuperAdmin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolSuperAdmin_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "SchoolRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RoleName",
                value: "Administrator");

            migrationBuilder.UpdateData(
                table: "SchoolRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RoleName",
                value: "Class Teacher");

            migrationBuilder.UpdateData(
                table: "SchoolRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RoleName",
                value: "Subject Teacher");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSuperAdmin_SchoolId",
                table: "SchoolSuperAdmin",
                column: "SchoolId");
        }
    }
}
