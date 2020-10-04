using IF.Core.Persistence;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IPageGridService: IRepository
    {
        Task<List<IFPageGrid>> GetGridList();
        Task AddGrid(IFPageGrid form);
        Task UpdateGrid(IFPageGrid form);
        Task<IFPageGrid> GetGrid(int id);


        Task<List<IFPagePanel>> GetPanelList();
        Task AddPanel(IFPagePanel form);
        Task UpdatePanel(IFPagePanel form);
        Task<IFPagePanel> GetPanel(int id);

        Task<List<IFPageGridLayout>> GetGridLayouts();
    }
}
