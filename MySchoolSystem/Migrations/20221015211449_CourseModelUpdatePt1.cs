using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchoolSystem.Migrations
{
    public partial class CourseModelUpdatePt1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OpenForEnrollment",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PeriodId",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_PeriodId",
                table: "Courses",
                column: "PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Periods_PeriodId",
                table: "Courses",
                column: "PeriodId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Periods_PeriodId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_PeriodId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "OpenForEnrollment",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Courses");
        }
    }
}
