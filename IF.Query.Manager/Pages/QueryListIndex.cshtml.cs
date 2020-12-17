using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Query.Pages
{
    public class QueryListIndexModel : PageModel
    {
        private readonly IQueryService queryService;

        public List<QueryDto> QueryList { get; set; }

        public QueryListIndexModel(IQueryService queryService)
        {
            this.queryService = queryService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetQueryListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_QueryListTable",
                ViewData = new ViewDataDictionary<List<QueryDto>>(ViewData, this.QueryList)
            };

        }

        public async Task<PartialViewResult> OnGetFilterTreeListAsync(int Id)
        {
            QueryFilterMapModel model = new QueryFilterMapModel();


            var query = await this.queryService.GetQuery(Id);

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


            return new PartialViewResult
            {
                ViewName = "_FilterTreeMain",
                ViewData = new ViewDataDictionary<QueryFilterMapModel>(ViewData, model)
            };
        }

        private async Task SetModel()
        {
            this.QueryList = await this.queryService.GetQueryList();
        }
    }
}
