using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessPointMap.Repository.SqlServer.Migrations
{
    public partial class DefaultAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddDate", "AdminPermission", "DeleteDate", "EditDate", "Email", "IsActivated", "LastLoginDate", "LastLoginIp", "Name", "Password" },
                values: new object[] { 1L, new DateTime(2021, 8, 24, 14, 49, 54, 382, DateTimeKind.Local).AddTicks(6978), true, null, new DateTime(2021, 8, 24, 14, 49, 54, 390, DateTimeKind.Local).AddTicks(6328), "admin@apm.com", true, new DateTime(2021, 8, 24, 14, 49, 54, 390, DateTimeKind.Local).AddTicks(9096), "", "Administrator", "$05$feN415S/rRMOaPcaiobkEeo5JTPoxY7PPMCwVGkbrbItw/mj19CBS" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);
        }
    }
}
