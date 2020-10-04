using IF.Core.Persistence;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IPageListViewService: IRepository
    {
        Task<List<IFPageListView>> GetListViewList();
        Task AddListView(IFPageListView form);
        Task UpdateListView(IFPageListView form);
        Task<IFPageListView> GetListView(int id);
        //Task<List<IFPageListViewLayout>> GetListViewLayouts();
        //Task<List<IFPageListViewItem>> GetListViewItems();
        //Task<List<IFPageListViewItemModelProperty>> GetPageListViewItemModelProperties(int id);
        //Task UpdateListViewItemModelProperties(List<IFPageListViewItemModelProperty> form, int formId);
    }
}
