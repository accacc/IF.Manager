using IF.CodeGeneration.Language.CSharp;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Service
{
    public class StartupClassWebApi : CSClass
    {

        private IFProject Project { get; set; }
        private string CoreDllName { get; set; }
        private IEntityService entityService { get; set; }

        public StartupClassWebApi(IFProject project,IEntityService entityService)
        {

            this.entityService = entityService;
            this.Project = project;
            this.CoreDllName = SolutionHelper.GetCoreNamespace(Project);


            this.Name = $"Startup";
            this.NameSpace = SolutionHelper.GetProjectNamespace(project);
            this.Usings.Add("Autofac");
            this.Usings.Add("IF.Cqrs.Builders");
            this.Usings.Add("IF.Dependency.AutoFac");
            this.Usings.Add("IF.Core.DependencyInjection.Interface");
            this.Usings.Add("IF.Configuration");
            this.Usings.Add("Microsoft.AspNetCore.Builder");
            this.Usings.Add("Microsoft.AspNetCore.Hosting");
            this.Usings.Add("Microsoft.Extensions.Configuration");
            this.Usings.Add("Microsoft.Extensions.DependencyInjection");
            this.Usings.Add("System");
            this.Usings.Add("System.Reflection");
            this.Usings.Add("IF.Persistence.EF.SqlServer.Integration");
            this.Usings.Add("Microsoft.Extensions.Hosting");
            this.Usings.Add("IF.Swagger.Integration");
            this.Usings.Add("IF.Persistence.EF.Audit");
            this.Usings.Add("IF.MongoDB.Integration");

            this.Usings.Add(SolutionHelper.GetCoreNamespace(project));
            this.Usings.Add(SolutionHelper.GetCoreBaseNamespace(project));



        }

        public async Task Build()
        {
            this.AddConstructor();
            this.ConfigureServices();
            await this.ConfigureContainer();
            this.AddProperties();
            this.Configure();

        }

        private void Configure()
        {
            CSMethod method = new CSMethod("Configure", "void", "public");

            CsMethodParameter appParameter = new CsMethodParameter();
            appParameter.Name = "app";
            appParameter.Type = $"IApplicationBuilder";
            method.Parameters.Add(appParameter);

            CsMethodParameter env = new CsMethodParameter();
            env.Name = "env";
            env.Type = $"IWebHostEnvironment";
            method.Parameters.Add(env);

            StringBuilder methodBody = new StringBuilder();

            methodBody.AppendLine($@"if (env.IsDevelopment())");
            methodBody.AppendLine($@" {{");
            methodBody.AppendLine($@"      app.UseDeveloperExceptionPage();");
            methodBody.AppendLine($@"}}");


            methodBody.AppendLine($@"@if.UseSwagger(app, ""v1"", ""{SolutionHelper.GetProjectFullName(Project)}"");");

            methodBody.AppendLine($@"app.UseRouting();");

            methodBody.AppendLine($@"app.UseAuthorization();");

            methodBody.AppendLine($@"app.UseEndpoints(endpoints =>");
            methodBody.AppendLine($@"{{");
            methodBody.AppendLine($@"endpoints.MapControllers();");
            methodBody.AppendLine($@"}});");

            method.Body = methodBody.ToString();

            this.Methods.Add(method);
        }

        private void ConfigureServices()
        {
            CSMethod method = new CSMethod("ConfigureServices", "void", "public");

            CsMethodParameter parameter = new CsMethodParameter();
            parameter.Name = "services";
            parameter.Type = $"IServiceCollection";
            method.Parameters.Add(parameter);

            StringBuilder methodBody = new StringBuilder();

            methodBody.AppendLine("this.services=services;");

            methodBody.AppendLine("services.AddControllers();");

            methodBody.AppendLine($@"var settings = this.Configuration.GetSettings<{this.Project.Name}AppSettings>();");


            methodBody.AppendLine($@"@if.AddDbContext<{this.Project.Name}DbContext>(services, settings.Database.ConnectionString,""{this.CoreDllName}"")");
            methodBody.AppendLine($@".AddSwagger(services, ""v1"", ""{SolutionHelper.GetProjectFullName(Project)}"", true)");
            methodBody.AppendLine(";");

            method.Body = methodBody.ToString();
            this.Methods.Add(method);
        }

        private void AddProperties()
        {
            var p = new CSProperty("private", @"@if", false);
            p.PropertyTypeString = $"InFrameworkBuilder";
            this.Properties.Add(p);

            var p2 = new CSProperty("public", @"Configuration", false);
            p2.PropertyTypeString = $"IConfiguration";
            this.Properties.Add(p2);

            var p3 = new CSProperty("public", @"services", false);
            p3.PropertyTypeString = $"IServiceCollection";
            this.Properties.Add(p3);
        }

        private async Task ConfigureContainer()
        {
            CSMethod method = new CSMethod("ConfigureContainer", "void", "public");

            CsMethodParameter parameter = new CsMethodParameter();
            parameter.Name = "builder";
            parameter.Type = $"ContainerBuilder";
            method.Parameters.Add(parameter);

            StringBuilder methodBody = new StringBuilder();

            methodBody.AppendLine($@"var settings = this.Configuration.GetSettings<{this.Project.Name}AppSettings>();");

            methodBody.AppendLine("@if.SetContainerBuilder(builder);");
            methodBody.AppendLine($@"var handlers = Assembly.Load(""{this.CoreDllName}"");");

            methodBody.AppendLine($@"@if.AddCqrs(cqrs =>");
            methodBody.AppendLine($@"{{");
            methodBody.AppendLine($@"cqrs.AddHandler(");

            methodBody.AppendLine($@"handler =>");
            methodBody.AppendLine($@"{{");

            methodBody.AppendLine($@"handler.AddQueryHandlers()");
            AddSystemQueryDecarators(methodBody);
            methodBody.AppendLine(".Build(new Assembly[] { handlers });");

            methodBody.AppendLine($@"handler.AddQueryAsyncHandlers()");
            AddSystemQueryDecarators(methodBody);
            methodBody.AppendLine($@".Build(new Assembly[] {{ handlers }});");

            methodBody.AppendLine($@"handler.AddCommandHandlers()");
            AddSystemCommandDecarators(methodBody);
            methodBody.AppendLine($@".Build(new Assembly[] {{ handlers }});");

            methodBody.AppendLine($@"handler.AddCommandAsyncHandlers()");
            AddSystemCommandDecarators(methodBody);
            methodBody.AppendLine($@".Build(new Assembly[] {{ handlers }});");

            methodBody.AppendLine($@"}});");
            methodBody.AppendLine($@"}})");

            if (this.Project.JsonAppType == Enum.JsonAppType.NewstonJson)
            {
                methodBody.AppendLine(@".AddNewstonJson(json => json.Build())");
            }

            if(this.Project.SystemDbType == Enum.SystemDbType.Mongo)
            {
                if(IsSystemCommandDecoratorAvailable() || IsSystemQueryDecoratorAvailable())
                {
                    methodBody.AppendLine($@".AddMongo(m => m.AddMongoPerServiceConnectionStrategy(settings.MongoConnection, c => {{");
                }

                if (this.Project.QueryAudit || this.Project.CommandAudit)
                {
                    methodBody.AppendLine(@"c.AddAuditLogger();");
                }


                if (this.Project.QueryErrorHandler || this.Project.CommandErrorHandler)
                {
                    methodBody.AppendLine(@"c.AddApplicationLogger();");
                }


                if (this.Project.QueryPerformanceCounter || this.Project.CommandPerformanceCounter)
                {
                    methodBody.AppendLine(@"c.AddPerformanceLogger();");
                }


                if (IsSystemCommandDecoratorAvailable() || IsSystemQueryDecoratorAvailable())
                {
                    methodBody.AppendLine("}");
                    methodBody.AppendLine("))");
                }

            }

            methodBody.AppendLine($@";");


            var shadowAuditEntities = await this.entityService.GetShadowAuditEntityList();

            foreach (var item in shadowAuditEntities)
            {
                // ShadowAuditing.RegisterAuditType<ProductEntity, ProductEntityShadowAudit, int>(c => c.Id);
                //TODO:Caglar multiple identity icinde çözüm sunmak lazim
                var identity = item.Properties.Where(p => p.IsIdentity).FirstOrDefault();

                methodBody.AppendLine($@"ShadowAuditing.RegisterAuditType<{item.Name}, {item.Name}ShadowAudit, {identity.Type}>(c => c.{identity.Name});");
            }

            method.Body = methodBody.ToString();
            this.Methods.Add(method);


        }

        private void AddSystemQueryDecarators(StringBuilder methodBody)
        {
            if(IsSystemQueryDecoratorAvailable())
            {
                methodBody.AppendLine(".Decoration(d =>");
            }

            if (this.Project.QueryAudit)
            {
                methodBody.AppendLine(@".AddAuditing()");
            }


            if (this.Project.QueryErrorHandler)
            {
                methodBody.AppendLine(@".AddErrorLogging()");
            }


            if (this.Project.QueryPerformanceCounter)
            {
                methodBody.AppendLine(@".AddPerformanceCounter()");
            }

            if (IsSystemQueryDecoratorAvailable())
            {
                methodBody.AppendLine(")");
            }
        }

        private bool IsSystemQueryDecoratorAvailable()
        {
            if (this.Project.QueryAudit || this.Project.QueryErrorHandler || this.Project.QueryPerformanceCounter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsSystemCommandDecoratorAvailable()
        {
            if (this.Project.CommandAudit || this.Project.CommandErrorHandler || this.Project.CommandPerformanceCounter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void AddSystemCommandDecarators(StringBuilder methodBody)
        {
            if (IsSystemCommandDecoratorAvailable())
            {
                methodBody.AppendLine(".Decoration(d =>");
            }

            if (this.Project.CommandAudit)
            {
                methodBody.AppendLine(@".AddAuditing()");
            }


            if (this.Project.CommandErrorHandler)
            {
                methodBody.AppendLine(@".AddErrorLogging()");
            }


            if (this.Project.CommandPerformanceCounter)
            {
                methodBody.AppendLine(@".AddPerformanceCounter()");
            }

            if (IsSystemCommandDecoratorAvailable())
            {
                methodBody.AppendLine(")");
            }
        }

        private void AddConstructor()
        {
            CSMethod consMethod = new CSMethod(this.Name, "", "public");
            consMethod.IsConstructor = true;

            CsMethodParameter parameter = new CsMethodParameter();
            parameter.Name = "configuration";
            parameter.Type = $"IConfiguration";
            consMethod.Parameters.Add(parameter);


            StringBuilder methodBody = new StringBuilder();

            methodBody.AppendLine("Configuration = configuration;");
            methodBody.AppendLine("@if = new InFrameworkBuilder();");


            consMethod.Body = methodBody.ToString();

            this.Methods.Add(consMethod);
        }
    }
}
