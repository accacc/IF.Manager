using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _10151 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomClassGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Prefix = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomClassGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomClass",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    CustomClassGroupId = table.Column<int>(nullable: true),
                    IsAudited = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomClass_CustomClassGroup_CustomClassGroupId",
                        column: x => x.CustomClassGroupId,
                        principalTable: "CustomClassGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomClassProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    CustomClassId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    IsNullable = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomClassProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomClassProperty_CustomClass_CustomClassId",
                        column: x => x.CustomClassId,
                        principalTable: "CustomClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomClassRelation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainCustomClassId = table.Column<int>(nullable: true),
                    RelatedCustomClassId = table.Column<int>(nullable: true),
                    RelationType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomClassRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomClassRelation_CustomClass_MainCustomClassId",
                        column: x => x.MainCustomClassId,
                        principalTable: "CustomClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomClassRelation_CustomClass_RelatedCustomClassId",
                        column: x => x.RelatedCustomClassId,
                        principalTable: "CustomClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomClass_CustomClassGroupId",
                table: "CustomClass",
                column: "CustomClassGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomClassProperty_CustomClassId",
                table: "CustomClassProperty",
                column: "CustomClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomClassRelation_MainCustomClassId",
                table: "CustomClassRelation",
                column: "MainCustomClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomClassRelation_RelatedCustomClassId",
                table: "CustomClassRelation",
                column: "RelatedCustomClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomClassProperty");

            migrationBuilder.DropTable(
                name: "CustomClassRelation");

            migrationBuilder.DropTable(
                name: "CustomClass");

            migrationBuilder.DropTable(
                name: "CustomClassGroup");
        }
    }
}
