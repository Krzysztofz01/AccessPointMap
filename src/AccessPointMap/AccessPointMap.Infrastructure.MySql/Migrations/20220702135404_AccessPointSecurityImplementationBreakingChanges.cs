using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessPointMap.Infrastructure.MySql.Migrations
{
    public partial class AccessPointSecurityImplementationBreakingChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Security_SerializedSecurityPayload",
                schema: "apm",
                table: "AccessPointStamp",
                newName: "Security_SecurityStandards");

            migrationBuilder.RenameColumn(
                name: "Security_SerializedSecurityPayload",
                schema: "apm",
                table: "AccessPoints",
                newName: "Security_SecurityStandards");

            migrationBuilder.AddColumn<string>(
                name: "Security_SecurityProtocols",
                schema: "apm",
                table: "AccessPointStamp",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Security_SecurityProtocols",
                schema: "apm",
                table: "AccessPoints",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Security_SecurityProtocols",
                schema: "apm",
                table: "AccessPointStamp");

            migrationBuilder.DropColumn(
                name: "Security_SecurityProtocols",
                schema: "apm",
                table: "AccessPoints");

            migrationBuilder.RenameColumn(
                name: "Security_SecurityStandards",
                schema: "apm",
                table: "AccessPointStamp",
                newName: "Security_SerializedSecurityPayload");

            migrationBuilder.RenameColumn(
                name: "Security_SecurityStandards",
                schema: "apm",
                table: "AccessPoints",
                newName: "Security_SerializedSecurityPayload");
        }
    }
}
