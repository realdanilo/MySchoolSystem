using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchoolSystem.Migrations
{
    public partial class ClearNullableWarning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10432363-48ae-45e8-8188-38316a740a8a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13655c66-752d-417d-98f9-8e9ae384af01");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87484e55-1263-44de-ba64-1956b821e3f4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ee7e88ab-0d1f-4adf-a8fc-c169dd6bbd35", "9e654de9-a32e-4830-b8bb-3bf2d6fbdbc2", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "07b51a10-e897-4f4e-be03-30e945bdbccd", "a1a35ab2-5ca8-4da0-9961-09f5eeaa0f92", "Student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3213b9b2-ac5a-45db-80fd-22eb9d81d292", "9c72dd60-5f89-471d-a001-d0ebb21ba87e", "Instructor", "INSTRUCTOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07b51a10-e897-4f4e-be03-30e945bdbccd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3213b9b2-ac5a-45db-80fd-22eb9d81d292");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee7e88ab-0d1f-4adf-a8fc-c169dd6bbd35");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "13655c66-752d-417d-98f9-8e9ae384af01", "ae8ddcfb-5333-4914-8560-74f01ccccc75", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "87484e55-1263-44de-ba64-1956b821e3f4", "9800f454-c7d3-4193-afab-9d865d8895ce", "Student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "10432363-48ae-45e8-8188-38316a740a8a", "7e459337-d561-4d0d-9d5e-e247a06bc9da", "Instructor", "INSTRUCTOR" });
        }
    }
}
