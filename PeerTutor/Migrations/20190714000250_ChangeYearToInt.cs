using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerTutor.Migrations
{
    public partial class ChangeYearToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClassYear",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ClassYear",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
