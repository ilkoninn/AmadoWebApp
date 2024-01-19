using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmadoApp.DAL.Migrations
{
    public partial class updateDbAndUpdateProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInStock",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInStock",
                table: "Products");
        }
    }
}
