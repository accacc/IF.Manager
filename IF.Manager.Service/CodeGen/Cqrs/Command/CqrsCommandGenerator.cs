using IF.CodeGeneration.Core;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.CodeGen.Cqrs;
using IF.Manager.Service.CodeGen.Model;
using IF.Manager.Service.Model;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Service
{
    public class CqrsCommandGenerator
    {
        private readonly IEntityService entityService;
        private readonly IClassService classService;
        private readonly FileSystemCodeFormatProvider fileSystem;
        IFProcess process;
        string generatedBasePath;
      

        public CqrsCommandGenerator(IEntityService entityService, IClassService classService, IFProcess process)
        {
            this.entityService = entityService;
            this.classService = classService;
            this.process = process;

            generatedBasePath = DirectoryHelper.GetTempProcessDirectory(process);

            this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
        }


        public async Task Generate()
        {
            string nameSpace = SolutionHelper.GetProcessNamaspace(process);

            var rootCommands = process.Commands.Where(c => !c.ParentId.HasValue).ToList();
           
            await GenerateCommands(nameSpace,rootCommands);

            foreach (var command in rootCommands.OrderBy(c=>c.Sequence))
            {
                if (command.IFClassMapper != null)
                {
                    await classService.GenerateClass(process, command.IFClassMapper.IFClassId.Value);
                    await classService.GenerateClassToModelMapper(process, command.IFClassMapper.IFClassId.Value, command.Id);
                }
            }

        }

        private async Task GenerateCommands(string nameSpace, List<IFCommand> commands)
        {
            foreach (var command in commands)
            {
                var childs = command.Childrens.OrderBy(c => c.Sequence).ToList();

                if (childs.Any())
                {
                    await GenerateParentCommand(nameSpace, command);

                    await GenerateCommands(nameSpace, childs);
                }
                else
                {
                    await GenerateChildCommand(nameSpace, command);
                }
            }
        }

        private async Task GenerateParentCommand(string nameSpace, IFCommand command)
        {
            GenerateCommandClassFilesDirectory(command);

            var entityTree = await entityService.GetEntityTree(command.Model.EntityId);

            MultiCommandModelGenerator modelGenerator = new MultiCommandModelGenerator(fileSystem, command.Model, nameSpace, command);

            modelGenerator.Generate(command.Name);

            CqrsCommandClassGenerator commandClassGenerator = new CqrsCommandClassGenerator(command, process, entityTree, fileSystem);

            commandClassGenerator.Generate();



            switch (command.CommandGetType)
            {
                case Core.Data.CommandType.Insert:
                    CqrsInsertCommandHandlerGenerator cqrsInsertCommandHandler = new CqrsInsertCommandHandlerGenerator(process, command);
                    cqrsInsertCommandHandler.GenerateMultiInsertCqrsHandlerClass();
                    break;
                case Core.Data.CommandType.Update:
                    CqrsUpdateCommandHandlerGenerator cqrsUpdateCommandHandler = new CqrsUpdateCommandHandlerGenerator(process, command);
                    cqrsUpdateCommandHandler.GenerateMultiInsertCqrsHandlerClass();
                    break;
                case Core.Data.CommandType.Delete:
                    CqrsDeleteCommandHandlerGenerator cqrsDeleteCommandHandler = new CqrsDeleteCommandHandlerGenerator(process, command);
                    cqrsDeleteCommandHandler.GenerateMultiInsertCqrsHandlerClass();
                    break;
                default:
                    throw new ApplicationException("unknow command type");
            }
        }

        private void GenerateCommandClassFilesDirectory(IFCommand command)
        {
            if (!Directory.Exists(generatedBasePath + "/" + command.Name))
            {
                Directory.CreateDirectory(generatedBasePath + "/" + command.Name);
            }
        }

        private async Task GenerateChildCommand(string nameSpace, IFCommand command)
        {

            GenerateCommandClassFilesDirectory(command);

            var entityTree = await entityService.GetEntityTree(command.Model.EntityId);

            ModelGenerator modelGenerator = new ModelGenerator(fileSystem, command.Model, nameSpace, entityTree);

            modelGenerator.Generate(command.Name);

            CqrsCommandClassGenerator commandClassGenerator = new CqrsCommandClassGenerator(command, process, entityTree, fileSystem);

            commandClassGenerator.Generate();


            switch (command.CommandGetType)
            {
                case Core.Data.CommandType.Insert:
                   
                    CqrsInsertCommandHandlerGenerator cqrsInsertCommandHandler = new CqrsInsertCommandHandlerGenerator(process,command);
                    cqrsInsertCommandHandler.Generate(entityTree);
                    break;
                case Core.Data.CommandType.Update:
                    CqrsUpdateCommandHandlerGenerator cqrsUpdateCommandHandler = new CqrsUpdateCommandHandlerGenerator(process, command);
                    cqrsUpdateCommandHandler.Generate(entityTree);
                    break;
                case Core.Data.CommandType.Delete:
                    CqrsDeleteCommandHandlerGenerator cqrsDeleteCommandHandler = new CqrsDeleteCommandHandlerGenerator(process, command);
                    cqrsDeleteCommandHandler.Generate(entityTree);
                    break;
                default:
                    throw new ApplicationException("unknow command type");
            }
        }
    }
}
