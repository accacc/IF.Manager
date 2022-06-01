using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Project.Pages
{
    public class ProcessIndexModel : PageModel
    {

        private readonly IProjectService projectService;

        public List<ProcessDto> ProcessList { get; set; }

        public ProcessIndexModel(IProjectService ProjectService)
        {
            this.projectService = ProjectService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetProcessListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_ProcessListTable",
                ViewData = new ViewDataDictionary<List<ProcessDto>>(ViewData, this.ProcessList)
            };

        }

        private async Task SetModel()
        {
            this.ProcessList = await this.projectService.GetProcessList();
        }
    }
}
