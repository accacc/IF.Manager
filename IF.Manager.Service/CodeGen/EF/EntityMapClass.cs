using IF.CodeGeneration.Language.CSharp;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen;

using System;
using System.Linq;
using System.Text;

namespace IF.Manager.Service
{
    public class EntityMapClass : CSClass
    {

        public EntityDto EntityMetaData { get; set; }
        public EntityMapClass(EntityDto entity, IFProject project)
        {
            this.EntityMetaData = entity;
            this.NameSpace = SolutionHelper.GetCoreBaseNamespace(project);
            this.Name = $"{this.EntityMetaData.Name}Mapping";
            this.InheritedInterfaces.Add($"IEntityTypeConfiguration<{this.EntityMetaData.Name}>");
            this.Usings.Add("Microsoft.EntityFrameworkCore");
            this.Usings.Add("Microsoft.EntityFrameworkCore.Metadata.Builders");
            this.Usings.Add("IF.Core.Audit");
            this.Usings.Add(SolutionHelper.GetCoreNamespace(project));
        }

        public void Build()
        {

            CSMethod mapClassMethod = new CSMethod("Configure", "void", "public");

            CsMethodParameter parameter = new CsMethodParameter();
            parameter.Name = "builder";
            parameter.Type = $"EntityTypeBuilder<{this.EntityMetaData.Name}>";

            mapClassMethod.Parameters.Add(parameter);

            StringBuilder methodBody = new StringBuilder();

            var name = ObjectNamerHelper.RemoveLastWord(this.EntityMetaData.Name, "Entity");

            methodBody.AppendLine($"builder.ToTable(\"{name}\");");

            foreach (var item in this.EntityMetaData.Properties)
            {
                methodBody.Append($"builder.Property(x => x.{item.Name})");

                if(item.IsAutoNumber)
                {
                    methodBody.Append(".ValueGeneratedOnAdd()");
                }

                methodBody.AppendLine(";");

            }

         

            foreach (var relation in this.EntityMetaData.Relations)
            {
                if (relation.ForeignKeyPropertyId.HasValue && relation.ForeignKeyPropertyId > 0)
                {


                    switch (relation.EntityRelationType)
                    {
                        case Contracts.Enum.EntityRelationType.None:
                            break;
                        case Contracts.Enum.EntityRelationType.OneToMany:
                            methodBody.AppendLine($"builder.HasMany(s => s.{relation.RelatedEntityName}s).WithOne(s => s.{relation.EntityName}).HasForeignKey(s => s.{relation.ForeignKeyPropertyName}).OnDelete(DeleteBehavior.Restrict);");

                            break;
                        case Contracts.Enum.EntityRelationType.OneToOne:
                            methodBody.AppendLine($"builder.HasOne(s => s.{relation.RelatedEntityName}).WithOne(s => s.{relation.EntityName}).HasForeignKey<{relation.RelatedEntityName}>(s => s.{relation.ForeignKeyPropertyName});");
                            break;
                        case Contracts.Enum.EntityRelationType.ManyToMany:
                            break;
                        default:
                            break;
                    }
                }
            }

            var identities = this.EntityMetaData.Properties.Where(p => p.IsIdentity).ToList();

            if (!identities.Any()) throw new BusinessException("EntityMapClass: no identity column");

            methodBody.Append("builder.HasKey(o => new {");
            methodBody.Append(String.Join(",", identities.Select(x => $"o.{x.Name.ToString()}").ToList()));
            methodBody.AppendLine("});");


            mapClassMethod.Body = methodBody.ToString();

            this.Methods.Add(mapClassMethod);

        }


    }
}
