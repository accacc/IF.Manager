using IF.Manager.Contracts.Model;
using IF.Manager.Persistence.EF.Mappings;
using IF.Manager.Service.Mappings;
using IF.Manager.Service.Model;

using Microsoft.EntityFrameworkCore;
using System;

namespace IF.Manager.Persistence.EF
{
    public class ManagerDbContext : DbContext
    {
        public ManagerDbContext(DbContextOptions<ManagerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<IFEntity>().HasMany(t => t.Relations)
        .WithOne(g => g.Entity)
        .HasForeignKey(g => g.EntityId);

            builder.Entity<IFEntity>().HasMany(t => t.ReverseRelations)
                .WithOne(g => g.Relation)
                .HasForeignKey(g => g.RelationId).OnDelete(DeleteBehavior.Restrict);


            builder.ApplyConfiguration(new IFEntityMapping());
            builder.ApplyConfiguration(new IFEntityPropertyMapping());
            builder.ApplyConfiguration(new IFLanguageEntityMapping());
            builder.ApplyConfiguration(new IFEntityGroupMapping());
            builder.ApplyConfiguration(new IFProjectMapping());
            builder.ApplyConfiguration(new IFSolutionMapping());
            builder.ApplyConfiguration(new IFProcessMapping());
            builder.ApplyConfiguration(new IFModelPropertyMapping());
            builder.ApplyConfiguration(new IFModelMapping());
            builder.ApplyConfiguration(new IFPublishMapping());

            builder.ApplyConfiguration(new IFQueryMapping());
            builder.ApplyConfiguration(new IFQueryOrderMapping());
            builder.ApplyConfiguration(new IFQueryFilterItemMapping());

            builder.ApplyConfiguration(new IFCommandMapping());            
            builder.ApplyConfiguration(new IFCommandFilterItemMapping());



            builder.ApplyConfiguration(new IFPageMapping());
            builder.ApplyConfiguration(new IFPageGridMapping());
            builder.ApplyConfiguration(new IFPageFormMapping());
            builder.ApplyConfiguration(new IFPageFormLayoutMapping());
            builder.ApplyConfiguration(new IFPageGridLayoutMapping());
            builder.ApplyConfiguration(new IFPageActionMapping());
            builder.ApplyConfiguration(new IFPageLayoutMapping());
            builder.ApplyConfiguration(new IFPageControlMapMapping());
            builder.ApplyConfiguration(new IFPageControlMapping());

            builder.ApplyConfiguration(new IFPageFormItemMapping());
            builder.ApplyConfiguration(new IFPageControlItemModelPropertyMapping());

            builder.ApplyConfiguration(new IFPageNavigationMapping());

            builder.ApplyConfiguration(new IFPageParameterBaseMapping());
            builder.ApplyConfiguration(new IFPageActionRouteValueMapping());
            builder.ApplyConfiguration(new IFPageListViewMapping());
            builder.ApplyConfiguration(new IFPagePanelMapping());

            builder.ApplyConfiguration(new IFPageParameterMapping());

            builder.ApplyConfiguration(new IFCustomClassMapping());

            builder.ApplyConfiguration(new IFCustomClassPropertyMapping());







        }

        public DbSet<IFEntity> Entities { get; set; }
        public DbSet<IFEntityProperty> EntityProperties { get; set; }
        public DbSet<IFLanguageEntity> LanguageEntities { get; set; }

        public DbSet<IFEntityGroup> Groups { get; set; }

        public DbSet<IFProject> Projects { get; set; }

        public DbSet<IFSolution> Solutions { get; set; }

        public DbSet<IFProcess>  Processes { get; set; }

        public DbSet<IFPublish> Publishes { get; set; }
        public DbSet<IFModel> Models { get; set; }
        
        public DbSet<IFModelProperty> ModelProperties { get; set; }

        

        public DbSet<IFQuery> Queries { get; set; }
        public DbSet<IFQueryOrder> QueryOrders { get; set; }

        
        public DbSet<IFQueryFilterItem> QueryFilterItems { get; set; }

        public DbSet<IFCommand> Commands { get; set; }
        public DbSet<IFCommandFilterItem> CommandFilterItems { get; set; }
        public DbSet<IFPage> Pages { get; set; }
        public DbSet<IFPageGrid> PageGrids { get; set; }
        
        public DbSet<IFPageAction> PageActions { get; set; }
        public DbSet<IFPageGridLayout> GridLayouts { get; set; }

        public DbSet<IFPageLayout> Layouts { get; set; }

        public DbSet<IFPageControlMap>  PageControlMaps { get; set; }

        public DbSet<IFPageControl> PageControls { get; set; }


        public DbSet<IFPageForm> PageForms { get; set; }
        public DbSet<IFPageFormLayout> PageFormLayouts { get; set; }

        public DbSet<IFPageFormItem> PageFormItems { get; set; }

        public DbSet<IFPageNavigation>  PageNavigations { get; set; }

        public DbSet<IFPageParameter> PageParameters { get; set; }

        public DbSet<IFPageControlItemModelProperty>   IFPageFormItemModelProperties   { get; set; }

        public DbSet<IFPageControlItemModelProperty> PageFormItemModelProperties { get; set; }

        public DbSet<IFPageActionRouteValue> PageActionRouteValues { get; set; }

        public DbSet<IFPageListView>  PageListViews { get; set; }

        public DbSet<IFPagePanel> Panels { get; set; }

        public DbSet<IFPageParameter>  IFPageParameters { get; set; }

        public DbSet<IFCustomClass> IFCustomClasses { get; set; }

        public DbSet<IFCustomClassProperty> IFCustomClassProperties { get; set; }


    }
}