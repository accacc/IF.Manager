using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Model;

namespace IF.Manager.Service
{
    public class ConfigInterface : CSInterface
    {

        public TemplateAppSettings config { get; set; }
        public ConfigInterface(TemplateAppSettings config, IFProject project)
        {
            this.NameSpace = SolutionHelper.GetCoreBaseNamespace(project);
            this.config = config;
            this.Name = $"I{project.Name}AppSettings";
            this.InheritedInterfaces.Add("IAppSettingsCore");

            this.Usings.Add("IF.Configuration");
            this.Usings.Add("IF.Core.Configuration");
            this.Usings.Add("IF.Core.Database");
            this.Usings.Add("IF.Core.MongoDb");

        }

        public void Build()
        {

            if (config.Database != null)
            {
                var p = new CSProperty("", "Database", false);
                p.PropertyTypeString = "DatabaseSettings";
                this.Properties.Add(p);

            }

            if(config.MongoConnection!=null)
            {
                var p = new CSProperty("", "MongoConnection", false);
                p.PropertyTypeString = "MongoConnectionSettings";
                this.Properties.Add(p);

            }
        }


    }
}
