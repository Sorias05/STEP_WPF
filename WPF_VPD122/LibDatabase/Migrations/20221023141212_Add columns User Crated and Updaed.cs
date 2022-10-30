using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibDatabase.Migrations
{
    public partial class AddcolumnsUserCratedandUpdaed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "tblUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "tblUsers",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "tblUsers");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "tblUsers");
        }
    }
}
