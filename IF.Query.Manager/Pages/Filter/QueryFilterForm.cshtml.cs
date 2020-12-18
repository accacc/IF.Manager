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

        //public async Task<PartialViewResult> OnGetEmptyFormGroupPartialAsync()
        //{

        //    SetEmptyForm(this.Form.QueryId);
        //    await SetFormDefaults(this.Form.QueryId);
        //    //QueryFilterDto emptyFormItem = new QueryFilterDto();

        //    return new PartialViewResult
        //    {
        //        ViewName = "QueryFilterForm",
        //        ViewData = new ViewDataDictionary<QueryFilterModel>(ViewData, this)
        //    };
        //}


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


            QueryFilterMapModel model = await GetTreeModel(this.Form.QueryId);

            return new PartialViewResult
            {
                ViewName = "_FilterTreeMain",
                ViewData = new ViewDataDictionary<ClassMapModel>(ViewData, model)
            };

        }

        private async Task<QueryFilterMapModel> GetTreeModel(int QueryId)
        {
            QueryFilterMapModel model = new QueryFilterMapModel();


            var query = await this.queryService.GetQuery(QueryId);

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

        //private void SetEmptyForm(int queryId,int? ParentId)
        //{
        //    this.Form = new QueryFilterDto();
        //    this.Form.QueryId = queryId;
        //    this.Form.ParentId = ParentId;
        //    var item = new QueryFilterItemDto();

        //    this.Form.Items.Add(item);            

        //}

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
