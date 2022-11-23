using IF.Core.Data;
using IF.Manager.Contracts.Model;

using System.Collections.Generic;
using System.Linq;

namespace IF.Manager.Contracts.Dto
{
    public class ModelClassTreeDto : TreeDto<ModelClassTreeDto>
    {
        public ModelClassTreeDto()
        {
            this.Childs = new List<ModelClassTreeDto>();
        }


        public static bool IsModelProperty(bool IsRelation,int entityPropertyId, IEnumerable<IFModelProperty> modelProperties)
        {
            bool IsModelProperty = false;

            if (IsRelation)
            {
                if (modelProperties.Any(p => p.EntityId == entityPropertyId))
                {
                    IsModelProperty = true;
                }
            }
            else
            {
                var property = modelProperties.SingleOrDefault(p => p.EntityPropertyId == entityPropertyId);

                if (property != null)
                {
                    IsModelProperty = true;
                }
            }

            return IsModelProperty;
        }

        public string Type { get; set; }
        public bool IsRelation { get; set; }

        public bool IsList { get; set; }

        public string ClientId { get; set; }


        public string Name { get; set; }


        public bool IsNullable { get; set; }

        public bool IsIdentity { get; set; }


    }
}
