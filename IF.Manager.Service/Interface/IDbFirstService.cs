using DatabaseSchemaReader.DataSchema;

using IF.Core.Persistence;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.CodeGen;
using IF.Manager.Service.Dto;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Service.Interface
{
    public interface IDbFirstService: IRepository
    {

        Task AddDbFirst(List<DatabaseTable> tableSchemas, List<TableDbFirstDto> tables,int ProcessId, GenerateOptions generateOptions);
        List<DatabaseTable> GetAllTableSchemas(string ConnectionString);

        Task GenerateQueryAndCommands(int processId, string name, IFEntity entity, GenerateOptions generateOptions);
    }
}
