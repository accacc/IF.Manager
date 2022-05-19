using IF.CodeGeneration.Core;
using IF.CodeGeneration.Language.CSharp;
using IF.Core.Audit;
using IF.Core.Data;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.CodeGen.EF;
using IF.Persistence.EF.Audit;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IF.Manager.Service
{
    class DbContextGenerator
    {

        private readonly IEntityService entityService;
        private readonly FileSystemCodeFormatProvider fileSystem;

        public DbContextGenerator(IEntityService entityService, FileSystemCodeFormatProvider fileSystem)
        {
            this.entityService = entityService;
            this.fileSystem = fileSystem;
        }

        public void Generate(List<EntityDto> entityList, IFProject project)
        {
            string solutionName = project.Solution.SolutionName;


            if (entityList.Any(e => e.AuditType == Enum.IFAuditType.Bulk))
            {
                EntityDto auditEntity = new EntityDto();
                auditEntity.Name = "AuditLog";
                auditEntity.Properties = new List<EntityPropertyDto>();

                auditEntity.Properties.Add(new EntityPropertyDto { Name = "Id", Type = "int" ,IsIdentity = true});
                auditEntity.Properties.Add(new EntityPropertyDto { Name = "CreatedBy", Type = "string" });
                auditEntity.Properties.Add(new EntityPropertyDto { Name = "ModifiedBy", Type = "string" });
                auditEntity.Properties.Add(new EntityPropertyDto { Name = "Type", Type = "string" });
                auditEntity.Properties.Add(new EntityPropertyDto { Name = "TableName", Type = "string" });
                auditEntity.Properties.Add(new EntityPropertyDto { Name = "Modified", Type = "DateTime?" });
                auditEntity.Properties.Add(new EntityPropertyDto { Name = "Created", Type = "DateTime" });
                auditEntity.Properties.Add(new EntityPropertyDto { Name = "OldValues", Type = "string" });
                auditEntity.Properties.Add(new EntityPropertyDto { Name = "NewValues", Type = "string" });
                auditEntity.Properties.Add(new EntityPropertyDto { Name = "AffectedColumns", Type = "string" });
                auditEntity.Properties.Add(new EntityPropertyDto { Name = "PrimaryKey", Type = "string" });
                
                EntityMapClass auditEntityMap = new EntityMapClass(auditEntity, project);

                auditEntityMap.Build();

                this.fileSystem.FormatCode(auditEntityMap.GenerateCode(), "cs", "", "");
            }

            foreach (var entity in entityList)
            {
                CSClass entityClass = new CSClass();

                entityClass.BaseClass = nameof(Entity);

                if(entity.IsSoftDeleted)
                {
                    entityClass.InheritedInterfaces.Add(nameof(ISoftDelete));

                    var softDeleteProperty = new CSProperty("public", "SoftDeleted", false);

                    softDeleteProperty.PropertyTypeString = "bool";

                    entityClass.Properties.Add(softDeleteProperty);
                }

                switch (entity.AuditType)
                {
                    case Enum.IFAuditType.Shadow:
                        
                        
                        entityClass.Usings.Add("IF.Core.Audit");
                        entityClass.InheritedInterfaces.Add(nameof(IShadowAuditableEntity));
                        entityClass.Usings.Add("System.ComponentModel.DataAnnotations.Schema");

                        AuditClass auditClass = new AuditClass(entity, project);
                        auditClass.Build();


                        this.fileSystem.FormatCode(auditClass.GenerateCode(), "cs", "", "");

                        var uniqueIdProperty = new CSProperty("public", "UniqueId", false);

                        uniqueIdProperty.PropertyTypeString = "Guid";

                        uniqueIdProperty.Attirubites.Add("NotMapped");

                        entityClass.Properties.Add(uniqueIdProperty);

                        break;
                    case Enum.IFAuditType.Bulk:
                        entityClass.Usings.Add("IF.Core.Audit");
                        entityClass.InheritedInterfaces.Add(nameof(ISimpleAuditableEntity));
                        break;

                    case Enum.IFAuditType.None:
                        break;
                    default:
                        break;
                }

                entityClass.Name = entity.Name;
                entityClass.NameSpace = $"{solutionName}.Core";
                entityClass.Usings.Add("System");
                entityClass.Usings.Add("IF.Core.Data");
                entityClass.Usings.Add("System.Collections.Generic");
                entityClass.Usings.Add("System.ComponentModel.DataAnnotations");
                entityClass.Usings.Add($"{solutionName}.Core");

                GenerateProperties(entity, entityClass);

                CSMethod constructorMethod = GenerateConstructorMethod(entity, entityClass);

                entityClass.Methods.Add(constructorMethod);


                this.fileSystem.FormatCode(entityClass.GenerateCode(), "cs","","");

                EntityMapClass entityMap = new EntityMapClass(entity, project);

                entityMap.Build();

                this.fileSystem.FormatCode(entityMap.GenerateCode(), "cs","","");
            }

            DbContextClass dbContext = new DbContextClass(entityList, project);

            dbContext.Build();

            this.fileSystem.FormatCode(dbContext.GenerateCode(), "cs","","");
        }

        private static void GenerateProperties(EntityDto entity, CSClass entityClass)
        {
            foreach (var entityProperty in entity.Properties)
            {

                bool IsNullable = entityProperty.IsNullable;

                if (entityProperty.Type == "string")
                {
                    IsNullable = false;
                }


                var classProperty = new CSProperty("public", entityProperty.Name, IsNullable);

                classProperty.PropertyTypeString = entityProperty.Type;

                entityClass.Properties.Add(classProperty);
            }

            foreach (var relation in entity.Relations)
            {
                var relationName = relation.RelatedEntityName;
                var type = relation.RelatedEntityName;

                if (relation.EntityRelationType == Contracts.Enum.EntityRelationType.ManyToMany ||
                    relation.EntityRelationType == Contracts.Enum.EntityRelationType.OneToMany)
                {
                    relationName += "s";

                    type = $"ICollection<{relation.RelatedEntityName}>";
                }


                var classProperty = new CSProperty("public", relationName, false);

                classProperty.PropertyTypeString = type;

                entityClass.Properties.Add(classProperty);

            }

            

            foreach (var relation in entity.ReverseRelations)
            {
                if (relation.EntityRelationType == Contracts.Enum.EntityRelationType.OneToOne) continue;

                var relationName = relation.RelatedEntityName;
                var type = relation.RelatedEntityName;

                if (relation.EntityRelationType == Contracts.Enum.EntityRelationType.ManyToMany)
                {
                    relationName += "s";
                    type = $"ICollection<{relation.RelatedEntityName}>";
                }

                var property = new CSProperty("public", relationName, false);

                property.PropertyTypeString = type;

                entityClass.Properties.Add(property);

            }
        }

        private static CSMethod GenerateConstructorMethod(EntityDto entity, CSClass cs)
        {
            CSMethod constructorMethod = new CSMethod(cs.Name, "", "public");

            StringBuilder constructorMethodBody = new StringBuilder();

            foreach (var relation in entity.Relations)
            {

                var relationName = relation.RelatedEntityName;

                if (relation.EntityRelationType == Contracts.Enum.EntityRelationType.ManyToMany ||
                     relation.EntityRelationType == Contracts.Enum.EntityRelationType.OneToMany)
                {
                    relationName += "s";
                    string type = $"List<{relation.RelatedEntityName}>";
                    constructorMethodBody.AppendLine($"{relationName} = new {type}();");
                }              

            }

            foreach (var relation in entity.ReverseRelations)
            {

                if (relation.EntityRelationType == Contracts.Enum.EntityRelationType.OneToOne) continue;

                var relationName = relation.RelatedEntityName;


                if (relation.EntityRelationType == Contracts.Enum.EntityRelationType.ManyToMany)
                {
                    relationName += "s";
                    string type = $"List<{relation.RelatedEntityName}>";
                    constructorMethodBody.AppendLine($"{relationName} = new {type}();");
                }
            }


            switch (entity.AuditType)
            {
                case Enum.IFAuditType.Shadow:
                    constructorMethodBody.AppendLine("this.UniqueId = Guid.NewGuid();");
                    break;
                case Enum.IFAuditType.Bulk:
                   
                    break;

                case Enum.IFAuditType.None:
                    break;
                default:
                    break;
            }

            constructorMethod.Body = constructorMethodBody.ToString();

            return constructorMethod;
        }
    }
}
