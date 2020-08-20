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

        private async Task SetModel()
        {
            this.QueryList = await this.queryService.GetQueryList();
        }
    }
}
