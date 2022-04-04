using IF.Core.Data;
using IF.Manager.Contracts.Enum;
using IF.Manager.Service.Enum;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFProject : Entity
    {
        public IFProject()
        {
            this.Processes = new List<IFProcess>();
            this.IFPages = new List<IFPage>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ProjectType  ProjectType { get; set; }

        public string ConnectionString { get; set; }
        public string SystemDbConnectionString { get; set; }

        public SystemDbType SystemDbType { get; set; }

        public JsonAppType JsonAppType { get; set; }

        public string Description { get; set; }
        
        public int SolutionId { get; set; }

        public AuthenticationType AuthenticationType { get; set; }
        public bool CommandAudit { get; set; }
        public bool IsAuthenticationAdded { get; set; }
        public bool CommandPerformanceCounter { get; set; }
        public bool CommandErrorHandler { get; set; }
        public bool QueryAudit { get; set; }
        public bool QueryPerformanceCounter { get; set; }
        public bool QueryErrorHandler { get; set; }

        public IFSolution Solution { get; set; }

        public ICollection<IFProcess> Processes { get; set; }
        public ICollection<IFPage> IFPages { get; set; }


    }
}