using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchoolSystem.Migrations
{
    public partial class StudentRoleUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d8b2965-725e-4649-89fe-46aa4410107f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81878a46-3134-492a-a371-d5b0416dc67a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b0e88ec-a76e-4df5-9acf-b9b14828f45a");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "8b0e88ec-a76e-4df5-9acf-b9b14828f45a", "6ef7716d-374b-49e3-8f60-d3aa3a2581b7", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "81878a46-3134-492a-a371-d5b0416dc67a", "9c7e27f2-7dd9-41ed-8656-52f4ec88c9c8", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0d8b2965-725e-4649-89fe-46aa4410107f", "e84304c4-c625-4edf-b1c4-5f892554fb5e", "Instructor", "INSTRUCTOR" });
        }
    }
}
