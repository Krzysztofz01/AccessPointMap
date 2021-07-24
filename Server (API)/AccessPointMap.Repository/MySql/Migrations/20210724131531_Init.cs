using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessPointMap.Repository.MySql.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AddDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EditDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DeleteDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    LastLoginIp = table.Column<string>(nullable: false, defaultValue: ""),
                    LastLoginDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    AdminPermission = table.Column<bool>(nullable: false, defaultValue: false),
                    ModPermission = table.Column<bool>(nullable: false, defaultValue: false),
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
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AddDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EditDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DeleteDate = table.Column<DateTime>(nullable: true),
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
                    IsSecure = table.Column<bool>(nullable: false, defaultValue: false),
                    IsHidden = table.Column<bool>(nullable: false, defaultValue: false),
                    Manufacturer = table.Column<string>(nullable: true),
                    DeviceType = table.Column<string>(nullable: true),
                    MasterGroup = table.Column<bool>(nullable: false, defaultValue: false),
                    Display = table.Column<bool>(nullable: false, defaultValue: false),
                    Note = table.Column<string>(nullable: true, defaultValue: ""),
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

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AddDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EditDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DeleteDate = table.Column<DateTime>(nullable: true),
                    Token = table.Column<string>(nullable: false),
                    Expires = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
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
                name: "IX_AccessPoints_UserAddedId",
                table: "AccessPoints",
                column: "UserAddedId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_UserModifiedId",
                table: "AccessPoints",
                column: "UserModifiedId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessPoints");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
