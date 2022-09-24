using IF.CodeGeneration.Language.CSharp;
using IF.Core.Audit;
using IF.Core.Data;
using IF.Core.Localization;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Persistence.EF.Audit;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen.EF
{
    public  class LanguageEntityClass: EntityBaseClass
    {

        public EntityDto EntityMetaData { get; set; }
        public LanguageEntityClass(EntityDto entity, IFProject project):base(entity)
        {
            this.EntityMetaData = entity;
            this.NameSpace = SolutionHelper.GetCoreBaseNamespace(project);
            this.Name = $"{this.EntityMetaData.Name}Language";
            this.BaseClass = nameof(Entity);
            this.InheritedInterfaces.Add(nameof(ILanguageEntity));
            this.InheritedInterfaces.Add($"I{entity.Name}Language");
            this.Usings.Add("System.ComponentModel.DataAnnotations");
            this.Usings.Add("IF.Core.Data");
            this.Usings.Add("System");
            this.Usings.Add("IF.Core.Localization");
            this.Usings.Add(SolutionHelper.GetCoreNamespace(project));
        }

        public override void Build()
        {
            this.GenerateProperties();

            var ObjectIdProperty = new CSProperty("public", "ObjectId", false);
            ObjectIdProperty.PropertyTypeString = "int";
            this.Properties.Add(ObjectIdProperty);



            var LanguageIdProperty = new CSProperty("public", "LanguageId", false);
            LanguageIdProperty.PropertyTypeString = "int";
            this.Properties.Add(LanguageIdProperty);

        }
    }
}
