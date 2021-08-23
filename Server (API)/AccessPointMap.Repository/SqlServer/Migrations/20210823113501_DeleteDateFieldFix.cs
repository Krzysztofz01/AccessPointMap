using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessPointMap.Repository.SqlServer.Migrations
{
    public partial class DeleteDateFieldFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "AccessPoints",
                type: "datetime2",
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
