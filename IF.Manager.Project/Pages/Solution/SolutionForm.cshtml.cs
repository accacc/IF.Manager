using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.Project.Pages.Solution
{
    public class SolutionFormModel : PageModel
    {
        private readonly IProjectService projectService;

        public SolutionFormModel(IProjectService ProjectService)
        {
            this.projectService = ProjectService;
        }

        [BindProperty, Required]
        public IFSolution Form { get; set; }
        public void OnGetAddAsync()
        {            
            this.Form = new IFSolution();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.projectService.GetSolution(Id);

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            try
            {
                await this.projectService.AddSolution(this.Form);
            }
            catch (System.Exception ex)
            {

                throw;
            }


            var list = await this.projectService.GetSolutionList();

            return new PartialViewResult
            {
                ViewName = "_SolutionListTable",
                ViewData = new ViewDataDictionary<List<IFSolution>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.projectService.UpdateSolution(this.Form);


            var list = await this.projectService.GetSolutionList();

            return new PartialViewResult
            {
                ViewName = "_SolutionListTable",
                ViewData = new ViewDataDictionary<List<IFSolution>>(ViewData, list)
            };
        }
    }
}
