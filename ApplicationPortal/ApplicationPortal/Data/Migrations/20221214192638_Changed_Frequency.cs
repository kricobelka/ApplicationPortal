using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationPortal.Data.Migrations
{
    public partial class Changed_Frequency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Range",
                table: "Frequencies",
                newName: "StartRange");

            migrationBuilder.AddColumn<double>(
                name: "EndRange",
                table: "Frequencies",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndRange",
                table: "Frequencies");

            migrationBuilder.RenameColumn(
                name: "StartRange",
                table: "Frequencies",
                newName: "Range");
        }
    }
}
