using IF.CodeGeneration.Core;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Enum;
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
        private readonly IClassService classService;
        private readonly VsManager vs;
        private readonly VsHelper vsHelper;



        public ProjectService(ManagerDbContext dbContext, IEntityService entityService, IModelService modelService, IClassService classService) : base(dbContext)
        {
            this.entityService = entityService;
            this.modelService = modelService;
            this.classService = classService;
            this.fileSystem = new FileSystemCodeFormatProvider(DirectoryHelper.GetTempGeneratedDirectoryName());
            this.vs = new VsManager();
            this.vsHelper = new VsHelper(DirectoryHelper.GetTempGeneratedDirectoryName());
        }





       
        public async Task AddProject(IFProject form)
        {
            IFProject entity = new IFProject();
            entity.Id = form.Id;
            entity.Name = form.Name;
            entity.SolutionId = form.SolutionId;
            entity.ProjectType = form.ProjectType;
            entity.ConnectionString = form.ConnectionString;

            entity.SystemDbType = form.SystemDbType;
            entity.SystemDbConnectionString = form.SystemDbConnectionString;
            entity.JsonAppType = form.JsonAppType;
            entity.CommandAudit = form.CommandAudit;
            entity.CommandErrorHandler = form.CommandErrorHandler;
            entity.CommandPerformanceCounter = form.CommandPerformanceCounter;
            entity.QueryAudit = form.QueryAudit;
            entity.QueryErrorHandler = form.QueryErrorHandler;
            entity.QueryPerformanceCounter = form.QueryPerformanceCounter;
            entity.AuthenticationType = form.AuthenticationType;
            entity.IsAuthenticationAdded = false;
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

            entity.SystemDbType = form.SystemDbType;
            entity.SystemDbConnectionString = form.SystemDbConnectionString;
            entity.JsonAppType = form.JsonAppType;
            entity.CommandAudit = form.CommandAudit;
            entity.CommandErrorHandler = form.CommandErrorHandler;
            entity.CommandPerformanceCounter = form.CommandPerformanceCounter;
            entity.QueryAudit = form.QueryAudit;
            entity.QueryErrorHandler = form.QueryErrorHandler;
            entity.QueryPerformanceCounter = form.QueryPerformanceCounter;
            entity.AuthenticationType = form.AuthenticationType;

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

        public async Task<List<IFProject>> GetProjectList(ProjectType projectType)
        {
            var data = await this.GetQuery<IFProject>().Where(p => p.ProjectType == projectType)
                                .ToListAsync();

            return data;
        }

        //        projects = projects.Where(p => p.ProjectType == Contracts.Enum.ProjectType.Web).ToList();

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


        public async Task<bool> ProcessIsExistByName(string name)
        {

            bool isExist = false;
            try
            {
              
                IFProcess entity = await this.GetQuery<IFProcess>(s => s.Name == name).SingleOrDefaultAsync();

                if (entity != null)
                {
                    isExist = true;
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }

            return isExist;
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

        public async Task AddAuthentication(int projectId)
        {
            var entity = await this.GetQuery<IFProject>()
            .SingleOrDefaultAsync(k => k.Id == projectId);

            if (entity == null) { throw new BusinessException(" No such entity exists"); }

            entity.IsAuthenticationAdded = true;

            this.Update(entity);
            await this.UnitOfWork.SaveChangesAsync();
        }
    }
}
