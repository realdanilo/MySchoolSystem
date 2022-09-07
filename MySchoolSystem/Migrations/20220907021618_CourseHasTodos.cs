using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchoolSystem.Migrations
{
    public partial class CourseHasTodos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CourseId",
                table: "Tasks",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Courses_CourseId",
                table: "Tasks",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Courses_CourseId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_CourseId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Tasks");
        }
    }
}
