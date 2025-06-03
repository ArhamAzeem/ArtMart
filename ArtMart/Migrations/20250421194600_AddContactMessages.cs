using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArtMart.Migrations
{
    /// <inheritdoc />
    public partial class AddContactMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0cd1f5d7-de86-49a7-b369-6c1225fd1f35");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0de041c0-6fc2-4262-b263-c2f2c28fde2a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6df74aa8-7f30-47af-8913-b513e1578594");

            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9ccb36cc-075d-46bc-a97c-3c068d681172", null, "Customer", "CUSTOMER" },
                    { "ebf952c1-5173-469e-9000-de5c1f9e8913", null, "Admin", "ADMIN" },
                    { "f6aa943b-7496-41d7-b08a-3ba508e2f471", null, "Artist", "ARTIST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9ccb36cc-075d-46bc-a97c-3c068d681172");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ebf952c1-5173-469e-9000-de5c1f9e8913");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6aa943b-7496-41d7-b08a-3ba508e2f471");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0cd1f5d7-de86-49a7-b369-6c1225fd1f35", null, "Artist", "ARTIST" },
                    { "0de041c0-6fc2-4262-b263-c2f2c28fde2a", null, "Customer", "CUSTOMER" },
                    { "6df74aa8-7f30-47af-8913-b513e1578594", null, "Admin", "ADMIN" }
                });
        }
    }
}
