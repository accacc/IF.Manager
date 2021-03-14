using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Dto
{
    public class ModelUpdateDto
    {
        public int Id { get; set; }
        public string Values { get; set; }

        private List<ModelPropertyDto> Properties { get; set; }

        public List<ModelPropertyDto> GetProperties()
        {
            this.Properties = new List<ModelPropertyDto>();

            foreach (var property in Values.Split(','))
            {
                if (property.StartsWith("self-") || property.StartsWith("relation")) continue;

                var stringArray = property.Split('-');

                ModelPropertyDto modelProperty = new ModelPropertyDto();
                modelProperty.EntityPropertyId = Convert.ToInt32(stringArray[0]);
                modelProperty.EntityId = Convert.ToInt32(stringArray[1]);

                if (3 == stringArray.Length && 4 > stringArray.Length && stringArray[2] != null)
                {
                    modelProperty.ModelPropertyId = Convert.ToInt32(stringArray[2]);
                }

                this.Properties.Add(modelProperty);
            }

            return this.Properties;
        }
    }

    public class ModelPropertyDto
    {

        public int EntityPropertyId { get; set; }

        public int EntityId { get; set; }

        public string Name { get; set; }

        public string EntityName { get; set; }

        public int ModelPropertyId { get; set; }
    }


    public class ModelDto
    {

        public int EntityId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
    }


}
