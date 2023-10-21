using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppScheduler_v1.Migrations
{
    public partial class ChangeAppointmentModelDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Appointments",
                newName: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Appointments",
                newName: "DateTime");
        }
    }
}
