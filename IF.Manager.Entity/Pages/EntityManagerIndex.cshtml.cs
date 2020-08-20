using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.Entity.Pages
{
    public class EntityManagerIndexModel : PageModel
    {
        private readonly IEntityService entityService;

        public List<List<EntityDto>> EntityList { get; set; }

        public EntityManagerIndexModel(IEntityService entityService)
        {
            this.entityService = entityService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }        

        public async Task<PartialViewResult> OnGetEntityListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_EntityListTable",
                ViewData = new ViewDataDictionary<List<List<EntityDto>>>(ViewData, this.EntityList)
            };

            //return Partial("",this.EntityList);
        }

        private async Task SetModel()
        {
            var list = await this.entityService.GetEntityListGrouped();

            this.EntityList = list;
        }
    }
}
