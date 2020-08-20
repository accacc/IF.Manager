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

namespace IF.Manager.Page.Pages.Form
{
    public class FormListIndexModel : PageModel
    {
        private readonly IPageFormService FormService;

        public List<IFPageForm> FormList { get; set; }

        public FormListIndexModel(IPageFormService FormService)
        {
            this.FormService = FormService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetFormListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_FormListTable",
                ViewData = new ViewDataDictionary<List<IFPageForm>>(ViewData, this.FormList)
            };

        }

        private async Task SetModel()
        {
            this.FormList = await this.FormService.GetFormList();
        }
    }
}
