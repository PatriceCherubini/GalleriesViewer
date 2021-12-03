using Microsoft.EntityFrameworkCore.Migrations;

namespace GalerieViewer.Migrations
{
    public partial class AddPictureThumb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileNameThumb",
                table: "ImageItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileNameThumb",
                table: "ImageItems");
        }
    }
}
