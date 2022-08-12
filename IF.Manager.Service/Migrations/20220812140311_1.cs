using Microsoft.EntityFrameworkCore.Migrations;

namespace IF.Manager.Service.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IFClass",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    GenericType = table.Column<string>(nullable: true),
                    IsNullable = table.Column<bool>(nullable: false),
                    IsPrimitive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFClass_IFClass_ParentId",
                        column: x => x.ParentId,
                        principalTable: "IFClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IFPageFormItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageFormItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IFPageFormLayout",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageFormLayout", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IFPageLayout",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ColumSize = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageLayout", x => x.Id);
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
                    AuditType = table.Column<int>(nullable: false),
                    IsSoftDeleted = table.Column<bool>(nullable: false)
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
                name: "IFPageGridLayout",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    LayoutId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageGridLayout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageGridLayout_IFPageLayout_LayoutId",
                        column: x => x.LayoutId,
                        principalTable: "IFPageLayout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFProject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProjectType = table.Column<int>(nullable: false),
                    ConnectionString = table.Column<string>(nullable: false),
                    SystemDbConnectionString = table.Column<string>(nullable: true),
                    SystemDbType = table.Column<int>(nullable: false),
                    JsonAppType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    SolutionId = table.Column<int>(nullable: false),
                    AuthenticationType = table.Column<int>(nullable: false),
                    CommandAudit = table.Column<bool>(nullable: false),
                    IsAuthenticationAdded = table.Column<bool>(nullable: false),
                    CommandPerformanceCounter = table.Column<bool>(nullable: false),
                    CommandErrorHandler = table.Column<bool>(nullable: false),
                    QueryAudit = table.Column<bool>(nullable: false),
                    QueryPerformanceCounter = table.Column<bool>(nullable: false),
                    QueryErrorHandler = table.Column<bool>(nullable: false)
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
                    IsAutoNumber = table.Column<bool>(nullable: false),
                    MaxValue = table.Column<int>(nullable: true),
                    EntityId = table.Column<int>(nullable: false),
                    IsAudited = table.Column<bool>(nullable: false),
                    IsMultiLanguage = table.Column<bool>(nullable: false),
                    IsNullable = table.Column<bool>(nullable: false)
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
                name: "IFEntityRelation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prefix = table.Column<string>(nullable: true),
                    ForeignKeyIFEntityPropertyId = table.Column<int>(nullable: true),
                    IsDbFirst = table.Column<bool>(nullable: false),
                    EntityId = table.Column<int>(nullable: false),
                    RelationId = table.Column<int>(nullable: false),
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
                        name: "FK_IFEntityRelation_IFEntityProperty_ForeignKeyIFEntityPropertyId",
                        column: x => x.ForeignKeyIFEntityPropertyId,
                        principalTable: "IFEntityProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFEntityRelation_IFEntity_RelationId",
                        column: x => x.RelationId,
                        principalTable: "IFEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFClassMapper",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IFClassId = table.Column<int>(nullable: true),
                    IFModelId = table.Column<int>(nullable: true),
                    IsList = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFClassMapper", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFClassMapper_IFClass_IFClassId",
                        column: x => x.IFClassId,
                        principalTable: "IFClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFClassMapper_IFModel_IFModelId",
                        column: x => x.IFModelId,
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
                    IsQueryOverride = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    ProcessId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFQuery", x => x.Id);
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
                name: "IFCommand",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    IFClassMapperId = table.Column<int>(nullable: true),
                    CommandGetType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ModelId = table.Column<int>(nullable: true),
                    IsQueryOverride = table.Column<bool>(nullable: false),
                    ProcessId = table.Column<int>(nullable: false),
                    IsList = table.Column<bool>(nullable: false),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFCommand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFCommand_IFClassMapper_IFClassMapperId",
                        column: x => x.IFClassMapperId,
                        principalTable: "IFClassMapper",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFCommand_IFModel_ModelId",
                        column: x => x.ModelId,
                        principalTable: "IFModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFCommand_IFCommand_ParentId",
                        column: x => x.ParentId,
                        principalTable: "IFCommand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFCommand_IFProcess_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "IFProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFClassMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IFClassMapperId = table.Column<int>(nullable: false),
                    FromPropertyId = table.Column<int>(nullable: true),
                    ToPropertyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFClassMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFClassMapping_IFClass_FromPropertyId",
                        column: x => x.FromPropertyId,
                        principalTable: "IFClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFClassMapping_IFClassMapper_IFClassMapperId",
                        column: x => x.IFClassMapperId,
                        principalTable: "IFClassMapper",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFClassMapping_IFModelProperty_ToPropertyId",
                        column: x => x.ToPropertyId,
                        principalTable: "IFModelProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    IFEntityId = table.Column<int>(nullable: true)
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
                        name: "FK_IFCommandFilterItem_IFEntity_IFEntityId",
                        column: x => x.IFEntityId,
                        principalTable: "IFEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFPageControl",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControlType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    CssClass = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true),
                    IFPageControlMapId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ProcessId = table.Column<int>(nullable: true),
                    IFProjectId = table.Column<int>(nullable: true),
                    PageLayoutId = table.Column<int>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Style = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: true),
                    QueryId = table.Column<int>(nullable: true),
                    CommandId = table.Column<int>(nullable: true),
                    IFModelId = table.Column<int>(nullable: true),
                    IFPageControlId = table.Column<int>(nullable: true),
                    WidgetType = table.Column<int>(nullable: true),
                    ActionType = table.Column<int>(nullable: true),
                    WidgetRenderType = table.Column<int>(nullable: true),
                    FormLayoutId = table.Column<int>(nullable: true),
                    IFPageForm_IFModelId = table.Column<int>(nullable: true),
                    IFQueryId = table.Column<int>(nullable: true),
                    IFPageGrid_QueryId = table.Column<int>(nullable: true),
                    IFFilterPageFormId = table.Column<int>(nullable: true),
                    GridLayoutId = table.Column<int>(nullable: true),
                    IFPageListView_FormLayoutId = table.Column<int>(nullable: true),
                    IFPageListView_IFQueryId = table.Column<int>(nullable: true),
                    IFPageNavigation_IFProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageControl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFProject_IFProjectId",
                        column: x => x.IFProjectId,
                        principalTable: "IFProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFPageLayout_PageLayoutId",
                        column: x => x.PageLayoutId,
                        principalTable: "IFPageLayout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFProcess_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "IFProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFCommand_CommandId",
                        column: x => x.CommandId,
                        principalTable: "IFCommand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFModel_IFModelId",
                        column: x => x.IFModelId,
                        principalTable: "IFModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFPageControl_IFPageControlId",
                        column: x => x.IFPageControlId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFQuery_QueryId",
                        column: x => x.QueryId,
                        principalTable: "IFQuery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFPageFormLayout_FormLayoutId",
                        column: x => x.FormLayoutId,
                        principalTable: "IFPageFormLayout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFModel_IFPageForm_IFModelId",
                        column: x => x.IFPageForm_IFModelId,
                        principalTable: "IFModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFQuery_IFQueryId",
                        column: x => x.IFQueryId,
                        principalTable: "IFQuery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFPageGridLayout_GridLayoutId",
                        column: x => x.GridLayoutId,
                        principalTable: "IFPageGridLayout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFPageControl_IFFilterPageFormId",
                        column: x => x.IFFilterPageFormId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFQuery_IFPageGrid_QueryId",
                        column: x => x.IFPageGrid_QueryId,
                        principalTable: "IFQuery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFPageFormLayout_IFPageListView_FormLayoutId",
                        column: x => x.IFPageListView_FormLayoutId,
                        principalTable: "IFPageFormLayout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFQuery_IFPageListView_IFQueryId",
                        column: x => x.IFPageListView_IFQueryId,
                        principalTable: "IFQuery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControl_IFProject_IFPageNavigation_IFProjectId",
                        column: x => x.IFPageNavigation_IFProjectId,
                        principalTable: "IFProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IFPageControlItemModelProperty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sequence = table.Column<int>(nullable: false),
                    IFPageFormItemId = table.Column<int>(nullable: false),
                    IFModelPropertyId = table.Column<int>(nullable: false),
                    ObjectId = table.Column<int>(nullable: true),
                    IFQueryId = table.Column<int>(nullable: true),
                    NameIFModelPropertyId = table.Column<int>(nullable: true),
                    ValueIFModelPropertyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageControlItemModelProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageControlItemModelProperty_IFModelProperty_IFModelPropertyId",
                        column: x => x.IFModelPropertyId,
                        principalTable: "IFModelProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFPageControlItemModelProperty_IFPageFormItem_IFPageFormItemId",
                        column: x => x.IFPageFormItemId,
                        principalTable: "IFPageFormItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFPageControlItemModelProperty_IFQuery_IFQueryId",
                        column: x => x.IFQueryId,
                        principalTable: "IFQuery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControlItemModelProperty_IFModelProperty_NameIFModelPropertyId",
                        column: x => x.NameIFModelPropertyId,
                        principalTable: "IFModelProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControlItemModelProperty_IFPageControl_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControlItemModelProperty_IFPageControl_ObjectId1",
                        column: x => x.ObjectId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageControlItemModelProperty_IFModelProperty_ValueIFModelPropertyId",
                        column: x => x.ValueIFModelPropertyId,
                        principalTable: "IFModelProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFPageControlMap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    IFPageControlId = table.Column<int>(nullable: false),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageControlMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageControlMap_IFPageControl_IFPageControlId",
                        column: x => x.IFPageControlId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFPageControlMap_IFPageControlMap_ParentId",
                        column: x => x.ParentId,
                        principalTable: "IFPageControlMap",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFPageParameter",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<int>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    ObjectType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageParameter_IFPageControl_ObjectId",
                        column: x => x.ObjectId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageParameter_IFPageControl_ObjectId1",
                        column: x => x.ObjectId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFPageParameter_IFPageControl_ObjectId2",
                        column: x => x.ObjectId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IFPageActionRouteValue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IFPageParameterId = table.Column<int>(nullable: false),
                    IFModelPropertyId = table.Column<int>(nullable: false),
                    IFPageActionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IFPageActionRouteValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IFPageActionRouteValue_IFModelProperty_IFModelPropertyId",
                        column: x => x.IFModelPropertyId,
                        principalTable: "IFModelProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFPageActionRouteValue_IFPageControl_IFPageActionId",
                        column: x => x.IFPageActionId,
                        principalTable: "IFPageControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IFPageActionRouteValue_IFPageParameter_IFPageParameterId",
                        column: x => x.IFPageParameterId,
                        principalTable: "IFPageParameter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IFQueryFilterItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    ConditionOperator = table.Column<int>(nullable: false),
                    FilterOperator = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    QueryId = table.Column<int>(nullable: false),
                    EntityPropertyId = table.Column<int>(nullable: false),
                    FormModelPropertyId = table.Column<int>(nullable: true),
                    IFPageParameterId = table.Column<int>(nullable: true),
                    IsNullCheck = table.Column<bool>(nullable: true),
                    IFEntityId = table.Column<int>(nullable: true)
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
                        name: "FK_IFQueryFilterItem_IFEntity_IFEntityId",
                        column: x => x.IFEntityId,
                        principalTable: "IFEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFQueryFilterItem_IFPageParameter_IFPageParameterId",
                        column: x => x.IFPageParameterId,
                        principalTable: "IFPageParameter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFQueryFilterItem_IFQueryFilterItem_ParentId",
                        column: x => x.ParentId,
                        principalTable: "IFQueryFilterItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IFQueryFilterItem_IFQuery_QueryId",
                        column: x => x.QueryId,
                        principalTable: "IFQuery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "IFPageFormItem",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Textbox", "Textbox" },
                    { 2, "Datepicker", "Datepicker" },
                    { 3, "Checkbox", "Checkbox" },
                    { 4, "DropDown", "DropDown" },
                    { 5, "MultipleSelect", "MultipleSelect" }
                });

            migrationBuilder.InsertData(
                table: "IFPageFormLayout",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "Standart", "Standart" });

            migrationBuilder.InsertData(
                table: "IFPageLayout",
                columns: new[] { "Id", "ColumSize", "Description", "Name" },
                values: new object[] { 1, 2, "Two Column", "Two Column" });

            migrationBuilder.InsertData(
                table: "IFPageGridLayout",
                columns: new[] { "Id", "Description", "LayoutId", "Name" },
                values: new object[] { 1, "Grid Layout", 1, "Grid Layout" });

            migrationBuilder.CreateIndex(
                name: "IX_IFClass_ParentId",
                table: "IFClass",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_IFClassMapper_IFClassId",
                table: "IFClassMapper",
                column: "IFClassId");

            migrationBuilder.CreateIndex(
                name: "IX_IFClassMapper_IFModelId",
                table: "IFClassMapper",
                column: "IFModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFClassMapping_FromPropertyId",
                table: "IFClassMapping",
                column: "FromPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFClassMapping_IFClassMapperId",
                table: "IFClassMapping",
                column: "IFClassMapperId");

            migrationBuilder.CreateIndex(
                name: "IX_IFClassMapping_ToPropertyId",
                table: "IFClassMapping",
                column: "ToPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_IFClassMapperId",
                table: "IFCommand",
                column: "IFClassMapperId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_ModelId",
                table: "IFCommand",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_ParentId",
                table: "IFCommand",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommand_ProcessId",
                table: "IFCommand",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandFilterItem_CommandId",
                table: "IFCommandFilterItem",
                column: "CommandId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandFilterItem_EntityPropertyId",
                table: "IFCommandFilterItem",
                column: "EntityPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFCommandFilterItem_IFEntityId",
                table: "IFCommandFilterItem",
                column: "IFEntityId");

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
                name: "IX_IFEntityRelation_ForeignKeyIFEntityPropertyId",
                table: "IFEntityRelation",
                column: "ForeignKeyIFEntityPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFEntityRelation_RelationId",
                table: "IFEntityRelation",
                column: "RelationId");

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
                name: "IX_IFPageActionRouteValue_IFModelPropertyId",
                table: "IFPageActionRouteValue",
                column: "IFModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageActionRouteValue_IFPageActionId",
                table: "IFPageActionRouteValue",
                column: "IFPageActionId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageActionRouteValue_IFPageParameterId",
                table: "IFPageActionRouteValue",
                column: "IFPageParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFProjectId",
                table: "IFPageControl",
                column: "IFProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_PageLayoutId",
                table: "IFPageControl",
                column: "PageLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_ProcessId",
                table: "IFPageControl",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_CommandId",
                table: "IFPageControl",
                column: "CommandId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFModelId",
                table: "IFPageControl",
                column: "IFModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageControlId",
                table: "IFPageControl",
                column: "IFPageControlId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_QueryId",
                table: "IFPageControl",
                column: "QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_FormLayoutId",
                table: "IFPageControl",
                column: "FormLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageForm_IFModelId",
                table: "IFPageControl",
                column: "IFPageForm_IFModelId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFQueryId",
                table: "IFPageControl",
                column: "IFQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_GridLayoutId",
                table: "IFPageControl",
                column: "GridLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFFilterPageFormId",
                table: "IFPageControl",
                column: "IFFilterPageFormId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageGrid_QueryId",
                table: "IFPageControl",
                column: "IFPageGrid_QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageListView_FormLayoutId",
                table: "IFPageControl",
                column: "IFPageListView_FormLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageListView_IFQueryId",
                table: "IFPageControl",
                column: "IFPageListView_IFQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControl_IFPageNavigation_IFProjectId",
                table: "IFPageControl",
                column: "IFPageNavigation_IFProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlItemModelProperty_IFModelPropertyId",
                table: "IFPageControlItemModelProperty",
                column: "IFModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlItemModelProperty_IFPageFormItemId",
                table: "IFPageControlItemModelProperty",
                column: "IFPageFormItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlItemModelProperty_IFQueryId",
                table: "IFPageControlItemModelProperty",
                column: "IFQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlItemModelProperty_NameIFModelPropertyId",
                table: "IFPageControlItemModelProperty",
                column: "NameIFModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlItemModelProperty_ObjectId",
                table: "IFPageControlItemModelProperty",
                column: "ObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlItemModelProperty_ValueIFModelPropertyId",
                table: "IFPageControlItemModelProperty",
                column: "ValueIFModelPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlMap_IFPageControlId",
                table: "IFPageControlMap",
                column: "IFPageControlId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IFPageControlMap_ParentId",
                table: "IFPageControlMap",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageGridLayout_LayoutId",
                table: "IFPageGridLayout",
                column: "LayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_IFPageParameter_ObjectId",
                table: "IFPageParameter",
                column: "ObjectId");

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
                name: "IX_IFQueryFilterItem_IFEntityId",
                table: "IFQueryFilterItem",
                column: "IFEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_IFPageParameterId",
                table: "IFQueryFilterItem",
                column: "IFPageParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_IFQueryFilterItem_ParentId",
                table: "IFQueryFilterItem",
                column: "ParentId");

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
                name: "IFClassMapping");

            migrationBuilder.DropTable(
                name: "IFCommandFilterItem");

            migrationBuilder.DropTable(
                name: "IFEntityRelation");

            migrationBuilder.DropTable(
                name: "IFLanguageEntity");

            migrationBuilder.DropTable(
                name: "IFPageActionRouteValue");

            migrationBuilder.DropTable(
                name: "IFPageControlItemModelProperty");

            migrationBuilder.DropTable(
                name: "IFPageControlMap");

            migrationBuilder.DropTable(
                name: "IFPublish");

            migrationBuilder.DropTable(
                name: "IFQueryFilterItem");

            migrationBuilder.DropTable(
                name: "IFQueryOrder");

            migrationBuilder.DropTable(
                name: "IFModelProperty");

            migrationBuilder.DropTable(
                name: "IFPageFormItem");

            migrationBuilder.DropTable(
                name: "IFPageParameter");

            migrationBuilder.DropTable(
                name: "IFEntityProperty");

            migrationBuilder.DropTable(
                name: "IFPageControl");

            migrationBuilder.DropTable(
                name: "IFCommand");

            migrationBuilder.DropTable(
                name: "IFQuery");

            migrationBuilder.DropTable(
                name: "IFPageFormLayout");

            migrationBuilder.DropTable(
                name: "IFPageGridLayout");

            migrationBuilder.DropTable(
                name: "IFClassMapper");

            migrationBuilder.DropTable(
                name: "IFProcess");

            migrationBuilder.DropTable(
                name: "IFPageLayout");

            migrationBuilder.DropTable(
                name: "IFClass");

            migrationBuilder.DropTable(
                name: "IFModel");

            migrationBuilder.DropTable(
                name: "IFProject");

            migrationBuilder.DropTable(
                name: "IFEntity");

            migrationBuilder.DropTable(
                name: "IFSolution");

            migrationBuilder.DropTable(
                name: "IFEntityGroup");
        }
    }
}
