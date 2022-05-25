using IF.CodeGeneration.Language.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IF.Manager.Service
{
    public class DbContextClass : CSClass
    {

        public List<EntityDto> Entities { get; set; }

        public DbContextClass(List<EntityDto> entities, IFProject project)
        {
            this.Entities = entities;
            this.NameSpace = SolutionHelper.GetCoreBaseNamespace(project);
            this.Name = $"{project.Name}DbContext";
            this.BaseClass = "DbContext";
            this.Usings.Add(SolutionHelper.GetCoreNamespace(project));
            this.Usings.Add("Microsoft.EntityFrameworkCore");
            this.Usings.Add("IF.Persistence.EF");

        }

        public void Build()
        {
            this.AddConstructor();
            this.AddOnModelCreating();
            this.AddDbSets();

        }

        private void AddDbSets()
        {

            if(this.Entities.Any(e=>e.AuditType == Enum.IFAuditType.Bulk))
            {
                var auditEntityProperty = new CSProperty("public", "AuditLog", false);
                auditEntityProperty.PropertyTypeString = "DbSet<AuditLog>";
                this.Properties.Add(auditEntityProperty);
                this.Usings.Add("IF.Core.Audit");
            }

            foreach (var entity in this.Entities)
            {
                var p = new CSProperty("public", entity.Name, false);
                p.PropertyTypeString = $"DbSet<{entity.Name}>";
                this.Properties.Add(p);


                switch (entity.AuditType)
                {
                    case Enum.IFAuditType.Shadow:

                        var auditEntityProperty = new CSProperty("public", entity.Name + "ShadowAudit", false);
                        auditEntityProperty.PropertyTypeString = $"DbSet<{entity.Name}ShadowAudit>";
                        this.Properties.Add(auditEntityProperty);


                        break;
                    case Enum.IFAuditType.Bulk:
                        break;
                    case Enum.IFAuditType.Self:
                        //ISelfAuditableEntity
                        break;
                    case Enum.IFAuditType.None:
                        break;
                    default:
                        break;
                }
            }
        }

        private void AddOnModelCreating()
        {
            CSMethod method = new CSMethod("OnModelCreating", "void", "protected");
            method.IsOvveride = true;
            CsMethodParameter parameter = new CsMethodParameter();
            parameter.Name = "builder";
            parameter.Type = $"ModelBuilder";
            method.Parameters.Add(parameter);

            StringBuilder methodBodyBuilder = new StringBuilder();

            foreach (var entity in this.Entities)
            {
                methodBodyBuilder.AppendLine($"builder.ApplyConfiguration(new {entity.Name}Mapping());");
            }

            methodBodyBuilder.AppendLine("builder.SoftDeleteEnabled();");


            method.Body = methodBodyBuilder.ToString();
            this.Methods.Add(method);
        }

        private void AddConstructor()
        {
            CSMethod consMethod = new CSMethod(this.Name, "", "public");
            consMethod.IsConstructor = true;

            CsMethodParameter parameter = new CsMethodParameter();
            parameter.Name = "options";
            parameter.UseBase = true;
            parameter.Type = $"DbContextOptions<{this.Name}>";

            consMethod.Parameters.Add(parameter);
            this.Methods.Add(consMethod);
        }
    }
}
