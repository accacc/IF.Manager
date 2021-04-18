using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Cqrs.Command;
using IF.Manager.Service.CodeGen.EF;
using IF.Manager.Service.EF;

namespace IF.Manager.Service
{
    public class CqrsUpdateCommandHandlerGenerator : CqrsBaseCommandHandlerGenerator
    {

        public CqrsUpdateCommandHandlerGenerator(IFProcess process, IFCommand command) : base(process, command)
        {
        }

        public void Generate(ModelClassTreeDto entityTree)
        {

            CSClass commandHandlerClass = GetCommandHandlerClass();

            EFUpdateCommandMethod method = new EFUpdateCommandMethod($"ExecuteCommand", entityTree, command);

            commandHandlerClass.Methods.Add(method.Build());

            base.fileSystem.FormatCode(commandHandlerClass.GenerateCode(), "cs","",command.Name);
           
            GenerateCommandContextAndOverrideClass();

        }

       
      




    }


}
