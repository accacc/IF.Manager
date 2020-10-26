using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _10261 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForeignKeyPropertyId",
                table: "IFEntityRelation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromEntityName",
                table: "IFEntityRelation",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDbFirst",
                table: "IFEntityRelation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ToEntityName",
                table: "IFEntityRelation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForeignKeyPropertyId",
                table: "IFEntityRelation");

            migrationBuilder.DropColumn(
                name: "FromEntityName",
                table: "IFEntityRelation");

            migrationBuilder.DropColumn(
                name: "IsDbFirst",
                table: "IFEntityRelation");

            migrationBuilder.DropColumn(
                name: "ToEntityName",
                table: "IFEntityRelation");
        }
    }
}
