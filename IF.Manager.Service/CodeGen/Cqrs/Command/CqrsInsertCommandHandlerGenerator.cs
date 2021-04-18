using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen.Cqrs.Command;
using IF.Manager.Service.CodeGen.EF;
using IF.Manager.Service.EF;

namespace IF.Manager.Service
{
    public class CqrsInsertCommandHandlerGenerator : CqrsBaseCommandHandlerGenerator
    {

        public CqrsInsertCommandHandlerGenerator(IFProcess process, IFCommand command) : base(process, command)
        {
        }


        public void Generate(ModelClassTreeDto entityTree)
        {

            CSClass commandHandlerClass = GetCommandHandlerClass();

            EFInsertCommandMethod method = new EFInsertCommandMethod($"ExecuteCommand", entityTree, command);

            if (command.IsQueryOverride)
            {
                CSClass commandHandlerOverrideClass = new CSClass();
                commandHandlerOverrideClass.Name = command.Name;
                commandHandlerOverrideClass.IsPartial = true;
                commandHandlerOverrideClass.NameSpace = commandHandlerClass.NameSpace;
                commandHandlerOverrideClass.Methods.Add(method.BuildOverridenQuery());

                fileSystem.FormatCode(commandHandlerOverrideClass.GenerateCode().Template, "cs", command.Name + "Override",command.Name);

            }
            else
            {
                commandHandlerClass.Methods.Add(method.Build());
            }

            base.fileSystem.FormatCode(commandHandlerClass.GenerateCode(), "cs","",command.Name);
           
            GenerateCommandContextAndOverrideClass();

        }

       
      




    }


}
