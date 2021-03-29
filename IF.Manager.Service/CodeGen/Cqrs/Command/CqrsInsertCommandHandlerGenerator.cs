using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.CodeGen.Cqrs;
using IF.Manager.Service.CodeGen.Cqrs.Command;
using IF.Manager.Service.CodeGen.EF;
using IF.Manager.Service.CodeGen.Interface;
using IF.Manager.Service.CodeGen.Model;
using IF.Manager.Service.EF;
using IF.Manager.Service.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
