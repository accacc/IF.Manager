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
    public class ProjectFormModel : PageModel
    {
        private readonly IProjectService projectService;

        public ProjectFormModel(IProjectService ProjectService)
        {
            this.projectService = ProjectService;
        }

        [BindProperty, Required]
        public IFProject Form { get; set; }
        public void OnGetAddAsync(int SolutionId)
        {            
            this.Form = new IFProject();
            this.Form.SolutionId = SolutionId;
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.projectService.GetProject(Id);

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.projectService.AddProject(this.Form);


            var list = await this.projectService.GetProjectList();

            return new PartialViewResult
            {
                ViewName = "_ProjectListTable",
                ViewData = new ViewDataDictionary<List<IFProject>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.projectService.UpdateProject(this.Form);


            var list = await this.projectService.GetProjectList();

            return new PartialViewResult
            {
                ViewName = "_ProjectListTable",
                ViewData = new ViewDataDictionary<List<IFProject>>(ViewData, list)
            };
        }
    }
}
