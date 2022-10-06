using IF.CodeGeneration.Language.CSharp;
using IF.Manager.Contracts.Dto;

using System.Collections.Generic;

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
            this.GenerateProperties(this.EntityMetaData.Properties);
        }
       

        public void GenerateProperties(List<EntityPropertyDto> entityProperties)
        {
            foreach (var entityProperty in entityProperties)
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
