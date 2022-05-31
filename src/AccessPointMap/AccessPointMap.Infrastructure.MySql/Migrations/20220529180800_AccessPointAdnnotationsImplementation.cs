using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessPointMap.Infrastructure.MySql.Migrations
{
    public partial class AccessPointAdnnotationsImplementation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessPointAdnnotation",
                schema: "apm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title_Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content_Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Timestamp_Value = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    accesspointId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPointAdnnotation_accesspointId",
                schema: "apm",
                table: "AccessPointAdnnotation",
                column: "accesspointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessPointAdnnotation",
                schema: "apm");
        }
    }
}
