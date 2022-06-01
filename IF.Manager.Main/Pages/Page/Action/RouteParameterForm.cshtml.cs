using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Page.Pages.Action
{


    public class RouteParameterFormModel : PageModel
    {

        private readonly IPageGridService pageGridService;
        private readonly IPageActionService pageActionService;
        private readonly IPageService pageService;
        private readonly IModelService modelService;
        private readonly IEntityService entityService;

        [BindProperty, Required]
        public List<IFPageActionRouteValue> Form { get; set; }


        [BindProperty(SupportsGet = true), Required]
        public int ActionId { get; set; }


        public RouteParameterFormModel(IPageGridService pageGridService, IPageService queryService, IModelService modelService, IEntityService entityService, IPageActionService pageActionService)
        {
            this.pageGridService = pageGridService;
            this.pageService = queryService;
            this.modelService = modelService;
            this.entityService = entityService;
            this.pageActionService = pageActionService;
        }



        public async Task OnGet(int Id)
        {

            this.Form = await this.pageActionService.GetPageActionRouteValues(Id);

            this.ActionId = Id;

            if (!this.Form.Any())
            {
                SetEmptyForm(Id);
            }

            await SetFromDefaults(Id);

        }
        



        public async Task<PartialViewResult> OnGetEmptyModelItemPartialAsync(int Id)
        {
            await SetFromDefaults(Id);

            var emptyFormItem = new IFPageActionRouteValue();

            return new PartialViewResult
            {
                ViewName = "_RouteParameterItem",
                ViewData = new ViewDataDictionary<IFPageActionRouteValue>(ViewData, emptyFormItem)
            };
        }

        public async Task<PartialViewResult> OnPost()
        {
            await this.pageActionService.UpdatePageActionRouteValues(this.Form, this.ActionId);


            var FormList = await this.pageActionService.GetActionList();


            return new PartialViewResult
            {
                ViewName = "_ActionListTable",
                ViewData = new ViewDataDictionary<List<IFPageAction>>(ViewData, FormList)
            };

        }

        private void SetEmptyForm(int Id)
        {
            var order = new IFPageActionRouteValue();
            this.ActionId = Id;
            this.Form = new List<IFPageActionRouteValue>();
            this.Form.Add(order);

        }

        private async Task SetFromDefaults(int Id)
        {
            var pageControl = await this.pageService.GetPageControlMapByControlId(Id);

            if (!pageControl.Childrens.Any())
            {
                throw new BusinessException("This action has no child,Please first set a child and add parameter to that child");
            }

            if (pageControl.Parent.IFPageControl is IFPageGrid)
            {
                var gridControl = (IFPageGrid)pageControl.Parent.IFPageControl;

                var grid = await this.pageGridService.GetGrid(gridControl.Id);

                var modelProperties = await this.modelService.GetModelPropertyList(grid.Query.ModelId);

                SetModelProperties(modelProperties);
            }            

            var childControl = pageControl.Childrens.First();

            if (childControl != null)
            {

                //if (childControl.IFPageControl is IFPage)
                //{
                //  var page = (IFPage)childControl.IFPageControl;

                await SetPageParameter(childControl.IFPageControlId);
                //}
            }



        }


        private async Task SetPageParameter(int Id)
        {

            

            var parameters = await this.pageService.GetPageParameters(Id);

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in parameters)
            {
                SelectListItem item = new SelectListItem();
                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["parameters"] = items;
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
