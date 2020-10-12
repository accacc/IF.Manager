//using IF.Core.Data;
//using IF.Manager.Contracts.Enum;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Text;

//namespace IF.Manager.Contracts.Model
//{
//    public class IFFormModelProperty:Entity
//    {

//        public IFFormModelProperty()
//        {
//            this.QueryFilterItems = new List<IFQueryFilterItem>();
//            this.CommandFilterItems = new List<IFCommandFilterItem>();
//        }

//        [Key]
//        public int Id { get; set; }


//        public FormModelType Type { get; set; }

//        public string Name { get; set; }

//        public bool IsNullable { get; set; }

//        public IFFormModel FormModel { get; set; }

//        public int FormModelId { get; set; }

//        public ICollection<IFQueryFilterItem> QueryFilterItems { get; set; }
//        public ICollection<IFCommandFilterItem> CommandFilterItems { get; set; }
//    }
//}
