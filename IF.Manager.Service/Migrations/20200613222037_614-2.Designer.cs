﻿// <auto-generated />
using System;
using IF.Manager.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IF.Manager.Persistence.EF.Migrations
{
    [DbContext(typeof(ManagerDbContext))]
    [Migration("20200613222037_614-2")]
    partial class _6142
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFCommand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CommandGetType")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FormModelId")
                        .HasColumnType("int");

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProcessId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FormModelId");

                    b.HasIndex("ModelId");

                    b.HasIndex("ProcessId");

                    b.ToTable("IFCommand");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFCommandFilterItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CommandId")
                        .HasColumnType("int");

                    b.Property<int>("ConditionOperator")
                        .HasColumnType("int");

                    b.Property<int>("EntityPropertyId")
                        .HasColumnType("int");

                    b.Property<int>("FilterOperator")
                        .HasColumnType("int");

                    b.Property<int?>("FormModelPropertyId")
                        .HasColumnType("int");

                    b.Property<int?>("IFEntityId")
                        .HasColumnType("int");

                    b.Property<int?>("IFFormModelId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CommandId");

                    b.HasIndex("EntityPropertyId");

                    b.HasIndex("FormModelPropertyId");

                    b.HasIndex("IFEntityId");

                    b.HasIndex("IFFormModelId");

                    b.ToTable("IFCommandFilterItem");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAudited")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("IFEntity");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFEntityGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prefix")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IFEntityGroup");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFEntityProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAudited")
                        .HasColumnType("bit");

                    b.Property<bool>("IsIdentity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsMultiLanguage")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNullable")
                        .HasColumnType("bit");

                    b.Property<int?>("MaxValue")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.ToTable("IFEntityProperty");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFEntityRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<int>("From")
                        .HasColumnType("int");

                    b.Property<int>("RelationId")
                        .HasColumnType("int");

                    b.Property<int>("To")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.HasIndex("RelationId");

                    b.ToTable("IFEntityRelation");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFFormModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IFFormModel");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFFormModelProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FormModelId")
                        .HasColumnType("int");

                    b.Property<bool>("IsNullable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FormModelId");

                    b.ToTable("IFFormModelProperty");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFLanguageEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EntityPropertyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("IFLanguageEntity");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.ToTable("IFModel");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFModelProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<int>("EntityPropertyId")
                        .HasColumnType("int");

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.HasIndex("EntityPropertyId");

                    b.HasIndex("ModelId");

                    b.ToTable("IFModelProperty");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPageControl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ControlType")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IFPageControl");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IFPageControl");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPageControlMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PageControlId")
                        .HasColumnType("int");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PageControlId");

                    b.ToTable("IFPageControlMap");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPageFormLayout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IFPageFormLayout");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPageGridLayout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LayoutId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LayoutId");

                    b.ToTable("IFPageGridLayout");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPageLayout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IFPageLayout");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFProcess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("IFProcess");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConnectionString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectType")
                        .HasColumnType("int");

                    b.Property<int>("SolutionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SolutionId");

                    b.ToTable("IFProject");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPublish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProcessId")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int?>("SolutionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProcessId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("SolutionId");

                    b.ToTable("IFPublish");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFQuery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FormModelId")
                        .HasColumnType("int");

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PageNumber")
                        .HasColumnType("int");

                    b.Property<int?>("PageSize")
                        .HasColumnType("int");

                    b.Property<int>("ProcessId")
                        .HasColumnType("int");

                    b.Property<int>("QueryGetType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FormModelId");

                    b.HasIndex("ModelId");

                    b.HasIndex("ProcessId");

                    b.ToTable("IFQuery");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFQueryFilterItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ConditionOperator")
                        .HasColumnType("int");

                    b.Property<int>("EntityPropertyId")
                        .HasColumnType("int");

                    b.Property<int>("FilterOperator")
                        .HasColumnType("int");

                    b.Property<int?>("FormModelPropertyId")
                        .HasColumnType("int");

                    b.Property<int?>("IFEntityId")
                        .HasColumnType("int");

                    b.Property<int?>("IFFormModelId")
                        .HasColumnType("int");

                    b.Property<int>("QueryId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EntityPropertyId");

                    b.HasIndex("FormModelPropertyId");

                    b.HasIndex("IFEntityId");

                    b.HasIndex("IFFormModelId");

                    b.HasIndex("QueryId");

                    b.ToTable("IFQueryFilterItem");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFQueryOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EntityPropertyId")
                        .HasColumnType("int");

                    b.Property<int>("QueryId")
                        .HasColumnType("int");

                    b.Property<int>("QueryOrderOperator")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EntityPropertyId");

                    b.HasIndex("QueryId");

                    b.ToTable("IFQueryOrder");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFSolution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SolutionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IFSolution");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPage", b =>
                {
                    b.HasBaseType("IF.Manager.Contracts.Model.IFPageControl");

                    b.Property<int>("PageLayoutId")
                        .HasColumnType("int");

                    b.Property<int?>("ProcessId")
                        .HasColumnType("int");

                    b.HasIndex("PageLayoutId");

                    b.HasIndex("ProcessId");

                    b.HasDiscriminator().HasValue("IFPage");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPageAction", b =>
                {
                    b.HasBaseType("IF.Manager.Contracts.Model.IFPageControl");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.Property<string>("Style")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WidgetRenderType")
                        .HasColumnType("int");

                    b.Property<int>("WidgetType")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("IFPageAction");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPageForm", b =>
                {
                    b.HasBaseType("IF.Manager.Contracts.Model.IFPageControl");

                    b.Property<int>("FormLayoutId")
                        .HasColumnType("int");

                    b.HasIndex("FormLayoutId");

                    b.HasDiscriminator().HasValue("IFPageForm");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPageGrid", b =>
                {
                    b.HasBaseType("IF.Manager.Contracts.Model.IFPageControl");

                    b.Property<int?>("GridLayoutId")
                        .HasColumnType("int");

                    b.Property<int?>("QueryId")
                        .HasColumnType("int");

                    b.HasIndex("GridLayoutId");

                    b.HasIndex("QueryId");

                    b.HasDiscriminator().HasValue("IFPageGrid");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFCommand", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFFormModel", "FormModel")
                        .WithMany("Commands")
                        .HasForeignKey("FormModelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("IF.Manager.Contracts.Model.IFModel", "Model")
                        .WithMany("Commands")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("IF.Manager.Contracts.Model.IFProcess", "Process")
                        .WithMany("Commands")
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFCommandFilterItem", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFCommand", "Command")
                        .WithMany("CommandFilterItems")
                        .HasForeignKey("CommandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IF.Manager.Contracts.Model.IFEntityProperty", "EntityProperty")
                        .WithMany("CommandFilterItems")
                        .HasForeignKey("EntityPropertyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("IF.Manager.Contracts.Model.IFFormModelProperty", "FormModelProperty")
                        .WithMany("CommandFilterItems")
                        .HasForeignKey("FormModelPropertyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("IF.Manager.Contracts.Model.IFEntity", null)
                        .WithMany("CommandFilterItems")
                        .HasForeignKey("IFEntityId");

                    b.HasOne("IF.Manager.Contracts.Model.IFFormModel", null)
                        .WithMany("CommandFilterItems")
                        .HasForeignKey("IFFormModelId");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFEntity", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFEntityGroup", "Group")
                        .WithMany("Entities")
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFEntityProperty", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFEntity", "Entity")
                        .WithMany("Properties")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFEntityRelation", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFEntity", "Entity")
                        .WithMany("Relations")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IF.Manager.Contracts.Model.IFEntity", "Relation")
                        .WithMany("ReverseRelations")
                        .HasForeignKey("RelationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFFormModelProperty", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFFormModel", "FormModel")
                        .WithMany("Properties")
                        .HasForeignKey("FormModelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFModel", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFEntity", "Entity")
                        .WithMany()
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFModelProperty", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFEntity", "Entity")
                        .WithMany("ModelProperties")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("IF.Manager.Contracts.Model.IFEntityProperty", "EntityProperty")
                        .WithMany("ModelProperties")
                        .HasForeignKey("EntityPropertyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("IF.Manager.Contracts.Model.IFModel", "Model")
                        .WithMany("Properties")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPageControlMap", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFPageControl", "PageControl")
                        .WithMany()
                        .HasForeignKey("PageControlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPageGridLayout", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFPageLayout", "Layout")
                        .WithMany("GridLayouts")
                        .HasForeignKey("LayoutId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFProcess", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFProject", "Project")
                        .WithMany("Processes")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFProject", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFSolution", "Solution")
                        .WithMany("Projects")
                        .HasForeignKey("SolutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPublish", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFProcess", "Process")
                        .WithMany()
                        .HasForeignKey("ProcessId");

                    b.HasOne("IF.Manager.Contracts.Model.IFSolution", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.HasOne("IF.Manager.Contracts.Model.IFSolution", "Solution")
                        .WithMany()
                        .HasForeignKey("SolutionId");
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFQuery", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFFormModel", "FormModel")
                        .WithMany("Queries")
                        .HasForeignKey("FormModelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("IF.Manager.Contracts.Model.IFModel", "Model")
                        .WithMany("Queries")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("IF.Manager.Contracts.Model.IFProcess", "Process")
                        .WithMany("Queries")
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFQueryFilterItem", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFEntityProperty", "EntityProperty")
                        .WithMany("QueryFilterItems")
                        .HasForeignKey("EntityPropertyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("IF.Manager.Contracts.Model.IFFormModelProperty", "FormModelProperty")
                        .WithMany("QueryFilterItems")
                        .HasForeignKey("FormModelPropertyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("IF.Manager.Contracts.Model.IFEntity", null)
                        .WithMany("QueryFilterItems")
                        .HasForeignKey("IFEntityId");

                    b.HasOne("IF.Manager.Contracts.Model.IFFormModel", null)
                        .WithMany("QueryFilterItems")
                        .HasForeignKey("IFFormModelId");

                    b.HasOne("IF.Manager.Contracts.Model.IFQuery", "Query")
                        .WithMany("QueryFilterItems")
                        .HasForeignKey("QueryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFQueryOrder", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFEntityProperty", "EntityProperty")
                        .WithMany("QueryOrders")
                        .HasForeignKey("EntityPropertyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("IF.Manager.Contracts.Model.IFQuery", "Query")
                        .WithMany("QueryOrders")
                        .HasForeignKey("QueryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPage", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFPageLayout", "PageLayout")
                        .WithMany()
                        .HasForeignKey("PageLayoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IF.Manager.Contracts.Model.IFProcess", "Process")
                        .WithMany("Pages")
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPageForm", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFPageFormLayout", "FormLayout")
                        .WithMany("PageForms")
                        .HasForeignKey("FormLayoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IF.Manager.Contracts.Model.IFPageGrid", b =>
                {
                    b.HasOne("IF.Manager.Contracts.Model.IFPageGridLayout", "GridLayout")
                        .WithMany("PageGrids")
                        .HasForeignKey("GridLayoutId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("IF.Manager.Contracts.Model.IFQuery", "Query")
                        .WithMany("Grids")
                        .HasForeignKey("QueryId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
