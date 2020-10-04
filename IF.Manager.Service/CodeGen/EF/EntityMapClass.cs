using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System.Text;

namespace IF.Manager.Service
{
    public class EntityMapClass: CSClass
    {

        public EntityDto EntityMetaData { get; set; }
        public EntityMapClass(EntityDto entity,IFProject project)
        {
            this.EntityMetaData = entity;
            this.NameSpace = SolutionHelper.GetCoreBaseNamespace(project);
            this.Name = $"{this.EntityMetaData.Name}Mapping";
            this.InheritedInterfaces.Add($"IEntityTypeConfiguration<{this.EntityMetaData.Name}>");
            this.Usings.Add("Microsoft.EntityFrameworkCore");
            this.Usings.Add("Microsoft.EntityFrameworkCore.Metadata.Builders");
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

            var name = DirectoryHelper.RemoveLastWord(this.EntityMetaData.Name, "Entity");

            methodBody.AppendLine($"builder.ToTable(\"{name}\");");

            foreach (var item in this.EntityMetaData.Properties)
            {
                methodBody.AppendLine($"builder.Property(x => x.{item.Name});");
            }

            mapClassMethod.Body = methodBody.ToString();
            this.Methods.Add(mapClassMethod);

        }


    }
}
