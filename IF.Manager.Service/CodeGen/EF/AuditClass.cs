using IF.CodeGeneration.Language.CSharp;
using IF.Core.Audit;
using IF.Core.Data;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Persistence.EF.Audit;
namespace IF.Manager.Service.CodeGen.EF
{
    public class AuditClass : CSClass
    {

        public EntityDto EntityMetaData { get; set; }
        public AuditClass(EntityDto entity, IFProject project)
        {
            this.EntityMetaData = entity;
            this.NameSpace = SolutionHelper.GetCoreBaseNamespace(project);
            this.Name = $"{this.EntityMetaData.Name}ShadowAudit";
            this.BaseClass = nameof(Entity);
            this.InheritedInterfaces.Add(nameof(IShadowAuditEntity));
            this.Usings.Add("Microsoft.EntityFrameworkCore");
            this.Usings.Add("IF.Core.Data");
            this.Usings.Add("System");
            this.Usings.Add("IF.Core.Audit");
            this.Usings.Add(SolutionHelper.GetCoreNamespace(project));
        }

        public void Build()
        {
            foreach (var entityProperty in this.EntityMetaData.Properties)
            {

                bool IsNullable = entityProperty.IsNullable;

                if (entityProperty.Type == "string")
                {
                    IsNullable = false;
                }


                var classProperty = new CSProperty("public", entityProperty.Name, IsNullable);

                classProperty.PropertyTypeString = entityProperty.Type;               

                this.Properties.Add(classProperty);             
            }

            var CreatedColumnNameProperty = new CSProperty("public", ShadowAuditing.CreatedColumnName, false);
            CreatedColumnNameProperty.PropertyTypeString = "DateTime";
            this.Properties.Add(CreatedColumnNameProperty);


            var CreatedByColumnNameProperty = new CSProperty("public", ShadowAuditing.CreatedByColumnName, false);
            CreatedByColumnNameProperty.PropertyTypeString = "string";
            this.Properties.Add(CreatedByColumnNameProperty);

            var LogTypeColumnNameProperty = new CSProperty("public", ShadowAuditing.LogTypeColumnName, false);
            LogTypeColumnNameProperty.PropertyTypeString = "int";
            this.Properties.Add(LogTypeColumnNameProperty);

            var ModifiedColumnNameProperty = new CSProperty("public", ShadowAuditing.ModifiedColumnName, false);
            ModifiedColumnNameProperty.PropertyTypeString = "DateTime?";
            this.Properties.Add(ModifiedColumnNameProperty);


            var ModifiedByColumnNameProperty = new CSProperty("public", ShadowAuditing.ModifiedByColumnName, false);
            ModifiedByColumnNameProperty.PropertyTypeString = "string";
            this.Properties.Add(ModifiedByColumnNameProperty);



            var LogDateColumnNameProperty = new CSProperty("public", ShadowAuditing.LogDateColumnName, false);
            LogDateColumnNameProperty.PropertyTypeString = "DateTime";
            this.Properties.Add(LogDateColumnNameProperty);


            var ObjectIdColumnNameProperty = new CSProperty("public", ShadowAuditing.ObjectIdColumnName, false);
            ObjectIdColumnNameProperty.PropertyTypeString = "string";
            this.Properties.Add(ObjectIdColumnNameProperty);


            var ChannelColumnNameProperty = new CSProperty("public", ShadowAuditing.ChannelColumnName, false);
            ChannelColumnNameProperty.PropertyTypeString = "string";
            this.Properties.Add(ChannelColumnNameProperty);

        }
    }
}