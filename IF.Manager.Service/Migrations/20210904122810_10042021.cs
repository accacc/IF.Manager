using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Service.Migrations
{
    public partial class _10042021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthenticationType",
                table: "IFProject",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthenticationType",
                table: "IFProject");
        }
    }
}
