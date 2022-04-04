using IF.CodeGeneration.Language.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Cqrs.Command;
using IF.Manager.Service.EF;

namespace IF.Manager.Service
{
    public class CqrsDeleteCommandHandlerGenerator : CqrsBaseCommandHandlerGenerator
    {

        public CqrsDeleteCommandHandlerGenerator(IFProcess process, IFCommand command) : base(process, command)
        {
        }


        public void Generate(ModelClassTreeDto entityTree)
        {

            CSClass commandHandlerClass = GetCommandHandlerClass();

            EFDeleteCommandMethod method = new EFDeleteCommandMethod($"ExecuteCommand", entityTree, command);

            commandHandlerClass.Methods.Add(method.Build());

            base.fileSystem.FormatCode(commandHandlerClass.GenerateCode(), "cs","",command.Name);
           
            GenerateCommandContextAndOverrideClass();

        }

       

       




    }


}
