﻿using DatabaseSchemaReader.DataSchema;

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
using System.Threading.Tasks;

namespace IF.Manager.Service.Services
{
    public class DbFirstService : GenericRepository, IDbFirstService
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


        public async Task AddDbFirst(List<DatabaseTable> tableSchemas, List<TableDbFirstDto> tables,int ProcessId, GenerateOptions generateOptions)
        {
            //using (TransactionScope scope = new TransactionScope())

            {

                try
                {
                    await AddEntities(tableSchemas, tables);

                    foreach (var item in tables)
                    {
                        if (item.Table == null) continue;

                        string entityName = ObjectNamerHelper.AddAsLastWord(item.Table, "Entity");

                        var entity = await this.GetQuery<IFEntity>(e => e.Name == entityName)
                            .Include(e => e.Properties)
                            .SingleOrDefaultAsync();

                        await GenerateQueryAndCommands(ProcessId, item.Table, entity,generateOptions);
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

                //  scope.Complete();

            }

        }

        public async Task GenerateQueryAndCommands(int processId, string name, IFEntity entity, GenerateOptions generateOptions)
        {
            if (generateOptions.SelectOperation)
            {
                await GenerateQuery(processId, entity, name + "Get", QueryGetType.Single);
                await GenerateQuery(processId, entity, name + "List", QueryGetType.List);
            }

            if(generateOptions.InsertOperation)
            {
                await GenerateCommand(processId, entity, name + "Insert", CommandType.Insert);
            }

            if (generateOptions.UpdateOperation)
            {
                await GenerateCommand(processId, entity, name + "Update", CommandType.Update);
            }

            if (generateOptions.DeleteOperation)
            {
                await GenerateCommand(processId, entity, name + "Delete", CommandType.Delete);
            }

          
           
           
        }

        private async Task GenerateCommand(int processId, IFEntity entity, string modelName, CommandType getType)
        {
            var selectGetModel = GenerateModel(modelName, entity);
            this.Add(selectGetModel);
            await this.UnitOfWork.SaveChangesAsync();
            await AddCommand(processId, modelName, selectGetModel, getType);
        }

        private async Task GenerateQuery(int processId, IFEntity entity, string modelName, QueryGetType getType)
        {
            var selectGetModel = GenerateModel(modelName, entity);
            this.Add(selectGetModel);
            await this.UnitOfWork.SaveChangesAsync();
            await AddQuery(processId, modelName, selectGetModel, getType);
        }

        private async Task AddCommand(int processId, string modelName, IFModel selectGetModel, CommandType getType)
        {
            IFCommand command = new IFCommand();
            command.Name = modelName;
            command.Description = modelName;
            command.ModelId = selectGetModel.Id;
            command.ProcessId = processId;
            command.IsList = false;
            command.CommandGetType = getType;

            await this.commandService.AddCommand(command);
        }


        private async Task AddQuery(int processId, string modelName, IFModel selectGetModel, QueryGetType getType)
        {
            QueryDto queryDto = new QueryDto();
            queryDto.Name = modelName;
            queryDto.Description = modelName;
            queryDto.ModelId = selectGetModel.Id;
            queryDto.ProcessId = processId;
            queryDto.QueryGetType = getType;
            queryDto.PageNumber = 1;
            queryDto.PageSize = 20;

            await this.queryService.AddQuery(queryDto);
        }

        private IFModel GenerateModel(string Name, IFEntity entity)
        {
            IFModel model = new IFModel();

            try
            {
                model.Name = Name + "DataModel";
                model.Description = Name;
                model.EntityId = entity.Id;

                foreach (var property in entity.Properties)
                {
                    IFModelProperty modelProperty = new IFModelProperty();
                    modelProperty.EntityId = entity.Id;
                    modelProperty.EntityPropertyId = property.Id;
                    model.Properties.Add(modelProperty);
                }

            }
            catch (Exception ex)
            {

                throw;
            }

            return model;
        }

        private async Task AddEntities(List<DatabaseTable> tableSchemas, List<TableDbFirstDto> tables)
        {
            foreach (var table in tableSchemas)
            {
                string entityName = ObjectNamerHelper.AddAsLastWord(table.Name, "Entity");

                if (await this.entityService.EntityIsExistByName(entityName))
                {
                    throw new BusinessException($"{entityName} Entity already exist");
                }

                IFEntity entity = new IFEntity();

                entity.Description = entityName;
                entity.Name = entityName;
                entity.AuditType = Enum.IFAuditType.None;

                foreach (var column in table.Columns)
                {
                    IFEntityProperty property = new IFEntityProperty();

                    property.IsIdentity = column.IsPrimaryKey;

                    property.IsAutoNumber = column.IsAutoNumber;

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
            if (type.Name == "Byte[]") { return "Byte[]"; }

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
