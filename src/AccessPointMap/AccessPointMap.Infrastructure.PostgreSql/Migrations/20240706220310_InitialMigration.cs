using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessPointMap.Infrastructure.PostgreSql.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "apm");

            migrationBuilder.CreateTable(
                name: "AccessPoints",
                schema: "apm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Bssid_Value = table.Column<string>(type: "text", nullable: true),
                    Manufacturer_Value = table.Column<string>(type: "text", nullable: true),
                    Ssid_Value = table.Column<string>(type: "text", nullable: true),
                    Frequency_Value = table.Column<double>(type: "double precision", nullable: false),
                    DeviceType_Value = table.Column<string>(type: "text", nullable: true),
                    ContributorId_Value = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTimestamp_Value = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VersionTimestamp_Value = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Positioning_LowSignalLevel = table.Column<int>(type: "integer", nullable: false),
                    Positioning_LowSignalLatitude = table.Column<double>(type: "double precision", nullable: false),
                    Positioning_LowSignalLongitude = table.Column<double>(type: "double precision", nullable: false),
                    Positioning_HighSignalLevel = table.Column<int>(type: "integer", nullable: false),
                    Positioning_HighSignalLatitude = table.Column<double>(type: "double precision", nullable: false),
                    Positioning_HighSignalLongitude = table.Column<double>(type: "double precision", nullable: false),
                    Positioning_SignalRadius = table.Column<double>(type: "double precision", nullable: false),
                    Positioning_SignalArea = table.Column<double>(type: "double precision", nullable: false),
                    Security_RawSecurityPayload = table.Column<string>(type: "text", nullable: true),
                    Security_SecurityStandards = table.Column<string>(type: "text", nullable: true),
                    Security_SecurityProtocols = table.Column<string>(type: "text", nullable: true),
                    Security_IsSecure = table.Column<bool>(type: "boolean", nullable: false),
                    Note_Value = table.Column<string>(type: "text", nullable: true),
                    RunIdentifier_Value = table.Column<Guid>(type: "uuid", nullable: true),
                    DisplayStatus_Value = table.Column<bool>(type: "boolean", nullable: false),
                    Presence_Value = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Identities",
                schema: "apm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name_Value = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Email_Value = table.Column<string>(type: "text", nullable: true),
                    PasswordHash_Value = table.Column<string>(type: "text", nullable: true),
                    LastLogin_IpAddress = table.Column<string>(type: "text", nullable: true),
                    LastLogin_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Role_Value = table.Column<int>(type: "integer", nullable: false),
                    Activation_Value = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessPointAdnnotation",
                schema: "apm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title_Value = table.Column<string>(type: "text", nullable: true),
                    Content_Value = table.Column<string>(type: "text", nullable: true),
                    Timestamp_Value = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    accesspointId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPointAdnnotation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessPointAdnnotation_AccessPoints_accesspointId",
                        column: x => x.accesspointId,
                        principalSchema: "apm",
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessPointPacket",
                schema: "apm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinationAddress_Value = table.Column<string>(type: "text", nullable: true),
                    FrameType_Value = table.Column<string>(type: "text", nullable: true),
                    Data_Value = table.Column<string>(type: "text", nullable: true),
                    accesspointId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPointPacket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessPointPacket_AccessPoints_accesspointId",
                        column: x => x.accesspointId,
                        principalSchema: "apm",
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessPointStamp",
                schema: "apm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ssid_Value = table.Column<string>(type: "text", nullable: true),
                    Frequency_Value = table.Column<double>(type: "double precision", nullable: false),
                    DeviceType_Value = table.Column<string>(type: "text", nullable: true),
                    ContributorId_Value = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTimestamp_Value = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Positioning_LowSignalLevel = table.Column<int>(type: "integer", nullable: false),
                    Positioning_LowSignalLatitude = table.Column<double>(type: "double precision", nullable: false),
                    Positioning_LowSignalLongitude = table.Column<double>(type: "double precision", nullable: false),
                    Positioning_HighSignalLevel = table.Column<int>(type: "integer", nullable: false),
                    Positioning_HighSignalLatitude = table.Column<double>(type: "double precision", nullable: false),
                    Positioning_HighSignalLongitude = table.Column<double>(type: "double precision", nullable: false),
                    Positioning_SignalRadius = table.Column<double>(type: "double precision", nullable: false),
                    Positioning_SignalArea = table.Column<double>(type: "double precision", nullable: false),
                    Security_RawSecurityPayload = table.Column<string>(type: "text", nullable: true),
                    Security_SecurityStandards = table.Column<string>(type: "text", nullable: true),
                    Security_SecurityProtocols = table.Column<string>(type: "text", nullable: true),
                    Security_IsSecure = table.Column<bool>(type: "boolean", nullable: false),
                    RunIdentifier_Value = table.Column<Guid>(type: "uuid", nullable: true),
                    Status_Value = table.Column<bool>(type: "boolean", nullable: false),
                    accesspointId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPointStamp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessPointStamp_AccessPoints_accesspointId",
                        column: x => x.accesspointId,
                        principalSchema: "apm",
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                schema: "apm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: true),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByIpAddress = table.Column<string>(type: "text", nullable: true),
                    Revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevokedByIpAddress = table.Column<string>(type: "text", nullable: true),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    ReplacedByTokenHash = table.Column<string>(type: "text", nullable: true),
                    identityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Token_Identities_identityId",
                        column: x => x.identityId,
                        principalSchema: "apm",
                        principalTable: "Identities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessPointAdnnotation_accesspointId",
                schema: "apm",
                table: "AccessPointAdnnotation",
                column: "accesspointId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPointPacket_accesspointId",
                schema: "apm",
                table: "AccessPointPacket",
                column: "accesspointId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_Bssid_Value",
                schema: "apm",
                table: "AccessPoints",
                column: "Bssid_Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessPointStamp_accesspointId",
                schema: "apm",
                table: "AccessPointStamp",
                column: "accesspointId");

            migrationBuilder.CreateIndex(
                name: "IX_Identities_Email_Value",
                schema: "apm",
                table: "Identities",
                column: "Email_Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Token_identityId",
                schema: "apm",
                table: "Token",
                column: "identityId");

            migrationBuilder.CreateIndex(
                name: "IX_Token_TokenHash",
                schema: "apm",
                table: "Token",
                column: "TokenHash",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessPointAdnnotation",
                schema: "apm");

            migrationBuilder.DropTable(
                name: "AccessPointPacket",
                schema: "apm");

            migrationBuilder.DropTable(
                name: "AccessPointStamp",
                schema: "apm");

            migrationBuilder.DropTable(
                name: "Token",
                schema: "apm");

            migrationBuilder.DropTable(
                name: "AccessPoints",
                schema: "apm");

            migrationBuilder.DropTable(
                name: "Identities",
                schema: "apm");
        }
    }
}
