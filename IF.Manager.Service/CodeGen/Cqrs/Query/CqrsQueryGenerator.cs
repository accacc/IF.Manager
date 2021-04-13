using IF.CodeGeneration.Core;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.Cqrs;
using IF.Manager.Service.Model;

using System.IO;
using System.Threading.Tasks;

namespace IF.Manager.Service.Services
{
    public class CqrsQueryGenerator
    {

        private readonly IEntityService entityService;
        private readonly FileSystemCodeFormatProvider fileSystem;
        string generatedBasePath;
        IFProcess process;

        public CqrsQueryGenerator(IEntityService entityService,IFProcess process)
        {
            this.entityService = entityService;
            this.process = process;
            generatedBasePath = generatedBasePath = DirectoryHelper.GetTempProcessDirectory(process);
            this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
        }


        public async Task Generate()
        {
            string nameSpace = SolutionHelper.GetProcessNamaspace(process);

            foreach (var query in process.Queries)
            {
                if(!Directory.Exists(generatedBasePath + "/" + query.Name))
                {
                    Directory.CreateDirectory(generatedBasePath + "/" + query.Name);
                }

                var entityTree = await entityService.GetEntityTree(query.Model.EntityId);

                ModelGenerator modelGenerator = new ModelGenerator(fileSystem, query.Model, nameSpace, entityTree);
                modelGenerator.Generate(query.Name);

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
