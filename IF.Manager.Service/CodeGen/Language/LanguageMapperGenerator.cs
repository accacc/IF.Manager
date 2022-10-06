using IF.CodeGeneration.Core;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen.Language
{
    public class LanguageMapperGenerator
    {

        private readonly FileSystemCodeFormatProvider fileSystem;

        public LanguageMapperGenerator(FileSystemCodeFormatProvider fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void Generate(List<EntityDto> entityList, IFProject project)
        {


        }
    }
}