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

namespace IF.Manager.Query.Pages.Filter
{
    

    public class QueryFilterModel : PageModel
    {

        private readonly IEntityService entityService;
        private readonly IQueryService queryService;
        private readonly IModelService modelService;
        private readonly IPageService pageService;

        [BindProperty, Required]
        public QueryFilterDto Form { get; set; }

        
        

        public QueryFilterModel(IEntityService entityService, IQueryService queryService, IModelService modelService,IPageService pageService)
        {
            
            this.entityService = entityService;
            this.queryService = queryService;
            this.modelService = modelService;
            this.pageService = pageService;
        }


        public async Task OnGet(int Id)
        {

            this.Form = await this.queryService.GetQueryFilterItems(Id);

            if (!this.Form.Items.Any())
            {
                SetEmptyForm(Id);
            }

            await SetFromDefaults(Id);

        }



        public async Task<PartialViewResult> OnGetEmptyFormItemPartialAsync(int Id)
        {
            await SetFromDefaults(Id);

            var emptyFormItem = new QueryFilterItemDto();

            return new PartialViewResult
            {
                ViewName = "_QueryFilterFormItem",
                ViewData = new ViewDataDictionary<QueryFilterItemDto>(ViewData, emptyFormItem)
            };
        }

        public async Task<PartialViewResult> OnPost()
        {
            await this.queryService.UpdatQueryFilters(this.Form);


            var QueryList = await this.queryService.GetQueryList();
            

            return new PartialViewResult
            {
                ViewName = "_QueryListTable",
                ViewData = new ViewDataDictionary<List<QueryDto>>(ViewData, QueryList)
            };

        }

        private void SetEmptyForm(int Id)
        {
            this.Form = new QueryFilterDto();
            this.Form.QueryId = Id;
            var item = new QueryFilterItemDto();
            
            this.Form.Items.Add(item);            

        }

        private async Task SetFromDefaults(int Id)
        {
            var query = await this.queryService.GetQuery(Id);

            var model = await this.modelService.GetModel(query.ModelId);

            var entities = await this.entityService.GetEntityAllRelations(model.EntityId);


            SetEntities(entities);
            await SetPageParameters(query);

        }

        private async Task SetPageParameters(QueryDto query)
        {

            List<SelectListItem> formModelItems = new List<SelectListItem>();

            List<IFPageParameter> parameters = await this.queryService.GetPageParametersFromQuery(query.Id);



            foreach (var property in parameters)
            {
                SelectListItem item = new SelectListItem();
                item.Text = property.Name;
                item.Value = property.Id.ToString();
                formModelItems.Add(item);
            }




            ViewData["page_parameters"] = formModelItems;
        }

     
       
        private void SetEntities(List<EntityDto> entities)
        {
            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var entity in entities)
            {
                foreach (var property in entity.Properties)
                {
                    SelectListItem item = new SelectListItem();

                    //if (this.Form.ModelId == group.Id)
                    //{
                    //    item.Selected = true;
                    //}

                    item.Text = entity.Name + " - " + property.Name;
                    item.Value = property.Id.ToString();
                    items.Add(item);
                }


            }

            ViewData["entities"] = items;
        }
    }
}
