using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.CodeGen.Rules.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Query.Pages.Filter
{
    public class ShowFilterModel
    {
        public string Filter { get; set; }
    }
    public class QueryFilterModel : PageModel
    {

        private readonly IEntityService entityService;
        private readonly IQueryService queryService;
        private readonly IModelService modelService;
        private readonly IPageService pageService;

        [BindProperty(SupportsGet =true), Required]
        public QueryFilterDto Form { get; set; }

        
        

      public QueryFilterModel(IEntityService entityService, IQueryService queryService, IModelService modelService,IPageService pageService)
        {
            
            this.entityService = entityService;
            this.queryService = queryService;
            this.modelService = modelService;
            this.pageService = pageService;
        }


        public async Task OnGet()
        {

            this.Form = await this.queryService.GetQueryFilterItems(this.Form.QueryId,this.Form.ParentId);

            await SetFormDefaults(this.Form.QueryId);

            if (!this.Form.Items.Any())
            {
                this.Form.Items.Add(new QueryFilterItemDto());
            }

         

        }

        public async Task<PartialViewResult> OnGetEmptyFormItemPartialAsync(int Id)
        {
            await SetFormDefaults(Id);

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


            QueryFilterMapModel model = await GetTreeModel();

            return new PartialViewResult
            {
                ViewName = "_FilterTreeMain",
                ViewData = new ViewDataDictionary<QueryFilterMapModel>(ViewData, model)
            };

        }

        public async Task<PartialViewResult> OnGetShow()
        {
            

            QueryFilterMapModel model = await GetTreeModel();

            ShowFilterModel showFilterModel = new ShowFilterModel();


            FilterContext filterContext = new FilterContext();
            filterContext.FilterItems = model.Tree.ToList();
            FilterRuleEngine filterRuleEngine = new FilterRuleEngine(filterContext);
            filterRuleEngine.Execute();

            showFilterModel.Filter = filterRuleEngine.GetFilter();

            return new PartialViewResult
            {
                ViewName = "_ShowFilter",
                ViewData = new ViewDataDictionary<ShowFilterModel>(ViewData, showFilterModel)
            };

        }

        private async Task<QueryFilterMapModel> GetTreeModel()
        {
            QueryFilterMapModel model = new QueryFilterMapModel();


            var query = await this.queryService.GetQuery(this.Form.QueryId);

            if (query != null)
            {
                var tree = await this.queryService.GetFilterTreeList(query.Id);


                model.Tree = tree;
                model.QueryId = query.Id;
            }
            else
            {
                model.Tree = new List<QueryFilterTreeDto>();
            }


            return model;
        }

        private async Task SetFormDefaults(int queryId)
        {
            var query = await this.queryService.GetQuery(queryId);

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

                    item.Text = entity.Name + " - " + property.Name;
                    item.Value = property.Id.ToString();
                    items.Add(item);
                }


            }

            ViewData["entities"] = items;
        }
    }
}
