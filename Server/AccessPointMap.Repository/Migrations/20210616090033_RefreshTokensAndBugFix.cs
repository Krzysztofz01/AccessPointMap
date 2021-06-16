using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessPointMap.Repository.Migrations
{
    public partial class RefreshTokensAndBugFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadPermission",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WritePermission",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "ModPermission",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "AccessPoints",
                nullable: true,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    EditDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(nullable: false),
                    Token = table.Column<string>(nullable: false),
                    Expires = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    CreatedByIp = table.Column<string>(nullable: false, defaultValue: ""),
                    Revoked = table.Column<DateTime>(nullable: true),
                    RevokedByIp = table.Column<string>(nullable: false, defaultValue: ""),
                    IsRevoked = table.Column<bool>(nullable: false, defaultValue: false),
                    ReplacedByToken = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "ModPermission",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "AccessPoints");

            migrationBuilder.AddColumn<bool>(
                name: "ReadPermission",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WritePermission",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
