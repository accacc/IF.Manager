using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _10081 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IFCustomClassRelation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainIFCustomClassId = table.Column<int>(nullable: true),
                    RelatedIFCustomClassId = table.Column<int>(nullable: true),
                    RelationType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFCustomClassRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFCustomClassRelation_IFCustomClass_MainIFCustomClassId",
                        column: x => x.MainIFCustomClassId,
                        principalTable: "IFCustomClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFCustomClassRelation_IFCustomClass_RelatedIFCustomClassId",
                        column: x => x.RelatedIFCustomClassId,
                        principalTable: "IFCustomClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFCustomClassRelation_MainIFCustomClassId",
                table: "IFCustomClassRelation",
                column: "MainIFCustomClassId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCustomClassRelation_RelatedIFCustomClassId",
                table: "IFCustomClassRelation",
                column: "RelatedIFCustomClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFCustomClassRelation");
        }
    }
}
