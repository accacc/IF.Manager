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



        public async Task OnGet()
        {

            this.Form = await this.pageFormService.GetPageFormItemModelProperties(this.FormId);
            

            if (!this.Form.Any())
            {
                SetEmptyForm(this.FormId);
            }

            await SetFormDefaults(this.FormId);

        }




        public async Task<PartialViewResult> OnGetEmptyModelItemPartialAsync()
        {
            await SetFormDefaults(this.FormId);

            var emptyFormItem = new IFPageFormItemModelProperty();
            emptyFormItem.IFPageForm = new IFPageForm();

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

        public async Task<PartialViewResult> OnPostMoveModelItemUpOneAsync(int Id)
        {
            this.pageFormService.MoveModelItemUp(Id);

            var propertyList = await this.pageFormService.GetPageFormItemModelProperties(this.FormId);

            await this.SetFormDefaults(this.FormId);

            return new PartialViewResult
            {
                ViewName = "_ModelItemList",
                ViewData = new ViewDataDictionary<List<IFPageFormItemModelProperty>>(ViewData, propertyList)
            };

        }

        public async Task<PartialViewResult> OnPostMoveModelItemDownOneAsync(int Id)
        {
            this.pageFormService.MoveModelItemDown(Id);            

            var propertyList = await this.pageFormService.GetPageFormItemModelProperties(this.FormId);

            await this.SetFormDefaults(this.FormId);

            return new PartialViewResult
            {
                ViewName = "_ModelItemList",
                ViewData = new ViewDataDictionary<List<IFPageFormItemModelProperty>>(ViewData, propertyList)
            };

        }

        private void SetEmptyForm(int Id)
        {
            var modelProperty = new IFPageFormItemModelProperty();
            modelProperty.IFPageForm = new IFPageForm();
            modelProperty.IFPageForm.Id = Id;
            this.FormId = Id;
            this.Form = new List<IFPageFormItemModelProperty>();            
            this.Form.Add(modelProperty);

        }

        private async Task SetFormDefaults(int Id)
        {           

            await SetModelProperties();
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

        private async Task SetModelProperties()
        {

            List<ModelPropertyDto> properties = await this.pageFormService.GetFormModelProperties(this.FormId);

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
