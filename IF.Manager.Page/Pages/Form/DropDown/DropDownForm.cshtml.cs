using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IF.Manager.Page.Pages.Form.DropDown
{
    public class DropDownFormModel : PageModel
    {


        private readonly IQueryService queryService;

        public DropDownFormModel(IQueryService queryService)
        {
            this.queryService = queryService;
        }

        [BindProperty, Required]
        public IFPageControlItemModelProperty Form { get; set; }
        
        public async Task OnGet()
        {            
            await this.SetFormDefaults();
        }

        private async Task SetFormDefaults()
        {
            await SetQueries();
        }

        private async Task SetQueries()
        {
            var entities = await this.queryService.GetQueryList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in entities)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.IFQueryId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["queries"] = items;
        }
    }
}
