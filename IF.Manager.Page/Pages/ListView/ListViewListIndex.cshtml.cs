using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Page.Pages.ListView
{
    public class ListViewListIndexModel : PageModel
    {
        private readonly IPageListViewService ListViewService;

        public List<IFPageListView> ListViewList { get; set; }

        public ListViewListIndexModel(IPageListViewService ListViewService)
        {
            this.ListViewService = ListViewService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetListViewListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_ListViewListTable",
                ViewData = new ViewDataDictionary<List<IFPageListView>>(ViewData, this.ListViewList)
            };

        }

        private async Task SetModel()
        {
            this.ListViewList = await this.ListViewService.GetListViewList();
        }
    }
}
