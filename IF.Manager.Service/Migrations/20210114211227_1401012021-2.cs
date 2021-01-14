using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _14010120212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFClassMapping_IFClass_ToPropertyId",
                table: "IFClassMapping");

            migrationBuilder.DropColumn(
                name: "IsList",
                table: "IFClassMapping");

            migrationBuilder.AddColumn<bool>(
                name: "IsList",
                table: "IFClassMapper",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_IFClassMapping_IFModelProperty_ToPropertyId",
                table: "IFClassMapping",
                column: "ToPropertyId",
                principalTable: "IFModelProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFClassMapping_IFModelProperty_ToPropertyId",
                table: "IFClassMapping");

            migrationBuilder.DropColumn(
                name: "IsList",
                table: "IFClassMapper");

            migrationBuilder.AddColumn<bool>(
                name: "IsList",
                table: "IFClassMapping",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_IFClassMapping_IFClass_ToPropertyId",
                table: "IFClassMapping",
                column: "ToPropertyId",
                principalTable: "IFClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
