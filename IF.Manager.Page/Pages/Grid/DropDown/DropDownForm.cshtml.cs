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
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Page.Pages.Form.DropDown
{
    public class DropDownGridModel : PageModel
    {


        private readonly IQueryService queryService;
        private readonly IPageService pageService;
        private readonly IPageFormService pageFormService;

        public DropDownGridModel(IQueryService queryService, IPageService pageService, IPageFormService pageFormService)
        {
            this.queryService = queryService;
            this.pageService = pageService;
            this.pageFormService = pageFormService;
        }

        [BindProperty(SupportsGet = true), Required]
        public IFPageControlItemModelProperty Form { get; set; }

        public async Task OnGet()
        {
            if (this.Form.IFQueryId > 0)
            {
                this.Form = await this.pageService.GetPageControlItemModelProperty(this.Form.Id);
                await this.SetQueryModelProperties();
            }

            await this.SetFormDefaults();

        }

      

        public async Task<IActionResult> OnPostSaveAsync()
        {

           
                await this.pageService.UpdatePageControlItemModelProperty(this.Form);
           

            return new EmptyResult();
        }

        public async Task<PartialViewResult> OnGetDropDownPropertyPartialAsync()
        {
            await SetQueryModelProperties();

            return new PartialViewResult
            {
                ViewName = "_DropDownProperty",
                ViewData = new ViewDataDictionary<IFPageControlItemModelProperty>(ViewData, this.Form)
            };

        }


        private async Task SetFormDefaults()
        {
            await SetQueries();
            //await SetModels();
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

        private async Task SetQueryModelProperties()
        {

            var properties = await this.queryService.GetQueryModelPropertyList(this.Form.IFQueryId.Value);

            List<SelectListItem> name_items = new List<SelectListItem>();


            foreach (var property in properties)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.NameIFModelPropertyId == property.Id)
                {
                    item.Selected = true;
                }

                item.Text = property.EntityProperty.Name;
                item.Value = property.Id.ToString();
                name_items.Add(item);
            }

            ViewData["name_models"] = name_items;



            List<SelectListItem> value_items = new List<SelectListItem>();


            foreach (var data in properties)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.ValueIFModelPropertyId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.EntityProperty.Name;
                item.Value = data.Id.ToString();
                value_items.Add(item);
            }

            ViewData["value_models"] = value_items;
        }
    }
}
