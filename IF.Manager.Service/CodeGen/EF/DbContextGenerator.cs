using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Core.Audit;
using IF.Core.Data;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;

using System;
using System.Collections.Generic;
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
                        entityClass.InheritedInterfaces.Add(nameof(IShadowAuditableEntity));
                        break;
                    case Enum.IFAuditType.Bulk:
                        break;
                        entityClass.InheritedInterfaces.Add(nameof(IBulkAuditableEntity));
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

                //if (entityProperty.IsIdentity)
                //{
                //    classProperty.Attirubites.Add("Key");
                //}

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

            constructorMethod.Body = constructorMethodBody.ToString();

            return constructorMethod;
        }
    }
}
