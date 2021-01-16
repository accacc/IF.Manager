using IF.CodeGeneration.CSharp;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen.Interface
{
    public interface ICommandMethodGenerator
    {
        CSMethod Build();
    }
}
