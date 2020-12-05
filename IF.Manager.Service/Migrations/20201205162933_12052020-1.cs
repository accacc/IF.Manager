using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _120520201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomClassProperty");

            migrationBuilder.DropTable(
                name: "CustomClassRelation");

            migrationBuilder.DropTable(
                name: "IFCustomClassProperty");

            migrationBuilder.DropTable(
                name: "IFCustomClassRelation");

            migrationBuilder.DropTable(
                name: "CustomClass");

            migrationBuilder.DropTable(
                name: "IFCustomClass");

            migrationBuilder.DropTable(
                name: "CustomClassGroup");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomClassGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomClassGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IFCustomClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFCustomClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomClassGroupId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAudited = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "IFCustomClassProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IFCustomClassId = table.Column<int>(type: "int", nullable: false),
                    IsNullable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFCustomClassProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFCustomClassProperty_IFCustomClass_IFCustomClassId",
                        column: x => x.IFCustomClassId,
                        principalTable: "IFCustomClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IFCustomClassRelation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainIFCustomClassId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedIFCustomClassId = table.Column<int>(type: "int", nullable: true),
                    RelationType = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "CustomClassProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomClassId = table.Column<int>(type: "int", nullable: false),
                    IsNullable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainCustomClassId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedCustomClassId = table.Column<int>(type: "int", nullable: true),
                    RelationType = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_IFCustomClassProperty_IFCustomClassId",
                table: "IFCustomClassProperty",
                column: "IFCustomClassId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCustomClassRelation_MainIFCustomClassId",
                table: "IFCustomClassRelation",
                column: "MainIFCustomClassId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCustomClassRelation_RelatedIFCustomClassId",
                table: "IFCustomClassRelation",
                column: "RelatedIFCustomClassId");
        }
    }
}
