using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessPointMap.Infrastructure.Sqlite.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Bssid_Value = table.Column<string>(type: "TEXT", nullable: true),
                    Manufacturer_Value = table.Column<string>(type: "TEXT", nullable: true),
                    Ssid_Value = table.Column<string>(type: "TEXT", nullable: true),
                    Frequency_Value = table.Column<double>(type: "REAL", nullable: false),
                    DeviceType_Value = table.Column<string>(type: "TEXT", nullable: true),
                    ContributorId_Value = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreationTimestamp_Value = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VersionTimestamp_Value = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Positioning_LowSignalLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    Positioning_LowSignalLatitude = table.Column<double>(type: "REAL", nullable: false),
                    Positioning_LowSignalLongitude = table.Column<double>(type: "REAL", nullable: false),
                    Positioning_HighSignalLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    Positioning_HighSignalLatitude = table.Column<double>(type: "REAL", nullable: false),
                    Positioning_HighSignalLongitude = table.Column<double>(type: "REAL", nullable: false),
                    Positioning_SignalRadius = table.Column<double>(type: "REAL", nullable: false),
                    Positioning_SignalArea = table.Column<double>(type: "REAL", nullable: false),
                    Security_RawSecurityPayload = table.Column<string>(type: "TEXT", nullable: true),
                    Security_SecurityStandards = table.Column<string>(type: "TEXT", nullable: true),
                    Security_SecurityProtocols = table.Column<string>(type: "TEXT", nullable: true),
                    Security_IsSecure = table.Column<bool>(type: "INTEGER", nullable: false),
                    Note_Value = table.Column<string>(type: "TEXT", nullable: true),
                    RunIdentifier_Value = table.Column<Guid>(type: "TEXT", nullable: true),
                    DisplayStatus_Value = table.Column<bool>(type: "INTEGER", nullable: false),
                    Presence_Value = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name_Value = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    Email_Value = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash_Value = table.Column<string>(type: "TEXT", nullable: true),
                    LastLogin_IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    LastLogin_Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Role_Value = table.Column<int>(type: "INTEGER", nullable: false),
                    Activation_Value = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessPointAdnnotation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title_Value = table.Column<string>(type: "TEXT", nullable: true),
                    Content_Value = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp_Value = table.Column<DateTime>(type: "TEXT", nullable: false),
                    accesspointId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPointAdnnotation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessPointAdnnotation_AccessPoints_accesspointId",
                        column: x => x.accesspointId,
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessPointPacket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DestinationAddress_Value = table.Column<string>(type: "TEXT", nullable: true),
                    FrameType_Value = table.Column<string>(type: "TEXT", nullable: true),
                    Data_Value = table.Column<string>(type: "TEXT", nullable: true),
                    accesspointId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPointPacket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessPointPacket_AccessPoints_accesspointId",
                        column: x => x.accesspointId,
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessPointStamp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Ssid_Value = table.Column<string>(type: "TEXT", nullable: true),
                    Frequency_Value = table.Column<double>(type: "REAL", nullable: false),
                    DeviceType_Value = table.Column<string>(type: "TEXT", nullable: true),
                    ContributorId_Value = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreationTimestamp_Value = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Positioning_LowSignalLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    Positioning_LowSignalLatitude = table.Column<double>(type: "REAL", nullable: false),
                    Positioning_LowSignalLongitude = table.Column<double>(type: "REAL", nullable: false),
                    Positioning_HighSignalLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    Positioning_HighSignalLatitude = table.Column<double>(type: "REAL", nullable: false),
                    Positioning_HighSignalLongitude = table.Column<double>(type: "REAL", nullable: false),
                    Positioning_SignalRadius = table.Column<double>(type: "REAL", nullable: false),
                    Positioning_SignalArea = table.Column<double>(type: "REAL", nullable: false),
                    Security_RawSecurityPayload = table.Column<string>(type: "TEXT", nullable: true),
                    Security_SecurityStandards = table.Column<string>(type: "TEXT", nullable: true),
                    Security_SecurityProtocols = table.Column<string>(type: "TEXT", nullable: true),
                    Security_IsSecure = table.Column<bool>(type: "INTEGER", nullable: false),
                    RunIdentifier_Value = table.Column<Guid>(type: "TEXT", nullable: true),
                    Status_Value = table.Column<bool>(type: "INTEGER", nullable: false),
                    accesspointId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPointStamp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessPointStamp_AccessPoints_accesspointId",
                        column: x => x.accesspointId,
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TokenHash = table.Column<string>(type: "TEXT", nullable: true),
                    Expires = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedByIpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    Revoked = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RevokedByIpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    IsRevoked = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReplacedByTokenHash = table.Column<string>(type: "TEXT", nullable: true),
                    identityId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Token_Identities_identityId",
                        column: x => x.identityId,
                        principalTable: "Identities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessPointAdnnotation_accesspointId",
                table: "AccessPointAdnnotation",
                column: "accesspointId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPointPacket_accesspointId",
                table: "AccessPointPacket",
                column: "accesspointId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_Bssid_Value",
                table: "AccessPoints",
                column: "Bssid_Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessPointStamp_accesspointId",
                table: "AccessPointStamp",
                column: "accesspointId");

            migrationBuilder.CreateIndex(
                name: "IX_Identities_Email_Value",
                table: "Identities",
                column: "Email_Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Token_identityId",
                table: "Token",
                column: "identityId");

            migrationBuilder.CreateIndex(
                name: "IX_Token_TokenHash",
                table: "Token",
                column: "TokenHash",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessPointAdnnotation");

            migrationBuilder.DropTable(
                name: "AccessPointPacket");

            migrationBuilder.DropTable(
                name: "AccessPointStamp");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropTable(
                name: "AccessPoints");

            migrationBuilder.DropTable(
                name: "Identities");
        }
    }
}
