using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Page.Pages.Form.Model
{


    public class ModelFormModel : PageModel
    {

        private readonly IPageFormService pageFormService;
        private readonly IPageActionService pageActionService;
        private readonly IPageService pageService;
        private readonly IModelService modelService;
        private readonly IEntityService entityService;

        [BindProperty, Required]
        public List<IFPageFormItemModelProperty> Form { get; set; }


        [BindProperty(SupportsGet = true), Required]
        public int FormId { get; set; }


        public ModelFormModel(IPageFormService pageFormService, IPageService queryService, IModelService modelService, IEntityService entityService, IPageActionService pageActionService)
        {
            this.pageFormService = pageFormService;
            this.pageService = queryService;
            this.modelService = modelService;
            this.entityService = entityService;
            this.pageActionService = pageActionService;
        }



        public async Task OnGet(int Id)
        {

            this.Form = await this.pageFormService.GetPageFormItemModelProperties(Id);
            this.FormId = Id;

            if (!this.Form.Any())
            {
                SetEmptyForm(Id);
            }

            await SetFromDefaults(Id);

        }




        public async Task<PartialViewResult> OnGetEmptyModelItemPartialAsync(int Id)
        {
            await SetFromDefaults(Id);

            var emptyFormItem = new IFPageFormItemModelProperty();

            return new PartialViewResult
            {
                ViewName = "_ModelItem",
                ViewData = new ViewDataDictionary<IFPageFormItemModelProperty>(ViewData, emptyFormItem)
            };
        }

        public async Task<PartialViewResult> OnPost()
        {
            await this.pageFormService.UpdateFormItemModelProperties(this.Form, this.FormId);


            var FormList = await this.pageFormService.GetFormList();


            return new PartialViewResult
            {
                ViewName = "_FormListTable",
                ViewData = new ViewDataDictionary<List<IFPageForm>>(ViewData, FormList)
            };

        }

        private void SetEmptyForm(int Id)
        {
            var order = new IFPageFormItemModelProperty();
            this.FormId = Id;
            this.Form = new List<IFPageFormItemModelProperty>();
            this.Form.Add(order);

        }

        private async Task SetFromDefaults(int Id)
        {
            List<ModelPropertyDto> modelProperties = await this.pageFormService.GetFormModelProperties(Id);

            SetModelProperties(modelProperties);

            await SetFormItems();

        }



        private async Task SetFormItems()
        {
            List<IFPageFormItem> formItems = await this.pageFormService.GetFormItems();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in formItems)
            {
                SelectListItem item = new SelectListItem();
                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["form_items"] = items;
        }

        private void SetModelProperties(List<ModelPropertyDto> properties)
        {
            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var property in properties)
            {
                SelectListItem item = new SelectListItem();
                item.Text = property.EntityName + " - " + property.Name;

                item.Value = property.ModelPropertyId.ToString();
                items.Add(item);
            }

            ViewData["model_properties"] = items;
        }
    }
}
