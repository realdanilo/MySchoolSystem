using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchoolSystem.Migrations
{
    public partial class DefaultRolesTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb3356dc-02e0-4942-ba6c-9482948384f4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "92c9835e-2481-4ca9-acf7-d6fcc45b1f66", "e4f2834d-f642-4770-b42b-74f03763c16d", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5e13e4b5-9c74-4fc8-a62a-f4d557846578", "1a7dd394-fd18-4fc8-8597-d6a652888225", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f1c276a1-3945-455b-9a50-de572b2e03aa", "72a1a040-e509-4270-8453-b0d4a210b901", "Instructor", "INSTRUCTOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e13e4b5-9c74-4fc8-a62a-f4d557846578");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92c9835e-2481-4ca9-acf7-d6fcc45b1f66");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f1c276a1-3945-455b-9a50-de572b2e03aa");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "eb3356dc-02e0-4942-ba6c-9482948384f4", "d1444432-c069-448d-95be-466f127c790e", "Admin", "ADMIN" });
        }
    }
}
