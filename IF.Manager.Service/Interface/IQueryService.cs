using IF.Core.Persistence;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IQueryService: IRepository
    {

        Task<List<QueryFilterTreeDto>> GetFilterTreeList(int queryId);
        Task<List<IFModelProperty>> GetQueryModelPropertyList(int queryId);
        Task<List<IFPageParameter>> GetPageParametersFromQuery(int queryId);

        //Task<List<IFFormModelProperty>> GetFormModelPropertyList(int formModelId);
        //Task<List<IFFormModel>> GetFormModelList();
        Task<QueryFilterDto> GetQueryFilterItems(int queryId,int? parentId);

        Task AddQuery(QueryDto form);

        Task<QueryDto> GetQuery(int id);
        Task UpdateQuery(QueryDto form);
        Task<List<QueryDto>> GetQueryList();
        //Task SaveFilter(int id,QueryBuilderFilterRule data);
        Task UpdatQueryFilters(QueryFilterDto form);
        Task UpdatOrderFilters(List<IFQueryOrder> form,int queryId);
        Task<List<IFQueryOrder>> GetQueryOrders(int id);
        Task<List<IFPageParameter>> GetPageParametersFromQuery(int queryId, PageParameterType objectType);
    }
}
