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
    
    
    public class QueryOrderModel : PageModel
    {

        private readonly IEntityService entityService;
        private readonly IQueryService queryService;
        private readonly IModelService modelService;

        [BindProperty, Required]
        public List<IFQueryOrder> Form { get; set; }


        [BindProperty(SupportsGet = true), Required]
        public int QueryId { get; set; }


        public QueryOrderModel(IEntityService entityService, IQueryService queryService, IModelService modelService)
        {
            this.entityService = entityService;
            this.queryService = queryService;
            this.modelService = modelService;
        }


        public async Task OnGet(int Id)
        {

            this.Form = await this.queryService.GetQueryOrders(Id);
            this.QueryId = Id;

            if (!this.Form.Any())
            {
                SetEmptyForm(Id);
            }

            await SetFromDefaults(Id);

        }



        public async Task<PartialViewResult> OnGetEmptyFormItemPartialAsync(int Id)
        {
            await SetFromDefaults(Id);

            var emptyFormItem = new IFQueryOrder();

            return new PartialViewResult
            {
                ViewName = "_QueryOrderFormItem",
                ViewData = new ViewDataDictionary<IFQueryOrder>(ViewData, emptyFormItem)
            };
        }

        public async Task<PartialViewResult> OnPost()
        {
            await this.queryService.UpdatOrderFilters(this.Form,this.QueryId);


            var QueryList = await this.queryService.GetQueryList();
            

            return new PartialViewResult
            {
                ViewName = "_QueryListTable",
                ViewData = new ViewDataDictionary<List<QueryDto>>(ViewData, QueryList)
            };

        }

        private void SetEmptyForm(int Id)
        {
            var order = new IFQueryOrder();
            this.QueryId = Id;
            this.Form = new List<IFQueryOrder>();
            this.Form.Add(order);            

        }

        private async Task SetFromDefaults(int Id)
        {
            var query = await this.queryService.GetQuery(Id);

            var model = await this.modelService.GetModel(query.ModelId);

            var entities = await this.entityService.GetEntityAllRelations(model.EntityId);

            SetEntities(entities);
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
