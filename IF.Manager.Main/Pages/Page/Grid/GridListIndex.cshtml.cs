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

namespace IF.Manager.Page.Pages.Grid
{
    public class GridListIndexModel : PageModel
    {
        private readonly IPageGridService gridService;

        public List<IFPageGrid> GridList { get; set; }

        public GridListIndexModel(IPageGridService gridService)
        {
            this.gridService = gridService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetGridListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_GridListTable",
                ViewData = new ViewDataDictionary<List<IFPageGrid>>(ViewData, this.GridList)
            };

        }

        private async Task SetModel()
        {
            this.GridList = await this.gridService.GetGridList();
        }
    }
}
