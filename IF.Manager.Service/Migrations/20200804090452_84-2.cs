using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _842 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageParameter_IFPageControl_IFPageId",
                table: "IFPageParameter");

            migrationBuilder.DropTable(
                name: "IFPageListViewParameters");

            migrationBuilder.DropIndex(
                name: "IX_IFPageParameter_IFPageId",
                table: "IFPageParameter");

            migrationBuilder.DropColumn(
                name: "IFPageId",
                table: "IFPageParameter");

            migrationBuilder.AddColumn<int>(
                name: "ObjectId",
                table: "IFPageParameter",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObjectType",
                table: "IFPageParameter",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageParameter_ObjectId",
                table: "IFPageParameter",
                column: "ObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageParameter_IFPageControl_ObjectId",
                table: "IFPageParameter",
                column: "ObjectId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageParameter_IFPageControl_ObjectId1",
                table: "IFPageParameter",
                column: "ObjectId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageParameter_IFPageControl_ObjectId",
                table: "IFPageParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageParameter_IFPageControl_ObjectId1",
                table: "IFPageParameter");

            migrationBuilder.DropIndex(
                name: "IX_IFPageParameter_ObjectId",
                table: "IFPageParameter");

            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "IFPageParameter");

            migrationBuilder.DropColumn(
                name: "ObjectType",
                table: "IFPageParameter");

            migrationBuilder.AddColumn<int>(
                name: "IFPageId",
                table: "IFPageParameter",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IFPageListViewParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IFPageListViewId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageListViewParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageListViewParameters_IFPageControl_IFPageListViewId",
                        column: x => x.IFPageListViewId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFPageParameter_IFPageId",
                table: "IFPageParameter",
                column: "IFPageId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageListViewParameters_IFPageListViewId",
                table: "IFPageListViewParameters",
                column: "IFPageListViewId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageParameter_IFPageControl_IFPageId",
                table: "IFPageParameter",
                column: "IFPageId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
