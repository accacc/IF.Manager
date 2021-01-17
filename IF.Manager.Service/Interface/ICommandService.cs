using IF.Core.Persistence;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service.Model;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface ICommandService: IRepository
    {
        Task<List<IFCommandFilterItem>> GetCommandFilterItems(int CommandId);

        Task<List<CommandControlTreeDto>> GetCommandTreeList(int ParentId);
        Task AddCommand(IFCommand form);

        Task<IFCommand> GetCommand(int id);
        Task UpdateCommand(IFCommand form);
        Task<List<IFCommand>> GetCommandList();
        Task UpdateCommandFilters(List<IFCommandFilterItem> form,int CommandId);
        Task<List<IFCommand>> GetCommandMultiItems(int Id);
        Task UpdateCommandMulties(List<IFCommand> form, int commandId);
    }
}
