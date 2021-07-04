using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Model;

namespace IF.Manager.Service
{
    public class ConfigClass : CSClass
    {

        public TemplateAppSettings config { get; set; }
        public ConfigClass(TemplateAppSettings config, IFProject project)
        {
            this.config = config;
            this.NameSpace = SolutionHelper.GetCoreBaseNamespace(project);
            this.Name = $"{project.Name}AppSettings";
            this.BaseClass = "AppSettingsCore";
            this.InheritedInterfaces.Add($"I{project.Name}AppSettings");
            this.Usings.Add("IF.Configuration");
            this.Usings.Add("IF.Core.Configuration");
            this.Usings.Add("IF.Core.Database");
            this.Usings.Add("IF.Core.MongoDb");
        }

        public void Build()
        {

            if (config.Database != null)
            {
                var p = new CSProperty("public", "Database", false);
                p.PropertyTypeString = "DatabaseSettings";
                this.Properties.Add(p);

            }

            if (config.MongoConnection != null)
            {
                var p = new CSProperty("public", "MongoConnection", false);
                p.PropertyTypeString = "MongoConnectionSettings";
                this.Properties.Add(p);

            }
        }


    }
}
