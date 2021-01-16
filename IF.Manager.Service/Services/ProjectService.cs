using IF.CodeGeneration.Core;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.Services;
using IF.Manager.Service.WebApi;
using IF.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace IF.Manager.Service
{
    public class ProjectService : GenericRepository, IProjectService
    {
        private readonly IEntityService entityService;
        private readonly IModelService modelService;
        private readonly FileSystemCodeFormatProvider fileSystem;
        private readonly VsManager vs;
        private readonly VsHelper vsHelper;



        public ProjectService(ManagerDbContext dbContext, IEntityService entityService, IModelService modelService) : base(dbContext)
        {
            this.entityService = entityService;
            this.modelService = modelService;
            this.fileSystem = new FileSystemCodeFormatProvider(DirectoryHelper.GetTempGeneratedDirectoryName());
            this.vs = new VsManager();
            this.vsHelper = new VsHelper(DirectoryHelper.GetTempGeneratedDirectoryName());
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

        public async Task AddProject(IFProject form)
        {
            IFProject entity = new IFProject();
            entity.Id = form.Id;
            entity.Name = form.Name;
            entity.SolutionId = form.SolutionId;
            entity.ProjectType = form.ProjectType;
            entity.ConnectionString = form.ConnectionString;
            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();
            form.Id = entity.Id;
        }

        public async Task UpdateProject(IFProject form)
        {

            var entity = await this.GetQuery<IFProject>()
               .SingleOrDefaultAsync(k => k.Id == form.Id);

            if (entity == null) { throw new BusinessException(" No such entity exists"); }

            entity.Name = form.Name;
            entity.ConnectionString = form.ConnectionString;
            entity.ProjectType = form.ProjectType;
            this.Update(entity);
            await this.UnitOfWork.SaveChangesAsync();
        }

        public async Task<IFProject> GetProject(int id)
        {
            var entity = await this.GetQuery<IFProject>()
                .Include(p=>p.Solution)
            
            .SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException("Project : No such entity exists"); }

            return entity;
        }


        public async Task<List<IFProject>> GetProjectList()
        {
            var data = await this.GetQuery<IFProject>()
                                .ToListAsync();

            return data;
        }

        public async Task AddSolution(IFSolution form)
        {
            IFSolution entity = new IFSolution();
            entity.SolutionName = form.SolutionName;
            entity.Path = form.Path;
            entity.Description = form.Description;
            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();
            form.Id = entity.Id;
        }

        public async Task UpdateSolution(IFSolution form)
        {

            var entity = await this.GetQuery<IFSolution>()
               .SingleOrDefaultAsync(k => k.Id == form.Id);

            if (entity == null) { throw new BusinessException(" No such entity exists"); }

            entity.SolutionName = form.SolutionName;
            entity.Path = form.Path;
            entity.Description = form.Description;
            this.Update(entity);
            await this.UnitOfWork.SaveChangesAsync();
        }

        public async Task<IFSolution> GetSolution(int id)
        {
            var entity = await this.GetQuery<IFSolution>()
            .SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException("Solution : No such entity exists"); }

            return entity;
        }


        public async Task<List<IFSolution>> GetSolutionList()
        {


            var solutions = await this.GetQuery<IFSolution>()

                                .Include(s => s.Projects).ThenInclude(s => s.IFPages)
                                .Include(s => s.Projects).ThenInclude(s => s.Processes)
                                .ToListAsync();

            return solutions;
        }


        public async Task AddProcess(ProcessDto form)
        {
            IFProcess entity = new IFProcess();
            entity.Name = form.Name;
            entity.Description = form.Description;
            entity.ProjectId = form.ProjectId;
            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();
            form.Id = entity.Id;
        }

        public async Task UpdateProcess(ProcessDto form)
        {

            var entity = await this.GetQuery<IFProcess>()
               .SingleOrDefaultAsync(k => k.Id == form.Id);

            if (entity == null) { throw new BusinessException(" No such entity exists"); }

            entity.Name = form.Name;

            entity.Description = form.Description;
            this.Update(entity);
            await this.UnitOfWork.SaveChangesAsync();
        }

        public async Task<ProcessDto> GetProcess(int id)
        {
            var entity = await this.GetQuery<IFProcess>()
            .Select(x => new ProcessDto
            {
                Id = x.Id,
                Name = x.Name,

                Description = x.Description
            }).SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException("Process : No such entity exists"); }

            return entity;
        }


        public async Task<List<ProcessDto>> GetProcessList()
        {
            var data = await this.GetQuery<IFProcess>()
                                .Select(x => new ProcessDto
                                {
                                    Id = x.Id,
                                    Name = x.Name,

                                    Description = x.Description
                                }).ToListAsync();

            return data;
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
            var solution = await this.GetSolution(form.SolutionId);

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


                IFProject project = await this.GetProject(publish.ProjectId);

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

            //publish.ProjectId = process.ProjectId;
            //publish.SolutionId = process.Project.SolutionId;

            //await this.AddPublish(publish);

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







        public async Task PublishProcess(PublishDto publish)
        {

            try
            {
                DirectoryHelper.CreateDirectory(DirectoryHelper.GetTempGeneratedDirectoryName(), true);
                DirectoryHelper.CreateDirectory(DirectoryHelper.GetBaseTempDirectoryName(), true);

                var process = await this.GetQuery<IFProcess>(p => p.Id == publish.ProcessId)
                    .Include(s => s.Project.Solution)
                    .Include(s => s.Commands).ThenInclude(s=>s.Childrens).ThenInclude(s => s.Model.Properties).ThenInclude(s => s.EntityProperty)
                    .Include(s => s.Commands).ThenInclude(s => s.CommandFilterItems)
                    .Include(s => s.Queries).ThenInclude(s => s.Model.Properties).ThenInclude(s => s.EntityProperty)
                    .Include(s => s.Queries).ThenInclude(s => s.QueryFilterItems).ThenInclude(s=>s.EntityProperty)
                    .Include(s => s.Queries).ThenInclude(s => s.QueryFilterItems).ThenInclude(s => s.IFPageParameter)
                    .SingleOrDefaultAsync();


                CqrsQueryGenerator queryGenerator = new CqrsQueryGenerator(entityService, modelService,process);
                await queryGenerator.Generate();

                CqrsCommandGenerator commandGenerator = new CqrsCommandGenerator(entityService,process);
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
