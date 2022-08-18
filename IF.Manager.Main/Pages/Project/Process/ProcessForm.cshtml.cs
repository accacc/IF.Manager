using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.Project.Pages
{
    public class ProcessFormModel : PageModel
    {
        private readonly IProjectService projectService;

        public ProcessFormModel(IProjectService ProjectService)
        {
            this.projectService = ProjectService;
        }

        [BindProperty, Required]
        public ProcessDto Form { get; set; }
        public void OnGetAddAsync(int ProjectId)
        {            
            this.Form = new ProcessDto();
            this.Form.ProjectId = ProjectId;
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.projectService.GetProcess(Id);

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.projectService.AddProcess(this.Form);


            var list = await this.projectService.GetSolutionList();

            return new PartialViewResult
            {
                ViewName = "~/Pages/Project/Solution/_SolutionListTable.cshtml",
                ViewData = new ViewDataDictionary<List<IFSolution>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.projectService.UpdateProcess(this.Form);


            var list = await this.projectService.GetSolutionList();

            return new PartialViewResult
            {
                ViewName = "~/Pages/Project/Solution/_SolutionListTable.cshtml",
                ViewData = new ViewDataDictionary<List<IFSolution>>(ViewData, list)
            };
        }
    }
}
