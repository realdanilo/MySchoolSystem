using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchoolSystem.Migrations
{
    public partial class UserRoleCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6e3c83e5-5011-4403-9290-71634948c800", "0e5449fa-53ef-4d69-85e1-653b0a77357f", "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ce73cc75-f14d-465e-847a-618e83fc767c", 0, "38fe4412-6c96-4a39-bc8f-4834e97aed23", "admin@gmail.om", true, false, null, null, null, "AQAAAAEAACcQAAAAEFZdFkvWduVL0JbGmWrzBg/rNnPGr0nStVI5gco6cfp65ASlsI3O4Bezgjbnfs3hPw==", null, false, "511a105c-a5d1-492c-a86e-2d0c55c9ca72", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "6e3c83e5-5011-4403-9290-71634948c800", "ce73cc75-f14d-465e-847a-618e83fc767c" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "6e3c83e5-5011-4403-9290-71634948c800", "ce73cc75-f14d-465e-847a-618e83fc767c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e3c83e5-5011-4403-9290-71634948c800");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ce73cc75-f14d-465e-847a-618e83fc767c");
        }
    }
}
