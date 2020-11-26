using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.ClassDesigner.Pages.ClassGroup
{
    public class GroupIndexModel : PageModel
    {

        private readonly IClassService classService;

        public List<EntityGroupDto> ClassGroupList { get; set; }

        public GroupIndexModel(IClassService classService)
        {
            this.classService = classService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetClassGroupListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_ClassGroupListTable",
                ViewData = new ViewDataDictionary<List<EntityGroupDto>>(ViewData, this.ClassGroupList)
            };
            
        }

        private async Task SetModel()
        {
            this.ClassGroupList = await this.classService.GetClassGroupList();
        }
    }
}
