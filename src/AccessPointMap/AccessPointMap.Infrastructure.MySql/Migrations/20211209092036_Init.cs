using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessPointMap.Infrastructure.MySql.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "apm");

            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessPoints",
                schema: "apm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Bssid_Value = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Manufacturer_Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ssid_Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Frequency_Value = table.Column<double>(type: "double", nullable: true),
                    DeviceType_Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContributorId_Value = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreationTimestamp_Value = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    VersionTimestamp_Value = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Positioning_LowSignalLevel = table.Column<int>(type: "int", nullable: true),
                    Positioning_LowSignalLatitude = table.Column<double>(type: "double", nullable: true),
                    Positioning_LowSignalLongitude = table.Column<double>(type: "double", nullable: true),
                    Positioning_HighSignalLevel = table.Column<int>(type: "int", nullable: true),
                    Positioning_HighSignalLatitude = table.Column<double>(type: "double", nullable: true),
                    Positioning_HighSignalLongitude = table.Column<double>(type: "double", nullable: true),
                    Positioning_SignalRadius = table.Column<double>(type: "double", nullable: true),
                    Positioning_SignalArea = table.Column<double>(type: "double", nullable: true),
                    Security_RawSecurityPayload = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Security_SerializedSecurityPayload = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Security_IsSecure = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Note_Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayStatus_Value = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPoints", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Identities",
                schema: "apm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name_Value = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email_Value = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash_Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastLogin_IpAddress = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastLogin_Date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Role_Value = table.Column<int>(type: "int", nullable: true),
                    Activation_Value = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identities", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AccessPointStamp",
                schema: "apm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Ssid_Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Frequency_Value = table.Column<double>(type: "double", nullable: true),
                    DeviceType_Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContributorId_Value = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreationTimestamp_Value = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Positioning_LowSignalLevel = table.Column<int>(type: "int", nullable: true),
                    Positioning_LowSignalLatitude = table.Column<double>(type: "double", nullable: true),
                    Positioning_LowSignalLongitude = table.Column<double>(type: "double", nullable: true),
                    Positioning_HighSignalLevel = table.Column<int>(type: "int", nullable: true),
                    Positioning_HighSignalLatitude = table.Column<double>(type: "double", nullable: true),
                    Positioning_HighSignalLongitude = table.Column<double>(type: "double", nullable: true),
                    Positioning_SignalRadius = table.Column<double>(type: "double", nullable: true),
                    Positioning_SignalArea = table.Column<double>(type: "double", nullable: true),
                    Security_RawSecurityPayload = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Security_SerializedSecurityPayload = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Security_IsSecure = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Status_Value = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    accesspointId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Token",
                schema: "apm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TokenHash = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Expires = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedByIpAddress = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Revoked = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RevokedByIpAddress = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsRevoked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ReplacedByTokenHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    identityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
