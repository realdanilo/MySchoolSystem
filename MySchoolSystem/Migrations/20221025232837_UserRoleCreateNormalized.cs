using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchoolSystem.Migrations
{
    public partial class UserRoleCreateNormalized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a2740142-4fd9-4c6a-a378-6e98eb41f807", "0befb799-5c82-4993-bcdf-b555002f1307", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c2816117-8c29-4893-af58-2962b824d4fc", 0, "a39e6ee0-9157-47b9-9842-14f490ca6752", "admin@gmail.om", true, false, null, null, "ADMIN", "AQAAAAEAACcQAAAAEOH9yK01JEeo1hRdvoE35+7hSC5dezk1w4BCP2xtpLjI7rJnoV7IbK/YBx1dcey0vg==", null, false, "287ee284-4b8c-4f92-90c9-64acbce1be3f", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a2740142-4fd9-4c6a-a378-6e98eb41f807", "c2816117-8c29-4893-af58-2962b824d4fc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a2740142-4fd9-4c6a-a378-6e98eb41f807", "c2816117-8c29-4893-af58-2962b824d4fc" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2740142-4fd9-4c6a-a378-6e98eb41f807");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c2816117-8c29-4893-af58-2962b824d4fc");

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
    }
}
