using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Page.Pages.ListView.Model
{
    

    public class ModelFormModel : PageModel
    {

        private readonly IPageFormService pageFormService;
        private readonly IPageActionService pageActionService;
        private readonly IPageService pageService;
        private readonly IModelService modelService;
        private readonly IEntityService entityService;

        [BindProperty, Required]
        public List<IFPageControlItemModelProperty> Form { get; set; }


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

            this.Form = await this.pageService.GetPageControlItemModelProperties(Id);
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

            var emptyFormItem = new IFPageControlItemModelProperty();

            return new PartialViewResult
            {
                ViewName = "_ModelItem",
                ViewData = new ViewDataDictionary<IFPageControlItemModelProperty>(ViewData, emptyFormItem)
            };
        }

        public async Task<PartialViewResult> OnPost()
        {
            await this.pageService.UpdateControlItemModelProperties(this.Form, this.FormId);


            var FormList = await this.pageFormService.GetFormList();


            return new PartialViewResult
            {
                ViewName = "_FormListTable",
                ViewData = new ViewDataDictionary<List<IFPageForm>>(ViewData, FormList)
            };

        }

        private void SetEmptyForm(int Id)
        {
            var order = new IFPageControlItemModelProperty();
            this.FormId = Id;
            this.Form = new List<IFPageControlItemModelProperty>();
            this.Form.Add(order);

        }

        private async Task SetFromDefaults(int Id)
        {
            //var pageControl = await this.pageService.GetPageControlMapByControlId(Id);

            //var parentActionMap = await this.pageService.GetPageControlMap(pageControl.ParentId.Value);

            //var action = (IFPageAction)parentActionMap.IFPageControl;

            //var model = await this.pageActionService.GetModelByAction(action.Id);

            //var modelProperties = await this.modelService.GetModelPropertyList(model.Id);

            //SetModelProperties(modelProperties);

            //await SetFormItems();

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
                //TODO:EntityPropertyId degil de ModelPropertyId(IFPageActionRouteValue sayfasinda farkedildi bu durum orda ModelPropertyId kullanildi) olmali sanki.
                item.Value = property.EntityPropertyId.ToString();
                    items.Add(item);              
            }

            ViewData["model_properties"] = items;
        }
    }
}
