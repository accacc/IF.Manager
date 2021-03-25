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


        public static bool IsModelProperty(ModelClassTreeDto childEntityTree, IFModel model)
        {
            bool IsModelProperty = false;

            if (childEntityTree.IsRelation)
            {
                if (model.Properties.Any(p => p.EntityId == childEntityTree.Id))
                {
                    IsModelProperty = true;
                }
            }
            else
            {
                var property = model.Properties.SingleOrDefault(p => p.EntityPropertyId == childEntityTree.Id);

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
