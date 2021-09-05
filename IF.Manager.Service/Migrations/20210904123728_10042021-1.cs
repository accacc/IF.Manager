using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Service.Migrations
{
    public partial class _100420211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAuthenticationAdded",
                table: "IFProject",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAuthenticationAdded",
                table: "IFProject");
        }
    }
}
