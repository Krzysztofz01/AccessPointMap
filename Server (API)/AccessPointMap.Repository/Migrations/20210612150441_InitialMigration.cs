using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessPointMap.Repository.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    EditDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    LastLoginIp = table.Column<string>(nullable: false, defaultValue: ""),
                    LastLoginDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    AdminPermission = table.Column<bool>(nullable: false, defaultValue: false),
                    WritePermission = table.Column<bool>(nullable: false, defaultValue: false),
                    ReadPermission = table.Column<bool>(nullable: false, defaultValue: false),
                    IsActivated = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessPoints",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    EditDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    DeleteDate = table.Column<DateTime>(nullable: false),
                    Bssid = table.Column<string>(nullable: false),
                    Ssid = table.Column<string>(nullable: false),
                    Fingerprint = table.Column<string>(nullable: false),
                    Frequency = table.Column<double>(nullable: false),
                    MaxSignalLevel = table.Column<int>(nullable: false),
                    MaxSignalLongitude = table.Column<double>(nullable: false),
                    MaxSignalLatitude = table.Column<double>(nullable: false),
                    MinSignalLevel = table.Column<int>(nullable: false),
                    MinSignalLongitude = table.Column<double>(nullable: false),
                    MinSignalLatitude = table.Column<double>(nullable: false),
                    SignalRadius = table.Column<double>(nullable: false),
                    SignalArea = table.Column<double>(nullable: false),
                    FullSecurityData = table.Column<string>(nullable: false),
                    SerializedSecurityData = table.Column<string>(nullable: false),
                    Manufacturer = table.Column<string>(nullable: true),
                    DeviceType = table.Column<string>(nullable: true),
                    MasterGroup = table.Column<bool>(nullable: false, defaultValue: false),
                    Display = table.Column<bool>(nullable: false, defaultValue: false),
                    UserAddedId = table.Column<long>(nullable: true),
                    UserModifiedId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessPoints_Users_UserAddedId",
                        column: x => x.UserAddedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccessPoints_Users_UserModifiedId",
                        column: x => x.UserModifiedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_UserAddedId",
                table: "AccessPoints",
                column: "UserAddedId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_UserModifiedId",
                table: "AccessPoints",
                column: "UserModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessPoints");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
