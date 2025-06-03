using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArtMart.Migrations
{
    /// <inheritdoc />
    public partial class CreateNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43ede431-dc96-466a-94c3-3a5526074ae2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "55d48390-8c7c-4ad0-aed4-fd1a85e7c2c9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9c105b0-80eb-4e02-a37e-8d5398b6abd1");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1fb78cb1-c375-4345-86bf-7c7cc0e7ae88", null, "Admin", "ADMIN" },
                    { "93536539-c8f8-4cb5-91bf-f55240fc45f0", null, "Customer", "CUSTOMER" },
                    { "a3f3c192-9bd3-4974-b923-a2582f7e06dc", null, "Artist", "ARTIST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fb78cb1-c375-4345-86bf-7c7cc0e7ae88");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93536539-c8f8-4cb5-91bf-f55240fc45f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3f3c192-9bd3-4974-b923-a2582f7e06dc");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "43ede431-dc96-466a-94c3-3a5526074ae2", null, "Admin", "ADMIN" },
                    { "55d48390-8c7c-4ad0-aed4-fd1a85e7c2c9", null, "Customer", "CUSTOMER" },
                    { "d9c105b0-80eb-4e02-a37e-8d5398b6abd1", null, "Artist", "ARTIST" }
                });
        }
    }
}
