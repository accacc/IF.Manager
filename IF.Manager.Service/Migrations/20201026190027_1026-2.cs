using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _10262 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "From",
                table: "IFEntityRelation");

            migrationBuilder.DropColumn(
                name: "FromEntityName",
                table: "IFEntityRelation");

            migrationBuilder.DropColumn(
                name: "To",
                table: "IFEntityRelation");

            migrationBuilder.DropColumn(
                name: "ToEntityName",
                table: "IFEntityRelation");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "IFEntityRelation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "IFEntityRelation");

            migrationBuilder.AddColumn<int>(
                name: "From",
                table: "IFEntityRelation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FromEntityName",
                table: "IFEntityRelation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "To",
                table: "IFEntityRelation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ToEntityName",
                table: "IFEntityRelation",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
