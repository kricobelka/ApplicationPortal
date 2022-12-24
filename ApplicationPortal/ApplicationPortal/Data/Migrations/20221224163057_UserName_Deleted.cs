using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationPortal.Data.Migrations
{
    public partial class UserName_Deleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
