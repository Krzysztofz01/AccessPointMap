using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessPointMap.Infrastructure.MySql.Migrations
{
    public partial class AccessPointPresenceImplementation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Presence_Value",
                schema: "apm",
                table: "AccessPoints",
                type: "tinyint(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Presence_Value",
                schema: "apm",
                table: "AccessPoints");
        }
    }
}
