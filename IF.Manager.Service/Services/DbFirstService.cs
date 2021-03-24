using DatabaseSchemaReader.DataSchema;

using IF.Core.Data;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.CodeGen;
using IF.Manager.Service.Dto;
using IF.Manager.Service.Interface;
using IF.Persistence.EF;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Service.Services
{
    public class DbFirstService: GenericRepository, IDbFirstService
    {
        private readonly IEntityService entityService;
        private readonly IProjectService projectService;
        private readonly IQueryService queryService;
        private readonly ICommandService commandService;
        public DbFirstService(ManagerDbContext dbContext, IEntityService entityService, IProjectService projectService, IQueryService queryService, ICommandService commandService) : base(dbContext)
        {
            this.entityService = entityService;
            this.projectService = projectService;
            this.queryService = queryService;
            this.commandService = commandService;
        }


        public async Task AddDbFirst(List<DatabaseTable> tableSchemas, List<TableDbFirstDto> tables, GenerateOptions generateOptions)
        {
            try
            {
                await AddEntities(tableSchemas, tables);

                ProcessDto process = new ProcessDto();
                process.Description = "test";
                process.ProjectId = 1;
                process.Name = "Test";

                await this.projectService.AddProcess(process);

                foreach (var item in tables)
                {
                    var entity = await this.GetQuery<IFEntity>(e => e.Name == item.Table + "Entity")
                        .Include(e => e.Properties).SingleOrDefaultAsync();

                    await GenerateQuery(process, entity, item.Table + "Get", QueryGetType.Single);
                    await GenerateQuery(process, entity, item.Table + "List", QueryGetType.Single);


                    await GenerateCommand(process, entity, item.Table + "Insert", CommandType.Insert);
                    await GenerateCommand(process, entity, item.Table + "Update", CommandType.Update);
                    await GenerateCommand(process, entity, item.Table + "Delete", CommandType.Delete);




                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        private async Task GenerateCommand(ProcessDto process, IFEntity entity, string modelName, CommandType getType)
        {
            var selectGetModel = GenerateModel(modelName, entity);
            this.Add(selectGetModel);
            await this.UnitOfWork.SaveChangesAsync();
            await AddCommand(process, modelName, selectGetModel, getType);
        }

        private async Task GenerateQuery(ProcessDto process, IFEntity entity, string modelName, QueryGetType getType)
        {
            var selectGetModel = GenerateModel(modelName, entity);
            this.Add(selectGetModel);
            await this.UnitOfWork.SaveChangesAsync();
            await AddQeury(process, modelName, selectGetModel,getType);
        }

        private async Task AddCommand(ProcessDto process, string modelName, IFModel selectGetModel, CommandType getType)
        {
            IFCommand command = new IFCommand();
            command.Name = modelName;
            command.Description = modelName;
            command.ModelId = selectGetModel.Id;
            command.ProcessId = process.Id;
            command.CommandGetType = getType;
            

            await this.commandService.AddCommand(command);
        }


        private async Task AddQeury(ProcessDto process, string modelName, IFModel selectGetModel, QueryGetType getType)
        {
            QueryDto queryDto = new QueryDto();
            queryDto.Name = modelName;
            queryDto.Description = modelName;
            queryDto.ModelId = selectGetModel.Id;
            queryDto.ProcessId = process.Id;
            queryDto.QueryGetType = getType;
            queryDto.PageNumber = 1;
            queryDto.PageSize = 20;

            await this.queryService.AddQuery(queryDto);
        }

        private IFModel GenerateModel(string Name, IFEntity entity)
        {
            IFModel model = new IFModel();
            model.Name = Name;
            model.Description = Name;
            model.EntityId = entity.Id;

            foreach (var property in entity.Properties)
            {
                IFModelProperty modelProperty = new IFModelProperty();
                modelProperty.EntityId = entity.Id;
                modelProperty.EntityPropertyId = property.Id;
            }

            return model;
        }

        private async Task AddEntities(List<DatabaseTable> tableSchemas, List<TableDbFirstDto> tables)
        {
            foreach (var table in tableSchemas)
            {

                if (await this.entityService.EntityIsExistByName(table.Name))
                {
                    throw new BusinessException($"{table.Name} Entity already exist");
                }

                string entityName = DirectoryHelper.AddAsLastWord(table.Name, "Entity");

                IFEntity entity = new IFEntity();

                entity.Description = entityName;
                entity.Name = entityName;
                entity.IsAudited = false;

                foreach (var column in table.Columns)
                {
                    IFEntityProperty property = new IFEntityProperty();

                    property.IsIdentity = column.IsPrimaryKey;


                    property.Name = column.Name;
                    property.Type = GetPrimitiveByDotnet(column.NetDataType());

                    if (column.DbDataType == "smallint")
                    {
                        property.Type = "Int16";
                    }

                    property.IsNullable = column.Nullable;
                    entity.Properties.Add(property);
                }

                if (!entity.Properties.Any(p => p.IsIdentity))
                {
                    var info = tables.SingleOrDefault(i => i.Table == table.Name);

                    if (info != null && info.PrimaryKey != null)
                    {
                        var primaryColumn = entity.Properties.SingleOrDefault(e => e.Name == info.PrimaryKey);

                        if (primaryColumn != null) primaryColumn.IsIdentity = true;

                    }

                }


                this.Add(entity);
            }

                await UnitOfWork.SaveChangesAsync();
            
        }

        public string GetPrimitiveByDotnet(Type type)
        {

            if (type.Name == "String") { return "string"; }
            if (type.Name == "Int32") { return "int"; }
            if (type.Name == "Int64") { return "long"; }
            if (type.Name == "Boolean") { return "bool"; }
            if (type.Name == "DateTime") { return "DateTime"; }
            if (type.Name == "Decimal") { return "decimal"; }

            throw new ApplicationException($"Tip bulunamadi {type.Name}");
        }

        public List<DatabaseTable> GetAllTableSchemas(string ConnectionString)
        {
            List<DatabaseTable> list = new List<DatabaseTable>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var dr = new DatabaseSchemaReader.DatabaseReader(connection);

                var schema = dr.ReadAll();

                foreach (var table in schema.Tables)
                {
                    list.Add(table);
                }
            }

            return list;
        }

    }
}
