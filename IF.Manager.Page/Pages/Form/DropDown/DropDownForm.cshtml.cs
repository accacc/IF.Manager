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
    public class DropDownFormModel : PageModel
    {


        private readonly IQueryService queryService;
        private readonly IModelService modelService;
        private readonly IPageFormService pageFormService;

        public DropDownFormModel(IQueryService queryService, IModelService modelService, IPageFormService pageFormService)
        {
            this.queryService = queryService;
            this.modelService = modelService;
            this.pageFormService = pageFormService;
        }

        [BindProperty(SupportsGet = true), Required]
        public IFPageControlItemModelProperty Form { get; set; }

        public async Task OnGet()
        {
            if (this.Form.IFPageFormItemModelPropertyId > 0)
            {
                var entity = await this.pageFormService.GetPageControlItemModelProperty(this.Form.IFPageFormItemModelPropertyId);
                if (entity != null)
                {
                    this.Form = entity;
                }
            }
            await this.SetFormDefaults();

        }

        //public async Task OnGetUpdateAsync(int Id)
        //{
        //    this.Form = await this.pageFormService.GetPageControlItemModelProperty(Id);
        //    await this.SetFormDefaults();

        //}

        public async Task<IActionResult> OnPostSaveAsync()
        {

            if (Form.Id <= 0)
            {
                await this.pageFormService.AddPageControlItemModelProperty(this.Form);
            }
            else
            {
                await this.pageFormService.UpdatePageControlItemModelProperty(this.Form);
            }

            return new EmptyResult();
        }

        //public async Task<PartialViewResult> OnPostUpdateAsync()
        //{

        //    await this.pageFormService.UpdateForm(this.Form);


        //    var list = await this.pageFormService.GetFormList();

        //    return new PartialViewResult
        //    {
        //        ViewName = "_FormListTable",
        //        ViewData = new ViewDataDictionary<List<IFPageForm>>(ViewData, list)
        //    };
        //}


        public async Task<PartialViewResult> OnGetDropDownPropertyPartialAsync()
        {
            await SetModels();

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

        private async Task SetModels()
        {

            var properties = await this.queryService.GetQueryModelPropertyList(this.Form.IFQueryId);

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

                if (this.Form.NameIFModelPropertyId == data.Id)
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
