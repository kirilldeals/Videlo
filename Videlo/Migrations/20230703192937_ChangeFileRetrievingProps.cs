using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Videlo.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFileRetrievingProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "URL",
                table: "Videos",
                newName: "VideoPath");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailPath",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailPath",
                table: "Videos");

            migrationBuilder.RenameColumn(
                name: "VideoPath",
                table: "Videos",
                newName: "URL");

            migrationBuilder.AddColumn<long>(
                name: "FileId",
                table: "Videos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FolderId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);
        }
    }
}
