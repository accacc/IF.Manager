using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.CodeGen
{
    public class GenerateOptions
    {

        public bool SelectOperation { get; set; }

        public bool InsertOperation { get; set; }

        public bool UpdateOperation { get; set; }

        public bool DeleteOperation { get; set; }

        public string ProcessName { get; set; }

        public int ProjectId { get; set; }
    }
}
