//using IF.Core.Data;
//using IF.Manager.Contracts.Enum;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Text;

//namespace IF.Manager.Contracts.Model
//{
//    public class IFFormModel:Entity
//    {
//        public IFFormModel()
//        {
//            this.Queries = new List<IFQuery>();
//            this.Commands = new List<IFCommand>();
//            this.Properties = new List<IFFormModelProperty>();
//            this.QueryFilterItems = new List<IFQueryFilterItem>();
//            this.CommandFilterItems= new List<IFCommandFilterItem>();
//        }


//        [Key]
//        public int Id { get; set; }

        

//        public string Name { get; set; }

        

//        public ICollection<IFQuery> Queries { get; set; }

//        public ICollection<IFCommand> Commands { get; set; }

//        public ICollection<IFFormModelProperty> Properties { get; set; }

//        public ICollection<IFQueryFilterItem> QueryFilterItems { get; set; }

//        public ICollection<IFCommandFilterItem> CommandFilterItems { get; set; }


//    }
//}

