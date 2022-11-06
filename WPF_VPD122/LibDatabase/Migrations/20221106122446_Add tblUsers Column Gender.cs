using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibDatabase.Migrations
{
    public partial class AddtblUsersColumnGender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "tblUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "tblUsers");
        }
    }
}
