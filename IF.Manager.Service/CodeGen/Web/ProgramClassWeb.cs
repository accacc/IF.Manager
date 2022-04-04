using IF.CodeGeneration.Language.CSharp;

using IF.Manager.Contracts.Model;

using System.Text;

namespace IF.Manager.Service
{
    public class ProgramClassWeb : CSClass
    {

        public IFProject Project { get; set; }


        public ProgramClassWeb(IFProject project)
        {
            this.Project = project;

            this.Name = $"Program";
            this.NameSpace = SolutionHelper.GetProjectNamespace(project);
            this.Usings.Add("Autofac.Extensions.DependencyInjection");
            this.Usings.Add("Microsoft.AspNetCore.Hosting");
            this.Usings.Add(SolutionHelper.GetCoreNamespace(project));
            this.Usings.Add("Microsoft.AspNetCore");
        }

        public void Build()
        {
            MainMethod();
            CreateHostBuilderMethod();

        }

        private void MainMethod()
        {
            CSMethod method = new CSMethod("Main", "void", "public");
            method.IsStatic = true;

            CsMethodParameter parameter = new CsMethodParameter();
            parameter.Name = "args";
            parameter.Type = $"string[]";

            method.Parameters.Add(parameter);

            StringBuilder methodBody = new StringBuilder();

            methodBody.AppendLine($"CreateHostBuilder(args).Build().Run();");

            method.Body = methodBody.ToString();
            this.Methods.Add(method);
        }

        private void CreateHostBuilderMethod()
        {
            CSMethod method = new CSMethod("CreateHostBuilder", "IHostBuilder", "public");
            method.IsStatic = true;

            CsMethodParameter parameter = new CsMethodParameter();
            parameter.Name = "args";
            parameter.Type = $"string[]";

            method.Parameters.Add(parameter);

            StringBuilder methodBody = new StringBuilder();

            methodBody.AppendLine($@"return Host.CreateDefaultBuilder(args)");
            methodBody.AppendLine($@".UseServiceProviderFactory(new AutofacServiceProviderFactory())");
            methodBody.AppendLine($@".ConfigureWebHostDefaults(webBuilder =>");
            methodBody.AppendLine($@"{{");
            methodBody.AppendLine($@"webBuilder.UseStartup<Startup>();");
            methodBody.AppendLine($@"}});");

            method.Body = methodBody.ToString();
            this.Methods.Add(method);
        }
    }
}
