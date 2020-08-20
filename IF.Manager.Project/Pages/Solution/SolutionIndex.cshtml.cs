using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Project.Pages.Solution
{
    public class SolutionIndexModel : PageModel
    {

        private readonly IProjectService projectService;

        public List<IFSolution> SolutionList { get; set; }

        public SolutionIndexModel(IProjectService project)
        {
            this.projectService = project;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetSolutionListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_SolutionListTable",
                ViewData = new ViewDataDictionary<List<IFSolution>>(ViewData, this.SolutionList)
            };

        }

        private async Task SetModel()
        {
            this.SolutionList = await this.projectService.GetSolutionList();
        }
    }
}
