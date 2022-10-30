using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibDatabase.Migrations
{
    public partial class AddimagecoltotblUserscs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "tblUsers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "tblUsers");
        }
    }
}
