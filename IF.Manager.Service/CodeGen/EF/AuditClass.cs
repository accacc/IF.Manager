using IF.CodeGeneration.CSharp;
using IF.Core.Audit;
using IF.Core.Data;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen;

using System;
using System.Linq;
using System.Text;
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

                //if (entityProperty.IsIdentity)
                //{
                //    classProperty.Attirubites.Add("Key");
                //}

                this.Properties.Add(classProperty);
            }

        }
    }
}