using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchoolSystem.Migrations
{
    public partial class OnlyAddRoleAtStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { "eb3356dc-02e0-4942-ba6c-9482948384f4", "d1444432-c069-448d-95be-466f127c790e", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eb3356dc-02e0-4942-ba6c-9482948384f4");

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
    }
}
