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

namespace IF.Manager.Page.Pages.Panel
{
    public class PanelListIndexModel : PageModel
    {
        private readonly IPageGridService gridService;

        public List<IFPagePanel> PanelList { get; set; }

        public PanelListIndexModel(IPageGridService gridService)
        {
            this.gridService = gridService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetPanelListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_PanelListTable",
                ViewData = new ViewDataDictionary<List<IFPagePanel>>(ViewData, this.PanelList)
            };

        }

        private async Task SetModel()
        {
            this.PanelList = await this.gridService.GetPanelList();
        }
    }
}
