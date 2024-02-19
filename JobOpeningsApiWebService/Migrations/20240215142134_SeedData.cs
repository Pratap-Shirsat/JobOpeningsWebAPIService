using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobOpeningsApiWebService.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedOn", "Email", "IsDeleted", "Name", "Password", "Phone", "UserType", "Username" },
                values: new object[,]
                {
                    { new Guid("22dfa6bc-9368-4271-6a90-08dc18541600"), new DateTime(2024, 2, 15, 14, 21, 33, 562, DateTimeKind.Utc).AddTicks(396), "jimin@admin.com", false, "Jimin Park", "$2a$10$c2gltoVOEhSIeYtE4ZkctOKSa57ye2i69p1akHwk87Kh8DUHRapM.", "9999999999", 1, "jimin" },
                    { new Guid("cd87c8dd-df68-46cc-a9d5-08dc1852bb58"), new DateTime(2024, 2, 15, 14, 21, 33, 562, DateTimeKind.Utc).AddTicks(345), "pratap@admin.com", false, "Pratap Shirsat", "$2a$10$VxE26TrbGlxAiapR4dBTq.KCZ0L1r1M032MbWVZ2gdHU9RwQpqMwu", "9999999999", 1, "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22dfa6bc-9368-4271-6a90-08dc18541600"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cd87c8dd-df68-46cc-a9d5-08dc1852bb58"));
        }
    }
}
