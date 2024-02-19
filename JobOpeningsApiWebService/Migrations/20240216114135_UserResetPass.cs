using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobOpeningsApiWebService.Migrations
{
    /// <inheritdoc />
    public partial class UserResetPass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetCode",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResetPassword",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22dfa6bc-9368-4271-6a90-08dc18541600"),
                columns: new[] { "CreatedOn", "ResetCode", "ResetPassword" },
                values: new object[] { new DateTime(2024, 2, 16, 11, 41, 34, 642, DateTimeKind.Utc).AddTicks(5116), "", "" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cd87c8dd-df68-46cc-a9d5-08dc1852bb58"),
                columns: new[] { "CreatedOn", "ResetCode", "ResetPassword" },
                values: new object[] { new DateTime(2024, 2, 16, 11, 41, 34, 642, DateTimeKind.Utc).AddTicks(5062), "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetPassword",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22dfa6bc-9368-4271-6a90-08dc18541600"),
                column: "CreatedOn",
                value: new DateTime(2024, 2, 15, 14, 21, 33, 562, DateTimeKind.Utc).AddTicks(396));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cd87c8dd-df68-46cc-a9d5-08dc1852bb58"),
                column: "CreatedOn",
                value: new DateTime(2024, 2, 15, 14, 21, 33, 562, DateTimeKind.Utc).AddTicks(345));
        }
    }
}
