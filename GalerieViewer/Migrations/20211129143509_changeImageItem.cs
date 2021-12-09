using Microsoft.EntityFrameworkCore.Migrations;

namespace GalerieViewer.Migrations
{
    public partial class changeImageItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "ImageItems",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ImageItems",
                newName: "Nom");
        }
    }
}
