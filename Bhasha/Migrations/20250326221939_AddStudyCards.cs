using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bhasha.Migrations
{
    /// <inheritdoc />
    public partial class AddStudyCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudyCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Language = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StudyLanguage = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    AudioId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ChapterDtoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyCards_Chapters_ChapterDtoId",
                        column: x => x.ChapterDtoId,
                        principalTable: "Chapters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudyCards_ChapterDtoId",
                table: "StudyCards",
                column: "ChapterDtoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudyCards");
        }
    }
}
