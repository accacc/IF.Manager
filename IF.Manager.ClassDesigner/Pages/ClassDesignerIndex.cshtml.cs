using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.ClassDesigner.Pages
{
    public class ClassManagerIndexModel : PageModel
    {
        private readonly IClassService ClassService;

        public List<List<EntityDto>> ClassList { get; set; }

        public ClassManagerIndexModel(IClassService ClassService)
        {
            this.ClassService = ClassService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }        

        public async Task<PartialViewResult> OnGetClassListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_ClassListTable",
                ViewData = new ViewDataDictionary<List<List<EntityDto>>>(ViewData, this.ClassList)
            };

        }

        private async Task SetModel()
        {
            var list = await this.ClassService.GetEntityListGrouped();

            this.ClassList = list;
        }
    }
}
