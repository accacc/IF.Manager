using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
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

        public async Task AddQuery(QueryDto form)
        {
            IFQuery entity = new IFQuery();

            string name = DirectoryHelper.AddAsLastWord(form.Name, "DataQuery");

            entity.Id = form.Id;
            entity.Name = name;
            entity.ModelId = form.ModelId;
            entity.ProcessId = form.ProcessId;
            entity.Description = form.Description;
            entity.QueryGetType = form.QueryGetType;
            entity.PageNumber = form.PageNumber;
            entity.PageSize = form.PageSize;
            entity.IsQueryOverride = form.IsQueryOverride;
            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();
            form.Id = entity.Id;
        }

        public async Task UpdateQuery(QueryDto form)
        {

            try
            {
                var entity = await this.GetQuery<IFQuery>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }
                string name = DirectoryHelper.AddAsLastWord(form.Name, "DataQuery");
                entity.Name = name;
                entity.Description = form.Description;
                entity.ModelId = form.ModelId;
                entity.ProcessId = form.ProcessId;
                entity.QueryGetType = form.QueryGetType;
                entity.PageNumber = form.PageNumber;
                entity.PageSize = form.PageSize;
                entity.IsQueryOverride = form.IsQueryOverride;

                //if (entity.QueryGetType == Contracts.Enum.QueryGetType.List)
                //{
                //    entity.PageSize = form.PageSize;
                //}

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


        //public async Task<List<IFFormModel>> GetFormModelList()
        //{
        //    var data = await this.GetQuery<IFFormModel>().ToListAsync();

        //    return data;
        //}

        //public async Task<List<IFFormModelProperty>> GetFormModelPropertyList(int formModelId)
        //{
        //    var data = await this.GetQuery<IFFormModelProperty>(x=>x.FormModelId == formModelId).ToListAsync();

        //    return data;
        //}

        public async Task<List<IFModelProperty>> GetQueryModelPropertyList(int queryId)
        {
            var query = await this.GetQuery<IFQuery>(q => q.Id == queryId).SingleOrDefaultAsync();

            var data = await this.GetQuery<IFModelProperty>(x => x.ModelId == query.ModelId).Include(p=>p.EntityProperty).ToListAsync();

            return data;
        }

        public async Task<QueryFilterDto> GetQueryFilterItems(int queryId)
        {
            QueryFilterDto filter = new QueryFilterDto();
            var query = await this.GetQuery<IFQueryFilterItem>(f => f.QueryId == queryId)


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
                    Id = i.Id

                }).ToListAsync();

            filter.Items.AddRange(query);

            if (query.Any())
            {

                filter.ConditionOperator = query.First().ConditionOperator;
                filter.QueryId = query.First().QueryId;
            }

            return filter;
        }

        public async Task UpdatQueryFilters(QueryFilterDto form)
        {
            var dtos = form.Items;

            try
            {
                var entity = await this.GetQuery<IFQuery>()
               .Include(e => e.QueryFilterItems)
               .SingleOrDefaultAsync(q => q.Id == form.QueryId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }


                for (int i = 0; i < entity.QueryFilterItems.Count; i++)
                {
                    if (!dtos.Any(d => d.Id == entity.QueryFilterItems.ElementAt(i).Id))
                    {
                        this.Delete(entity.QueryFilterItems.ElementAt(i));
                    }
                }


                foreach (var dto in dtos)
                {

                    if (dto.Id <= 0)
                    {
                        IFQueryFilterItem filter = new IFQueryFilterItem();
                        
                        if (dto.FormModelPropertyId.HasValue)
                        {
                            filter.FormModelPropertyId = dto.FormModelPropertyId;
                            filter.Value = null;
                        }
                        else if (dto.PageParameterId.HasValue)
                        {
                            filter.IFPageParameterId = dto.PageParameterId;
                            filter.Value = null;
                        }
                        else if(dto.Value!=null)
                        {
                            filter.Value = dto.Value;
                        }
                        else
                        {
                            filter.Value = null;
                            filter.IFPageParameterId = null;
                            filter.FormModelPropertyId = null;
                        }

                        filter.QueryId = form.QueryId;
                        filter.ConditionOperator = form.ConditionOperator;
                        filter.EntityPropertyId = dto.EntityPropertyId;
                        filter.FilterOperator = dto.FilterOperator;

                        this.Add(filter);
                    }
                    else
                    {
                        var filter = await this.GetQuery<IFQueryFilterItem>(p => p.Id == dto.Id).SingleOrDefaultAsync();

                        if (filter.FormModelPropertyId.HasValue)
                        {
                            filter.FormModelPropertyId = dto.FormModelPropertyId;
                            filter.Value = "";
                        }
                        else if (dto.PageParameterId.HasValue)
                        {
                            filter.IFPageParameterId = dto.PageParameterId;
                            filter.Value = "";
                        }
                        else if (dto.Value != null)
                        {
                            filter.Value = dto.Value;
                        }
                        else
                        {
                            filter.Value = null;
                            filter.IFPageParameterId = null;
                            filter.FormModelPropertyId = null;
                        }
                        filter.QueryId = form.QueryId;
                        filter.ConditionOperator = form.ConditionOperator;
                        filter.EntityPropertyId = dto.EntityPropertyId;
                        filter.FilterOperator = dto.FilterOperator;

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

        public async Task UpdatOrderFilters(List<IFQueryOrder> dtos, int queryId)
        {
            try
            {
                var entity = await this.GetQuery<IFQuery>()
               .Include(e => e.QueryOrders)
               .SingleOrDefaultAsync(k => k.Id == queryId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                for (int i = 0; i < entity.QueryOrders.Count; i++)
                {
                    if (!dtos.Any(d=>d.Id == entity.QueryOrders.ElementAt(i).Id))
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

        public async Task<List<IFPageParameter>> GetPageParametersFromQuery(int queryId,PageParameterType objectType)
        {
            var query = await this.GetQuery<IFQuery>().Include(q => q.Process.Pages).SingleOrDefaultAsync();
            var parameters = await this.GetQuery<IFPageParameter>(p => p.ObjectId == queryId && p.ObjectType == objectType).ToListAsync();
            return parameters;
        }


        public async Task<List<IFPageParameter>> GetPageParametersFromQuery(int id)
        {
            var parameters = await this.GetQuery<IFPageParameter>()
                //.Where(p => p.IFPage.Process.Queries.Any(q => q.Id == id))
                .ToListAsync();
            return parameters;
        }
    }

}
