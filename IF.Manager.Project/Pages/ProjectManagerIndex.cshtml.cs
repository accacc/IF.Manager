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

namespace IF.Manager.Project.Pages
{
    public class ProjectIndexModel : PageModel
    {

        private readonly IProjectService projectService;

        public List<IFProject> ProjectList { get; set; }

        public ProjectIndexModel(IProjectService ProjectService)
        {
            this.projectService = ProjectService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetProjectListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_ProjectListTable",
                ViewData = new ViewDataDictionary<List<IFProject>>(ViewData, this.ProjectList)
            };

        }

        private async Task SetModel()
        {
            this.ProjectList = await this.projectService.GetProjectList();
        }
    }
}
