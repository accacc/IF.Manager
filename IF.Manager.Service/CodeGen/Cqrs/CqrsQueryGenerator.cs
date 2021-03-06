using IF.CodeGeneration.Core;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.Cqrs;
using IF.Manager.Service.Model;
using System.Threading.Tasks;

namespace IF.Manager.Service.Services
{
    public class CqrsQueryGenerator
    {

        private readonly IEntityService entityService;
        private readonly IModelService modelService;
        private readonly FileSystemCodeFormatProvider fileSystem;
        string generatedBasePath;
        IFProcess process;

        public CqrsQueryGenerator(IEntityService entityService, IModelService modelService, IFProcess process)
        {
            this.entityService = entityService;
            this.modelService = modelService;
            this.process = process;
            generatedBasePath = generatedBasePath = DirectoryHelper.GetTempProcessDirectory(process);
            this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
        }


        public async Task Generate()
        {
            string nameSpace = SolutionHelper.GetProcessNamaspace(process);

            foreach (var query in process.Queries)
            {
                var entityTree = await entityService.GetEntityTree(query.Model.EntityId);

                ModelGenerator modelGenerator = new ModelGenerator(fileSystem, query.Model, nameSpace, entityTree);
                modelGenerator.Generate();

                CqrsQueryHandlerGenerator handlerGenerator = new CqrsQueryHandlerGenerator();
                handlerGenerator.GenerateCqrsHandlerClass(query, entityTree, fileSystem);



                CqrsRequestClass requestClass = new CqrsRequestClass();
                requestClass.GenerateRequestClass(query, fileSystem);

                CqrsResponseClass responseClass = new CqrsResponseClass();
                responseClass.GenerateResponseClass(query, fileSystem);
            }


        }

        
        
    }
}
