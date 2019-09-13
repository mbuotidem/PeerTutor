using Microsoft.EntityFrameworkCore.Migrations;

namespace PeerTutor.Migrations
{
    public partial class AddCourseTutors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseTutors",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    CourseId = table.Column<int>(nullable: false),
                    Grade = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTutors", x => new { x.UserId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_CourseTutors_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTutors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseTutors_CourseId",
                table: "CourseTutors",
                column: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseTutors");
        }
    }
}
