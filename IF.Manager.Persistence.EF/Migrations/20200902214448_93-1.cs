using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _931 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageControl_IFQuery_IFPageNameValueControl_IFQueryId",
                table: "IFPageControl");

            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_IFPageFormId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropIndex(
                name: "IX_IFPageFormItemModelProperty_IFPageFormId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropIndex(
                name: "IX_IFPageControl_IFPageNameValueControl_IFQueryId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "IFPageFormId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropColumn(
                name: "IFPageNameValueControl_IFQueryId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "NamePropertyId",
                table: "IFPageControl");

            migrationBuilder.DropColumn(
                name: "ValuePropertyId",
                table: "IFPageControl");

            migrationBuilder.AddColumn<int>(
                name: "ObjectId",
                table: "IFPageFormItemModelProperty",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageFormItemModelProperty_ObjectId",
                table: "IFPageFormItemModelProperty",
                column: "ObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_ObjectId",
                table: "IFPageFormItemModelProperty",
                column: "ObjectId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_ObjectId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropIndex(
                name: "IX_IFPageFormItemModelProperty_ObjectId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "IFPageFormItemModelProperty");

            migrationBuilder.AddColumn<int>(
                name: "IFPageFormId",
                table: "IFPageFormItemModelProperty",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IFPageNameValueControl_IFQueryId",
                table: "IFPageControl",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NamePropertyId",
                table: "IFPageControl",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ValuePropertyId",
                table: "IFPageControl",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageFormItemModelProperty_IFPageFormId",
                table: "IFPageFormItemModelProperty",
                column: "IFPageFormId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageNameValueControl_IFQueryId",
                table: "IFPageControl",
                column: "IFPageNameValueControl_IFQueryId");

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageControl_IFQuery_IFPageNameValueControl_IFQueryId",
                table: "IFPageControl",
                column: "IFPageNameValueControl_IFQueryId",
                principalTable: "IFQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IFPageFormItemModelProperty_IFPageControl_IFPageFormId",
                table: "IFPageFormItemModelProperty",
                column: "IFPageFormId",
                principalTable: "IFPageControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
