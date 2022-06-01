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

namespace IF.Manager.Page.Pages.Action
{
    public class ActionListIndexModel : PageModel
    {
        private readonly IPageActionService actionService;

        public List<IFPageAction> ActionList { get; set; }

        public ActionListIndexModel(IPageActionService actionService)
        {
            this.actionService = actionService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetActionListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_ActionListTable",
                ViewData = new ViewDataDictionary<List<IFPageAction>>(ViewData, this.ActionList)
            };

        }

        private async Task SetModel()
        {
            this.ActionList = await this.actionService.GetActionList();
        }
    }
}
