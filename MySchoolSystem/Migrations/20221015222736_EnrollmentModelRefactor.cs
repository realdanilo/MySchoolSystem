using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchoolSystem.Migrations
{
    public partial class EnrollmentModelRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Periods_PeriodId",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_PeriodId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "OpenForEnrollment",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Enrollments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OpenForEnrollment",
                table: "Enrollments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PeriodId",
                table: "Enrollments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Enrollments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_PeriodId",
                table: "Enrollments",
                column: "PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Periods_PeriodId",
                table: "Enrollments",
                column: "PeriodId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
