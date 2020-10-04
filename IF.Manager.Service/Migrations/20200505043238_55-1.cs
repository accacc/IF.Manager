using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Persistence.EF.Migrations
{
    public partial class _551 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IFEntityGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Prefix = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFEntityGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IFFormModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFFormModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IFLanguageEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityPropertyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFLanguageEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IFSolution",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolutionName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Path = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFSolution", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IFEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    GroupId = table.Column<int>(nullable: true),
                    IsAudited = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFEntity_IFEntityGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "IFEntityGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFFormModelProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IsNullable = table.Column<bool>(nullable: false),
                    FormModelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFFormModelProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFFormModelProperty_IFFormModel_FormModelId",
                        column: x => x.FormModelId,
                        principalTable: "IFFormModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFProject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    ConnectionString = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    SolutionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFProject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFProject_IFSolution_SolutionId",
                        column: x => x.SolutionId,
                        principalTable: "IFSolution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IFEntityProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    IsIdentity = table.Column<bool>(nullable: false, defaultValue: false),
                    MaxValue = table.Column<int>(nullable: true),
                    EntityId = table.Column<int>(nullable: false),
                    IsAudited = table.Column<bool>(nullable: false),
                    IsMultiLanguage = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFEntityProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFEntityProperty_IFEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "IFEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IFEntityRelation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(nullable: false),
                    RelationId = table.Column<int>(nullable: false),
                    From = table.Column<int>(nullable: false),
                    To = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFEntityRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFEntityRelation_IFEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "IFEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFEntityRelation_IFEntity_RelationId",
                        column: x => x.RelationId,
                        principalTable: "IFEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFModel_IFEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "IFEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IFProcess",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFProcess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFProcess_IFProject_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "IFProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IFCommand",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommandGetType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    FormModelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFCommand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFCommand_IFFormModel_FormModelId",
                        column: x => x.FormModelId,
                        principalTable: "IFFormModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFCommand_IFModel_ModelId",
                        column: x => x.ModelId,
                        principalTable: "IFModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFModelProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(nullable: false),
                    EntityPropertyId = table.Column<int>(nullable: false),
                    ModelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFModelProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFModelProperty_IFEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "IFEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFModelProperty_IFEntityProperty_EntityPropertyId",
                        column: x => x.EntityPropertyId,
                        principalTable: "IFEntityProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFModelProperty_IFModel_ModelId",
                        column: x => x.ModelId,
                        principalTable: "IFModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFPublish",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    SolutionId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    ProcessId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPublish", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPublish_IFProcess_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "IFProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPublish_IFSolution_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "IFSolution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPublish_IFSolution_SolutionId",
                        column: x => x.SolutionId,
                        principalTable: "IFSolution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFQuery",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueryGetType = table.Column<int>(nullable: false),
                    PageSize = table.Column<int>(nullable: true),
                    PageNumber = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    FormModelId = table.Column<int>(nullable: false),
                    ProcessId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFQuery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFQuery_IFFormModel_FormModelId",
                        column: x => x.FormModelId,
                        principalTable: "IFFormModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFQuery_IFModel_ModelId",
                        column: x => x.ModelId,
                        principalTable: "IFModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFQuery_IFProcess_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "IFProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFCommandFilterItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConditionOperator = table.Column<int>(nullable: false),
                    FilterOperator = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    CommandId = table.Column<int>(nullable: false),
                    EntityPropertyId = table.Column<int>(nullable: false),
                    FormModelPropertyId = table.Column<int>(nullable: true),
                    IFEntityId = table.Column<int>(nullable: true),
                    IFFormModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFCommandFilterItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFCommandFilterItem_IFCommand_CommandId",
                        column: x => x.CommandId,
                        principalTable: "IFCommand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFCommandFilterItem_IFEntityProperty_EntityPropertyId",
                        column: x => x.EntityPropertyId,
                        principalTable: "IFEntityProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFCommandFilterItem_IFFormModelProperty_FormModelPropertyId",
                        column: x => x.FormModelPropertyId,
                        principalTable: "IFFormModelProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFCommandFilterItem_IFEntity_IFEntityId",
                        column: x => x.IFEntityId,
                        principalTable: "IFEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFCommandFilterItem_IFFormModel_IFFormModelId",
                        column: x => x.IFFormModelId,
                        principalTable: "IFFormModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFQueryFilterItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConditionOperator = table.Column<int>(nullable: false),
                    FilterOperator = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    QueryId = table.Column<int>(nullable: false),
                    EntityPropertyId = table.Column<int>(nullable: false),
                    FormModelPropertyId = table.Column<int>(nullable: true),
                    IFCommandId = table.Column<int>(nullable: true),
                    IFEntityId = table.Column<int>(nullable: true),
                    IFFormModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFQueryFilterItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFQueryFilterItem_IFEntityProperty_EntityPropertyId",
                        column: x => x.EntityPropertyId,
                        principalTable: "IFEntityProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFQueryFilterItem_IFFormModelProperty_FormModelPropertyId",
                        column: x => x.FormModelPropertyId,
                        principalTable: "IFFormModelProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFQueryFilterItem_IFCommand_IFCommandId",
                        column: x => x.IFCommandId,
                        principalTable: "IFCommand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFQueryFilterItem_IFEntity_IFEntityId",
                        column: x => x.IFEntityId,
                        principalTable: "IFEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFQueryFilterItem_IFFormModel_IFFormModelId",
                        column: x => x.IFFormModelId,
                        principalTable: "IFFormModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFQueryFilterItem_IFQuery_QueryId",
                        column: x => x.QueryId,
                        principalTable: "IFQuery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IFQueryOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueryId = table.Column<int>(nullable: false),
                    EntityPropertyId = table.Column<int>(nullable: false),
                    QueryOrderOperator = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFQueryOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFQueryOrder_IFEntityProperty_EntityPropertyId",
                        column: x => x.EntityPropertyId,
                        principalTable: "IFEntityProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFQueryOrder_IFQuery_QueryId",
                        column: x => x.QueryId,
                        principalTable: "IFQuery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_FormModelId",
                table: "IFCommand",
                column: "FormModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_ModelId",
                table: "IFCommand",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandFilterItem_CommandId",
                table: "IFCommandFilterItem",
                column: "CommandId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandFilterItem_EntityPropertyId",
                table: "IFCommandFilterItem",
                column: "EntityPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandFilterItem_FormModelPropertyId",
                table: "IFCommandFilterItem",
                column: "FormModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandFilterItem_IFEntityId",
                table: "IFCommandFilterItem",
                column: "IFEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandFilterItem_IFFormModelId",
                table: "IFCommandFilterItem",
                column: "IFFormModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFEntity_GroupId",
                table: "IFEntity",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_IFEntityProperty_EntityId",
                table: "IFEntityProperty",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_IFEntityRelation_EntityId",
                table: "IFEntityRelation",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_IFEntityRelation_RelationId",
                table: "IFEntityRelation",
                column: "RelationId");

            migrationBuilder.CreateIndex(
                name: "IX_IFFormModelProperty_FormModelId",
                table: "IFFormModelProperty",
                column: "FormModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFModel_EntityId",
                table: "IFModel",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_IFModelProperty_EntityId",
                table: "IFModelProperty",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_IFModelProperty_EntityPropertyId",
                table: "IFModelProperty",
                column: "EntityPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFModelProperty_ModelId",
                table: "IFModelProperty",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFProcess_ProjectId",
                table: "IFProcess",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_IFProject_SolutionId",
                table: "IFProject",
                column: "SolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPublish_ProcessId",
                table: "IFPublish",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPublish_ProjectId",
                table: "IFPublish",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPublish_SolutionId",
                table: "IFPublish",
                column: "SolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQuery_FormModelId",
                table: "IFQuery",
                column: "FormModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQuery_ModelId",
                table: "IFQuery",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQuery_ProcessId",
                table: "IFQuery",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_EntityPropertyId",
                table: "IFQueryFilterItem",
                column: "EntityPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_FormModelPropertyId",
                table: "IFQueryFilterItem",
                column: "FormModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_IFCommandId",
                table: "IFQueryFilterItem",
                column: "IFCommandId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_IFEntityId",
                table: "IFQueryFilterItem",
                column: "IFEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_IFFormModelId",
                table: "IFQueryFilterItem",
                column: "IFFormModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_QueryId",
                table: "IFQueryFilterItem",
                column: "QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryOrder_EntityPropertyId",
                table: "IFQueryOrder",
                column: "EntityPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryOrder_QueryId",
                table: "IFQueryOrder",
                column: "QueryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IFCommandFilterItem");

            migrationBuilder.DropTable(
                name: "IFEntityRelation");

            migrationBuilder.DropTable(
                name: "IFLanguageEntity");

            migrationBuilder.DropTable(
                name: "IFModelProperty");

            migrationBuilder.DropTable(
                name: "IFPublish");

            migrationBuilder.DropTable(
                name: "IFQueryFilterItem");

            migrationBuilder.DropTable(
                name: "IFQueryOrder");

            migrationBuilder.DropTable(
                name: "IFFormModelProperty");

            migrationBuilder.DropTable(
                name: "IFCommand");

            migrationBuilder.DropTable(
                name: "IFEntityProperty");

            migrationBuilder.DropTable(
                name: "IFQuery");

            migrationBuilder.DropTable(
                name: "IFFormModel");

            migrationBuilder.DropTable(
                name: "IFModel");

            migrationBuilder.DropTable(
                name: "IFProcess");

            migrationBuilder.DropTable(
                name: "IFEntity");

            migrationBuilder.DropTable(
                name: "IFProject");

            migrationBuilder.DropTable(
                name: "IFEntityGroup");

            migrationBuilder.DropTable(
                name: "IFSolution");
        }
    }
}
