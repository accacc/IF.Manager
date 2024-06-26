﻿using IF.Core.Data;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.CodeGen;
using IF.Persistence.EF;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Service.Services
{
    public class QueryService : GenericRepository, IQueryService
    {


        public QueryService(ManagerDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<QueryFilterTreeDto>> GetFilterTreeList(int queryId)
        {
            List<QueryFilterTreeDto> tree = null;

            try
            {
                var list = await this.GetQuery<IFQueryFilterItem>().Include(f => f.EntityProperty).Select(x =>
                new QueryFilterTreeDto
                {

                    ConditionOperator = x.ConditionOperator,
                    Id = x.Id,
                    ParentId = x.ParentId,
                    FilterOperator = x.FilterOperator,
                    IsNullCheck = x.IsNullCheck,
                    EntityPropertyId = x.EntityPropertyId,
                    Value = x.Value,
                    QueryId = x.QueryId,
                    PropertyName = x.EntityProperty.Name

                }).ToListAsync();

                var parents = list.Where(c => c.QueryId == queryId && !c.ParentId.HasValue).ToList();


                tree = list.ToTree(parents);
            }
            catch (Exception ex)
            {

                throw;
            }

            return tree;
        }

        public async Task AddQuery(QueryDto form)
        {
            IFQuery queryEntity = new IFQuery();

            string name = ObjectNamerHelper.AddAsLastWord(form.Name, "DataQuery");

            queryEntity.Id = form.Id;
            queryEntity.Name = name;
            queryEntity.ModelId = form.ModelId;
            queryEntity.ProcessId = form.ProcessId;
            queryEntity.Description = form.Description;
            queryEntity.QueryGetType = form.QueryGetType;
            queryEntity.PageNumber = form.PageNumber;
            queryEntity.PageSize = form.PageSize;
            queryEntity.IsQueryOverride = form.IsQueryOverride;
            this.Add(queryEntity);
            await this.UnitOfWork.SaveChangesAsync();
            form.Id = queryEntity.Id;
        }

        public async Task UpdateQuery(QueryDto form)
        {

            try
            {
                var entity = await this.GetQuery<IFQuery>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                string name = ObjectNamerHelper.AddAsLastWord(form.Name, "DataQuery");
                entity.Name = name;
                entity.Description = form.Description;
                entity.ModelId = form.ModelId;
                entity.ProcessId = form.ProcessId;
                entity.QueryGetType = form.QueryGetType;
                entity.PageNumber = form.PageNumber;
                entity.PageSize = form.PageSize;
                entity.IsQueryOverride = form.IsQueryOverride;
                this.Update(entity);
                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<QueryDto> GetQuery(int id)
        {
            var entity = await this.GetQuery<IFQuery>()
            .Select(x => new QueryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ModelId = x.ModelId,
                ProcessId = x.ProcessId,
                QueryGetType = x.QueryGetType,
                PageSize = x.PageSize,
                PageNumber = x.PageNumber,
                IsQueryOverride = x.IsQueryOverride

            }).SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException("Query : No such entity exists"); }

            return entity;
        }


        public async Task<List<QueryDto>> GetQueryList()
        {
            var data = await this.GetQuery<IFQuery>()
                                .Select(x => new QueryDto
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Description = x.Description,
                                }).ToListAsync();

            return data;
        }

        public async Task<List<IFModelProperty>> GetQueryModelPropertyList(int queryId)
        {
            var query = await this.GetQuery<IFQuery>(q => q.Id == queryId).SingleOrDefaultAsync();

            var data = await this.GetQuery<IFModelProperty>(x => x.ModelId == query.ModelId).Include(p => p.EntityProperty).ToListAsync();

            return data;
        }

        public async Task<QueryFilterDto> GetQueryFilterItems(int queryId, int? ParentId)
        {
            QueryFilterDto filter = new QueryFilterDto();
            filter.QueryId = queryId;
            filter.ParentId = ParentId;


            var query = await this.GetQuery<IFQueryFilterItem>(f => f.QueryId == queryId && f.ParentId == ParentId)


                .Select(i => new QueryFilterItemDto
                {

                    ConditionOperator = i.ConditionOperator,
                    FilterOperator = i.FilterOperator,
                    QueryId = i.QueryId,
                    Value = i.Value,
                    EntityPropertyId = i.EntityPropertyId,
                    PropertyName = i.EntityProperty.Name,
                    FormModelPropertyId = i.FormModelPropertyId,
                    PageParameterId = i.IFPageParameterId,
                    Id = i.Id,
                    IsNullCheck = i.IsNullCheck



                }).ToListAsync();

            filter.Items.AddRange(query);

            if (query.Any())
            {
                filter.ConditionOperator = query.First().ConditionOperator;
            }

            return filter;
        }

        public async Task UpdatQueryFilters(QueryFilterDto form)
        {
            var queryFilterFormItems = form.Items;

            try
            {
                var entity = await this.GetQuery<IFQuery>().SingleOrDefaultAsync(q => q.Id == form.QueryId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }


                var queryFilterItems = await this.GetQuery<IFQueryFilterItem>(f => f.QueryId == form.QueryId && f.ParentId == form.ParentId).ToListAsync();

                for (int i = 0; i < queryFilterItems.Count; i++)
                {
                    if (!queryFilterFormItems.Any(d => d.Id == queryFilterItems.ElementAt(i).Id))
                    {
                        this.Delete(queryFilterItems.ElementAt(i));
                    }
                }


                foreach (var queryFilterForm in queryFilterFormItems)
                {

                    if (queryFilterForm.Id <= 0)
                    {
                        IFQueryFilterItem filter = new IFQueryFilterItem();

                        if (queryFilterForm.FormModelPropertyId.HasValue)
                        {
                            filter.FormModelPropertyId = queryFilterForm.FormModelPropertyId;
                            filter.Value = null;
                        }
                        else if (queryFilterForm.PageParameterId.HasValue)
                        {
                            filter.IFPageParameterId = queryFilterForm.PageParameterId;
                            filter.Value = null;
                        }
                        else if (queryFilterForm.Value != null)
                        {
                            filter.Value = queryFilterForm.Value;
                        }
                        else
                        {
                            filter.Value = null;
                            filter.IFPageParameterId = null;
                            filter.FormModelPropertyId = null;
                        }

                        filter.QueryId = form.QueryId;
                        filter.ConditionOperator = form.ConditionOperator;
                        filter.EntityPropertyId = queryFilterForm.EntityPropertyId;
                        filter.FilterOperator = queryFilterForm.FilterOperator;
                        filter.IsNullCheck = queryFilterForm.IsNullCheck;
                        filter.ParentId = form.ParentId;

                        this.Add(filter);
                    }
                    else
                    {
                        var filter = await this.GetQuery<IFQueryFilterItem>(p => p.Id == queryFilterForm.Id).SingleOrDefaultAsync();

                        if (filter.FormModelPropertyId.HasValue)
                        {
                            filter.FormModelPropertyId = queryFilterForm.FormModelPropertyId;
                            filter.Value = "";
                        }
                        else if (queryFilterForm.PageParameterId.HasValue)
                        {
                            filter.IFPageParameterId = queryFilterForm.PageParameterId;
                            filter.Value = "";
                        }
                        else if (queryFilterForm.Value != null)
                        {
                            filter.Value = queryFilterForm.Value;
                        }
                        else
                        {
                            filter.Value = null;
                            filter.IFPageParameterId = null;
                            filter.FormModelPropertyId = null;
                        }
                        filter.QueryId = form.QueryId;
                        filter.ConditionOperator = form.ConditionOperator;
                        filter.EntityPropertyId = queryFilterForm.EntityPropertyId;
                        filter.FilterOperator = queryFilterForm.FilterOperator;
                        filter.IsNullCheck = queryFilterForm.IsNullCheck;
                        filter.ParentId = form.ParentId;
                        this.Update(filter);
                    }
                }

                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task UpdateOrderFilters(List<IFQueryOrder> dtos, int queryId)
        {
            try
            {
                var entity = await this.GetQuery<IFQuery>()
               .Include(e => e.QueryOrders)
               .SingleOrDefaultAsync(k => k.Id == queryId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                for (int i = 0; i < entity.QueryOrders.Count; i++)
                {
                    if (!dtos.Any(d => d.Id == entity.QueryOrders.ElementAt(i).Id))
                    {
                        this.Delete(entity.QueryOrders.ElementAt(i));
                    }
                }

                foreach (var dto in dtos)
                {

                    if (dto.Id <= 0)
                    {
                        IFQueryOrder property = new IFQueryOrder();
                        property.EntityPropertyId = dto.EntityPropertyId;
                        property.QueryId = queryId;
                        property.QueryOrderOperator = dto.QueryOrderOperator;
                        entity.QueryOrders.Add(property);
                    }
                    else
                    {
                        var property = entity.QueryOrders.SingleOrDefault(p => p.Id == dto.Id);
                        property.EntityPropertyId = dto.EntityPropertyId;
                        property.QueryId = queryId;
                        property.QueryOrderOperator = dto.QueryOrderOperator;
                        this.Update(property);
                    }
                }

                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Task<List<IFQueryOrder>> GetQueryOrders(int id)
        {
            return this.GetQuery<IFQueryOrder>(o => o.QueryId == id).ToListAsync();
        }

        public async Task<List<IFPageParameter>> GetPageParametersFromQuery(int queryId, PageParameterType objectType)
        {
            var query = await this.GetQuery<IFQuery>().Include(q => q.Process.Pages).SingleOrDefaultAsync();
            var parameters = await this.GetQuery<IFPageParameter>(p => p.ObjectId == queryId && p.ObjectType == objectType).ToListAsync();
            return parameters;
        }


        public async Task<List<IFPageParameter>> GetPageParametersFromQuery(int id)
        {
            var parameters = await this.GetQuery<IFPageParameter>()
                .ToListAsync();
            return parameters;
        }
    }

}
