using IF.Core.Persistence;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IPageActionService: IRepository
    {

        Task<List<IFPageActionRouteValue>> GetPageActionRouteValues(int id);
        Task<List<IFPageAction>> GetActionList();
        Task AddAction(IFPageAction form);
        Task UpdateAction(IFPageAction form);
        //Task<IFModel> GetModelByAction(int id);

        Task<IFPageAction> GetAction(int id);
        Task UpdatePageActionRouteValues(List<IFPageActionRouteValue> form, int actionId);
    }
}
