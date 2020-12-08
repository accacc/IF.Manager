using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
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

                CSClass cs = new CSClass();
                cs.BaseClass = nameof(Entity);
                cs.Name = entity.Name;
                cs.NameSpace = $"{solutionName}.Core";
                cs.Usings.Add("System");
                cs.Usings.Add("IF.Core.Data");
                cs.Usings.Add("System.Collections.Generic");
                cs.Usings.Add("System.ComponentModel.DataAnnotations");
                cs.Usings.Add($"{solutionName}.Core");

                foreach (var property in entity.Properties)
                {

                    bool IsNullable = property.IsNullable;
                    if(property.Type == "string")
                    {
                        IsNullable = false;
                    }


                    var p = new CSProperty("public", property.Name, IsNullable);

                    p.PropertyTypeString = property.Type;

                    if (property.IsIdentity)
                    {
                        p.Attirubites.Add("Key");
                    }                    

                    cs.Properties.Add(p);
                }

                foreach (var relation in entity.Relations)
                {
                    var relationName = relation.RelatedEntityName;
                    var type = relation.RelatedEntityName;

                    if (relation.EntityRelationType == Contracts.Enum.EntityRelationType.ManyToMany || 
                        relation.EntityRelationType ==Contracts.Enum.EntityRelationType.OneToMany)
                    {
                        relationName += "s";
                        type = $"ICollection<{relation.RelatedEntityName}>";
                    }

                    var p = new CSProperty("public", relationName, false);
                    p.PropertyTypeString = type;

                    cs.Properties.Add(p);

                }

                foreach (var relation in entity.ReverseRelations)
                {

                    var relationName = relation.RelatedEntityName;
                    var type = relation.RelatedEntityName;

                    if (relation.EntityRelationType == Contracts.Enum.EntityRelationType.ManyToMany)
                    {
                        relationName += "s";
                        type = $"ICollection<{relation.RelatedEntityName}>";
                    }

                    var p = new CSProperty("public", relationName, false);
                    p.PropertyTypeString = type;

                    cs.Properties.Add(p);

                }


                CSMethod constructorMethod = new CSMethod(cs.Name, "", "public");

                StringBuilder constructorMethodBody = new StringBuilder();

                foreach (var relation in entity.Relations)
                {

                    var relationName = relation.RelatedEntityName;
                    var type = relation.RelatedEntityName;

                    if (relation.EntityRelationType == Contracts.Enum.EntityRelationType.ManyToMany ||
                         relation.EntityRelationType == Contracts.Enum.EntityRelationType.OneToMany)
                    {
                        relationName += "s";
                        type = $"List<{relation.RelatedEntityName}>";
                    }

                    constructorMethodBody.AppendLine($"{relationName} = new {type}();");

                }

                foreach (var relation in entity.ReverseRelations)
                {

                    var relationName = relation.RelatedEntityName;
                    var type = relation.RelatedEntityName;

                    if (relation.EntityRelationType == Contracts.Enum.EntityRelationType.ManyToMany)
                    {
                        relationName += "s";
                        type = $"List<{relation.RelatedEntityName}>";
                    }

                    constructorMethodBody.AppendLine($"{relationName} = new {type}();");

                }

                constructorMethod.Body = constructorMethodBody.ToString();

                @cs.Methods.Add(constructorMethod);


                this.fileSystem.FormatCode(cs.GenerateCode(), "cs");


                EntityMapClass entityMap = new EntityMapClass(entity,project);
                entityMap.Build();

                this.fileSystem.FormatCode(entityMap.GenerateCode(), "cs");
            }

            DbContextClass dbContext = new DbContextClass(entityList,project);

            dbContext.Build();

            this.fileSystem.FormatCode(dbContext.GenerateCode(), "cs");
        }
    }
}
