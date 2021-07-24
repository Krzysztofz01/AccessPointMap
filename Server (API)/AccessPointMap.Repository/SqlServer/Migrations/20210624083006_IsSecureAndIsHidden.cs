using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessPointMap.Repository.SqlServer.Migrations
{
    public partial class IsSecureAndIsHidden : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "AccessPoints",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSecure",
                table: "AccessPoints",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "AccessPoints");

            migrationBuilder.DropColumn(
                name: "IsSecure",
                table: "AccessPoints");
        }
    }
}
