using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _28101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForeignKeyPropertyId",
                table: "IFEntityRelation");

            migrationBuilder.AddColumn<int>(
                name: "ForeignKeyIFEntityPropertyId",
                table: "IFEntityRelation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFEntityRelation_ForeignKeyIFEntityPropertyId",
                table: "IFEntityRelation",
                column: "ForeignKeyIFEntityPropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFEntityRelation_IFEntityProperty_ForeignKeyIFEntityPropertyId",
                table: "IFEntityRelation",
                column: "ForeignKeyIFEntityPropertyId",
                principalTable: "IFEntityProperty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFEntityRelation_IFEntityProperty_ForeignKeyIFEntityPropertyId",
                table: "IFEntityRelation");

            migrationBuilder.DropIndex(
                name: "IX_IFEntityRelation_ForeignKeyIFEntityPropertyId",
                table: "IFEntityRelation");

            migrationBuilder.DropColumn(
                name: "ForeignKeyIFEntityPropertyId",
                table: "IFEntityRelation");

            migrationBuilder.AddColumn<int>(
                name: "ForeignKeyPropertyId",
                table: "IFEntityRelation",
                type: "int",
                nullable: true);
        }
    }
}
