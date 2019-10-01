using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerTutor.Migrations
{
    public partial class UpdateReviewWithStars : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stars",
                table: "Reviews",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stars",
                table: "Reviews");
        }
    }
}
