using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessPointMap.Repository.MySql.Migrations
{
    public partial class DeleteDateFieldFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "RefreshTokens",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "AccessPoints",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "AccessPoints");
        }
    }
}
