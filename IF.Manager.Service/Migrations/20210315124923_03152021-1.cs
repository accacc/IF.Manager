using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _031520211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "IFCommand",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "IFCommand");
        }
    }
}
