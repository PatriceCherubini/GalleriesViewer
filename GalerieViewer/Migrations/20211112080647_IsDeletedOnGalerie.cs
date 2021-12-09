using Microsoft.EntityFrameworkCore.Migrations;

namespace GalerieViewer.Migrations
{
    public partial class IsDeletedOnGalerie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Galeries",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Galeries");
        }
    }
}
