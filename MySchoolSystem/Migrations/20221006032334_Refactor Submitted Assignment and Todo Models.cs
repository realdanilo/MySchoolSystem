using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchoolSystem.Migrations
{
    public partial class RefactorSubmittedAssignmentandTodoModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileLocation",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "FileLocation",
                table: "Submitted_Assignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileLocation",
                table: "Submitted_Assignments");

            migrationBuilder.AddColumn<string>(
                name: "FileLocation",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
