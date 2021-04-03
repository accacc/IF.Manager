using Autofac;
using IF.Configuration;
using IF.Core.Cache;
using IF.Core.DependencyInjection.Interface;
using IF.Core.Localization;
using IF.Core.Persistence;
using IF.Cqrs.Builders;
using IF.Dependency.AutoFac;
using IF.Manager.Main.Infrastructure;
using IF.Manager.Persistence.EF;
using IF.Manager.Service;
using IF.Persistence.EF.Localization;
using IF.Persistence.EF.SqlServer.Integration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using System.Reflection;

namespace IF.Manager.Main
{
    public class Startup
    {
        InFrameworkBuilder @if;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            @if = new InFrameworkBuilder();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settings = this.Configuration.GetSettings<IFManagerAppSettings>();

            @if.AddDbContext<ManagerDbContext>(services, settings.Database.ConnectionString);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue; //not recommended value
                options.MultipartBodyLengthLimit = long.MaxValue; //not recommended value
            });

            services.AddMemoryCache();
            services.AddSession();


            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });


            services.AddRazorPages(options =>
            {

                options.Conventions.ConfigureFilter(new ExceptionPagefilterAsync());
                options.Conventions.ConfigureFilter(new NotificationPageFilterAsync());
            }
            );
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {

            @if.SetContainerBuilder(builder);

            var handlers = Assembly.Load("IF.Manager.Service");

            @if.AddCqrs(cqrs =>
            {
                cqrs.AddHandler(

                    handler =>
                    {
                        handler.AddQueryHandlers().Build(new Assembly[] { handlers });

                        handler.AddQueryAsyncHandlers().Build(new Assembly[] { handlers });

                        handler.AddCommandHandlers().Build(new Assembly[] { handlers });

                        handler.AddCommandAsyncHandlers().Build(new Assembly[] { handlers });
                    });
            })

                 //.AddLocalization(l =>
                 //{
                 //    l.AddEntityFramework(services, new LanguageMapper(), new LocalizationSettings(GetCultureInfos(), 1055, new Assembly[] { Assembly.Load("IF.Manager.Contracts") }));

                 //})
                 //.AddModule(m => m.AddDictionary(mvc))
                 //.RegisterType<PermissionRepository, IPermissionRepository>(DependencyScope.PerInstance)
                 //.RegisterType<RoleRepository, IRoleRepository>(DependencyScope.PerInstance)
                 //.Register<IMapperService>(new AutoMapperService(mapper), DependencyScope.Single)
                 .RegisterRepositories<IRepository>(new Assembly[] { Assembly.Load("IF.Manager.Service") })
                 .RegisterType<MemoryCacheService, ICacheService>(DependencyScope.Single)
                 .RegisterType<LanguageService, ILanguageService>(DependencyScope.PerInstance)
                 .RegisterType<TranslatorService, ITranslatorService>(DependencyScope.PerInstance);
            ;
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseSession();
            app.UseStaticFiles();
            app.UseCookiePolicy();           

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
        }
    }
}
