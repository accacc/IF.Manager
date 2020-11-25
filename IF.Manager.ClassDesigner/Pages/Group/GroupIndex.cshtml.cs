using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Entity.Pages.Group
{
    public class GroupIndexModel : PageModel
    {

        private readonly IClassService classService;

        public List<EntityGroupDto> EntityGroupList { get; set; }

        public GroupIndexModel(IClassService entityService)
        {
            this.classService = entityService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetEntityGroupListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_EntityGroupListTable",
                ViewData = new ViewDataDictionary<List<EntityGroupDto>>(ViewData, this.EntityGroupList)
            };
            
        }

        private async Task SetModel()
        {
            this.EntityGroupList = await this.classService.GetEntityGroupList();
        }
    }
}
