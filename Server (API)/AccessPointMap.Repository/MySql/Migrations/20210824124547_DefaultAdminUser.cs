using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccessPointMap.Repository.MySql.Migrations
{
    public partial class DefaultAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddDate", "AdminPermission", "DeleteDate", "EditDate", "Email", "IsActivated", "LastLoginDate", "LastLoginIp", "Name", "Password" },
                values: new object[] { 1L, new DateTime(2021, 8, 24, 14, 45, 46, 977, DateTimeKind.Local).AddTicks(9616), true, null, new DateTime(2021, 8, 24, 14, 45, 46, 981, DateTimeKind.Local).AddTicks(9252), "admin@apm.com", true, new DateTime(2021, 8, 24, 14, 45, 46, 982, DateTimeKind.Local).AddTicks(1564), "", "Administrator", "$05$feN415S/rRMOaPcaiobkEeo5JTPoxY7PPMCwVGkbrbItw/mj19CBS" });
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
