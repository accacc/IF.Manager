using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Cqrs.Command;
using IF.Manager.Service.CodeGen.EF;
using IF.Manager.Service.EF;

namespace IF.Manager.Service
{
    public class CqrsInsertCommandHandlerGenerator: CqrsBaseCommandHandlerGenerator
    {

        public CqrsInsertCommandHandlerGenerator(IFProcess process,IFCommand command):base(process, command)
        {
        }
       

        public void GenerateInsertCqrsHandlerClass(ModelClassTreeDto entityTree)
        {
    
            CSClass commandHandlerClass = GetCommandHandlerClass();

            EFInsertCommandMethod method = new EFInsertCommandMethod($"ExecuteCommand", entityTree, command);

            commandHandlerClass.Methods.Add(method.Build());

            base.fileSystem.FormatCode(commandHandlerClass.GenerateCode(), "cs");

           // if (command.IsBeforeExecuteOverride || command.IsAfterExecuteOverride)
            {
                CqrsCommandOverrideClassGenerator overrideClassGenerator = new CqrsCommandOverrideClassGenerator(command, base.fileSystem);
                overrideClassGenerator.Generate();
            }


        }

        public void GenerateMultiInsertCqrsHandlerClass()
        {


            CSClass commandHandlerClass = GetCommandHandlerClass();

            MultiCommandGenerator method = new MultiCommandGenerator($"ExecuteCommand", command);


            commandHandlerClass.Methods.Add(method.Build());

            base.fileSystem.FormatCode(commandHandlerClass.GenerateCode(), "cs");


        }




    }


}
