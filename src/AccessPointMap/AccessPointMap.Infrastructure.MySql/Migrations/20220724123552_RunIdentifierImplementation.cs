using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessPointMap.Infrastructure.MySql.Migrations
{
    public partial class RunIdentifierImplementation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RunIdentifier_Value",
                schema: "apm",
                table: "AccessPointStamp",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "RunIdentifier_Value",
                schema: "apm",
                table: "AccessPoints",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RunIdentifier_Value",
                schema: "apm",
                table: "AccessPointStamp");

            migrationBuilder.DropColumn(
                name: "RunIdentifier_Value",
                schema: "apm",
                table: "AccessPoints");
        }
    }
}
