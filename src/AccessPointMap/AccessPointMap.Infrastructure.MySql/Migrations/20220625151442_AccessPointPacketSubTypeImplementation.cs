using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessPointMap.Infrastructure.MySql.Migrations
{
    public partial class AccessPointPacketSubTypeImplementation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subtype_Value",
                schema: "apm",
                table: "AccessPointPacket",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subtype_Value",
                schema: "apm",
                table: "AccessPointPacket");
        }
    }
}
