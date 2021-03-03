using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;

using System;
using System.Collections.Generic;
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

        }

        public void Build()
        {
            this.AddConstructor();
            this.AddOnModelCreating();
            this.AddDbSets();

        }

        private void AddDbSets()
        {
            foreach (var entity in this.Entities)
            {
                var p = new CSProperty("public", entity.Name, false);
                p.PropertyTypeString = $"DbSet<{entity.Name}>";
                this.Properties.Add(p);
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

            StringBuilder methodBody = new StringBuilder();

            foreach (var entity in this.Entities)
            {
                methodBody.AppendLine($"builder.ApplyConfiguration(new {entity.Name}Mapping());");
            }

            method.Body = methodBody.ToString();
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
