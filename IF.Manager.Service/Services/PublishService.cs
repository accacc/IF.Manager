using IF.CodeGeneration.Core;
using IF.Core.Data;
using IF.Core.Navigation;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.Web.Page;
using IF.Manager.Service.Web.Page.Form;
using IF.Manager.Service.Web.Page.ListView;
using IF.Manager.Service.Web.Page.Panel;
using IF.Manager.Service.WebApi;
using IF.Persistence.EF;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Service.Services
{
    public class PublishService : GenericRepository, IPublishService
    {

        private readonly IPageService pageService;
        private readonly VsHelper vsHelper;
        private readonly IPageGridService pageGridService;
        private readonly IProjectService projectService;
        private readonly IEntityService entityService;
        private readonly IClassService classService;
        private readonly FileSystemCodeFormatProvider fileSystem;
        public PublishService(ManagerDbContext dbContext, IClassService classService, IEntityService entityService, IPageService pageService, IPageGridService pageGridService, IProjectService projectService) : base(dbContext)
        {
            this.pageService = pageService;
            this.pageGridService = pageGridService;
            this.projectService = projectService;
            this.entityService = entityService;
            this.fileSystem = new FileSystemCodeFormatProvider(DirectoryHelper.GetTempGeneratedDirectoryName());
            this.classService = classService;
            this.vsHelper = new VsHelper(DirectoryHelper.GetTempGeneratedDirectoryName());
        }

        public Task<List<PublishDto>> GetMenuPublishHistory()
        {
            throw new NotImplementedException();
        }

        public async Task AddPublish(PublishDto form)
        {
            IFPublish entity = new IFPublish();
            entity.Name = form.Name;
            entity.Description = form.Description;
            entity.ProcessId = form.ProcessId;
            entity.SolutionId = form.SolutionId;
            entity.ProjectId = form.ProjectId;
            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();
        }



        public async Task<List<PublishDto>> GetPublishList(int processId)
        {
            var data = await this.GetQuery<IFPublish>(p => p.ProcessId == processId)
                                .Select(x => new PublishDto
                                {

                                    Name = x.Name,
                                    Description = x.Description
                                }).ToListAsync();

            return data;
        }


        public async Task PublishSolution(PublishDto form)
        {
            var solution = await this.projectService.GetSolution(form.SolutionId);

            var solutionName = solution.SolutionName;

            this.vsHelper.GenerateSolution(solutionName);

            DirectoryHelper.CopyProject(solutionName, solution.Path);

            this.vsHelper.ExploreSolution(solution.Path, solutionName);
        }

        public async Task PublishProject(PublishDto publish)
        {
            try
            {
                DirectoryHelper.CreateDirectory(DirectoryHelper.GetTempGeneratedDirectoryName(), true);

                DirectoryHelper.CreateDirectory(DirectoryHelper.GetBaseTempDirectoryName(), true);


                IFProject project = await this.projectService.GetProject(publish.ProjectId);

                if (project.ProjectType == Contracts.Enum.ProjectType.Api)
                {
                    await this.PublishApiProject(publish.ProjectId);
                }
                else if (project.ProjectType == Contracts.Enum.ProjectType.Web)
                {
                    await this.PublishWebProject(publish.ProjectId);
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }


        }

        public async Task PublishWebProject(int projectId)
        {
            var project = await this.GetQuery<IFProject>(p => p.Id == projectId)
                .Include(s => s.Solution)
                .SingleOrDefaultAsync();

            string projectName = project.Name;

            string solutionName = project.Solution.SolutionName;

            this.vsHelper.GenerateProject(projectName, project.ProjectType, solutionName, project.Solution.Path);

            DirectoryHelper.CopyProject(solutionName, project.Solution.Path);

            this.vsHelper.ExploreSolution(project.Solution.Path, solutionName);
        }

        public async Task PublishApiProject(int projectId)
        {
            var project = await this.GetQuery<IFProject>(p => p.Id == projectId)
                .Include(s => s.Solution)
                .SingleOrDefaultAsync();

            string projectName = project.Name;

            string solutionName = project.Solution.SolutionName;

            var entityList = await this.entityService.GetEntityList();


            DbContextGenerator dbContextGenerator = new DbContextGenerator(entityService, fileSystem);
            dbContextGenerator.Generate(entityList, project);

            ProgramClassWebApi program = new ProgramClassWebApi(project);
            program.Build();
            fileSystem.FormatCode(program.GenerateCode(), "cs");


            StartupClassWebApi startup = new StartupClassWebApi(project);
            startup.Build();
            fileSystem.FormatCode(startup.GenerateCode(), "cs");

            GenerateConfigClassAndJson(project);


            this.vsHelper.GenerateProject(projectName, project.ProjectType, solutionName, project.Solution.Path);
            this.vsHelper.AddClassToProject(project.ProjectType, projectName, "Startup", solutionName);
            this.vsHelper.AddClassToProject(project.ProjectType, projectName, "Program", solutionName);
            this.vsHelper.AddConfigJsonFileToApiProject(project.ProjectType, projectName, solutionName);

            //CoreDllGenerator coreDllGenerator = new CoreDllGenerator();
            //coreDllGenerator.GenerateCoreBaseDll(solutionName, DirectoryHelper.GetTempGeneratedDirectoryName());

            var files = Directory.GetFiles(DirectoryHelper.GetTempGeneratedDirectoryName(), "*.cs", SearchOption.AllDirectories).Where(s => s.EndsWith(".cs") || s.EndsWith(".cs")).ToArray();

            string targetPath = DirectoryHelper.GetNewCoreProjectDirectory(project);
            string fileName = string.Empty;
            string destFile = string.Empty;

            foreach (var s in files)
            {
                fileName = System.IO.Path.GetFileName(s);
                destFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Copy(s, destFile, true);
            }

            //this.vsHelper.AddDllReferenceToApiProject($"{solutionName}.Core.Base", solutionName, project.Solution.Path);
            //this.vsHelper.AddDllReferenceToCoreProject($"{solutionName}.Core.Base", solutionName, project.Solution.Path);

            DirectoryHelper.CopyProject(solutionName, project.Solution.Path);

            this.vsHelper.ExploreSolution(project.Solution.Path, solutionName);

        }

        private void GenerateConfigClassAndJson(IFProject project)
        {
            TemplateAppSettings settings = new TemplateAppSettings();
            settings.Database = new IF.Core.Database.DatabaseSettings();
            settings.Database.ConnectionString = project.ConnectionString;

            settings.ApplicationName = project.Name;
            settings.Version = "1.0.0";
            settings.RabbitMQConnection = new IF.Core.RabbitMQ.RabbitMQConnectionSettings();
            settings.RabbitMQConnection.EventBusConnection = "";


            ConfigGenerator configGenerator = new ConfigGenerator();
            configGenerator.Generate(project.Name, settings, DirectoryHelper.GetTempGeneratedDirectoryName());


            ConfigInterface configInterface = new ConfigInterface(settings, project);
            configInterface.Build();
            fileSystem.FormatCode(configInterface.GenerateCode(), "cs");

            ConfigClass configClass = new ConfigClass(settings, project);
            configClass.Build();
            fileSystem.FormatCode(configClass.GenerateCode(), "cs");
        }

        public async Task PublishPageTree(PublishDto form)
        {
            var page = await this.pageService.GetPageTree(form.PageId);

            PageControl pageControl = new PageControl(page);

            pageControl.Generate();

            Render(page.Childrens);

            DirectoryHelper.MoveDirectory(DirectoryHelper.GetTempPageDirectory(pageControl.PageControlMap), DirectoryHelper.GetNewPageDirectory(pageControl.PageControlMap));

        }



        public void Render(ICollection<IFPageControlMap> pageControlMaps)
        {
            foreach (var pageControlMap in pageControlMaps)
            {
                if (pageControlMap.IFPageControl is IFPage)
                {
                    PageControl pageControl = new PageControl(pageControlMap);
                    pageControl.Generate();
                    //DirectoryHelper.MoveDirectory(DirectoryHelper.GetTempPageDirectory(pageControl), DirectoryHelper.GetNewPageDirectory(pageControl));
                }

                else if (pageControlMap.IFPageControl is IFPageGrid)
                {
                    PageGridViewControl gridControl = new PageGridViewControl(pageControlMap);
                    gridControl.Generate();

                }
                else if (pageControlMap.IFPageControl is IFPageAction)
                {
                    //IFPageAction actionControl = new IFPageAction(pageControlMap);
                    //actionControl.Render();
                }
                else if (pageControlMap.IFPageControl is IFPageForm)
                {
                    PageFormControl formControl = new PageFormControl(pageControlMap);
                    formControl.Generate();
                }
                else if (pageControlMap.IFPageControl is IFPageListView)
                {
                    PageListViewControl listViewControl = new PageListViewControl(pageControlMap);
                    listViewControl.Generate();
                }
                else if (pageControlMap.IFPageControl is IFPagePanel)
                {
                    PagePanelControl panel = new PagePanelControl(pageControlMap);
                    panel.Generate();
                }

                if (pageControlMap.Childrens != null && pageControlMap.Childrens.Any())
                {
                    Render(pageControlMap.Childrens);
                }
            }
        }


        public async Task PublishMenu(PublishDto form)
        {
            var tree = await this.pageService.GetPageControlMapMenuList(form.ProjectId);

            List<NavigationDto> list = new List<NavigationDto>();

            foreach (var item in tree)
            {
                NavigationDto dto = new NavigationDto();
                dto.Id = item.Id;
                dto.ControlType = item.PageControl.ControlType;
                dto.Name = item.PageControl.Name;
                dto.PageControlId = item.PageControlId;
                dto.ParentId = item.ParentId;
                dto.SortOrder = item.SortOrder;
                if (item.PageControl is IFPage)
                {
                    string path = item.PageControl.IFPageControlMap.GetPagePath();
                    dto.Description = $"{path}/{item.Name}";
                }
                list.Add(dto);
            }

            list = list.ToTree();

            var config = JsonConvert.SerializeObject(list,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            File.WriteAllText($"{DirectoryHelper.GetTempGeneratedDirectoryName()}/menu.json", config);

            IFProject project = await this.projectService.GetProject(form.ProjectId);

            this.vsHelper.AddMenuJsonFileToApiProject(project);

        }

        public async Task PublishProcess(PublishDto publish)
        {

            try
            {
                DirectoryHelper.CreateDirectory(DirectoryHelper.GetTempGeneratedDirectoryName(), true);
                DirectoryHelper.CreateDirectory(DirectoryHelper.GetBaseTempDirectoryName(), true);

                var process = await this.GetQuery<IFProcess>(p => p.Id == publish.ProcessId)
                    .Include(s => s.Project.Solution)
                    .Include(s => s.Commands).ThenInclude(c => c.Parent)
                    .Include(s => s.Commands).ThenInclude(c => c.Childrens)
                    .Include(s => s.Commands).ThenInclude(s => s.IFClassMapper.IFClass)
                    .Include(s => s.Commands).ThenInclude(s => s.Model.Properties).ThenInclude(s => s.EntityProperty)
                    .Include(s => s.Commands).ThenInclude(s => s.Model.Entity.Relations)
                    .Include(s => s.Commands).ThenInclude(s => s.Model.Entity.Relations).ThenInclude(s => s.ForeignKeyIFEntityProperty)
                    .Include(s => s.Commands).ThenInclude(s => s.CommandFilterItems).SingleOrDefaultAsync();

                var process2 = await this.GetQuery<IFProcess>(p => p.Id == publish.ProcessId)
                    .Include(s => s.Queries).ThenInclude(s => s.Model.Properties).ThenInclude(s => s.EntityProperty)
                    .Include(s => s.Queries).ThenInclude(s => s.QueryFilterItems).ThenInclude(s => s.EntityProperty)
                    .Include(s => s.Queries).ThenInclude(s => s.QueryFilterItems).ThenInclude(s => s.IFPageParameter)

                    .SingleOrDefaultAsync();


                CqrsQueryGenerator queryGenerator = new CqrsQueryGenerator(entityService, process);
                await queryGenerator.Generate();

                CqrsCommandGenerator commandGenerator = new CqrsCommandGenerator(entityService, classService, process);
                await commandGenerator.Generate();

                DirectoryHelper.MoveDirectory(DirectoryHelper.GetTempProcessDirectory(process), DirectoryHelper.GetNewProcessDirectory(process));

                WebApiMethodGenerator apiMethodGenerator = new WebApiMethodGenerator(process);
                apiMethodGenerator.Generate();

                DirectoryHelper.MoveDirectory(DirectoryHelper.GetTempWebApiControllerDirectory(process.Name), DirectoryHelper.GetNewApiControllerDirectory(process));


                publish.ProjectId = process.ProjectId;
                publish.SolutionId = process.Project.SolutionId;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

    }
}