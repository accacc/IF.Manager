using IF.Core.Persistence;
using IF.Manager.Contracts.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface ICommandService: IRepository
    {

        //Task<List<IFFormModelProperty>> GetFormModelPropertyList(int formModelId);
        //Task<List<IFFormModel>> GetFormModelList();
        Task<List<IFCommandFilterItem>> GetCommandFilterItems(int CommandId);

        Task AddCommand(IFCommand form);

        Task<IFCommand> GetCommand(int id);
        Task UpdateCommand(IFCommand form);
        Task<List<IFCommand>> GetCommandList();
        //Task SaveFilter(int id,CommandBuilderFilterRule data);
        Task UpdateCommandFilters(List<IFCommandFilterItem> form,int CommandId);
        
    }
}
