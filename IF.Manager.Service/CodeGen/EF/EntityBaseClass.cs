using IF.CodeGeneration.Language.CSharp;
using IF.Manager.Contracts.Dto;

using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen.EF
{
    public abstract class EntityBaseClass: CSClass
    {

        public EntityDto EntityMetaData { get; set; }

        public EntityBaseClass(EntityDto entity)
        {
            this.EntityMetaData = entity;
        }

        public abstract void Build();
        public void GenerateProperties()
        {
            foreach (var entityProperty in this.EntityMetaData.Properties)
            {

                bool IsNullable = entityProperty.IsNullable;

                if (entityProperty.Type == "string")
                {
                    IsNullable = false;
                }


                var classProperty = new CSProperty("public", entityProperty.Name, IsNullable);

                classProperty.PropertyTypeString = entityProperty.Type;

                this.Properties.Add(classProperty);
            }
        }
    }
}
