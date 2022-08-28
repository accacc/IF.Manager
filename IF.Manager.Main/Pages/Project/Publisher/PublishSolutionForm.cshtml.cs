using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.Project.Pages
{
    public class PublishSolutionFormModel : PageModel
    {



        [BindProperty, Required]
        public PublishDto Form { get; set; }

        private readonly IEntityService entityService;
        private readonly IProjectService projectService;
        private readonly IPublishService publishService;

        public PublishSolutionFormModel(IEntityService entityService, IProjectService projectService, IPublishService publishService)
        {
            this.entityService = entityService;
            this.projectService = projectService;
            this.publishService = publishService;
        }


        public void OnGetAsync(int SolutionId)
        {
            this.Form = new PublishDto();
            this.Form.SolutionId = SolutionId;

        }




        public async Task<PartialViewResult> OnPostAsync()
        {

            try
            {
                await this.publishService.PublishSolution(this.Form);
            }
            catch (Exception ex)
            {

                throw;
            }

            var solutionList = await this.projectService.GetSolutionList();

            return new PartialViewResult
            {
                ViewName = "../Solution/_SolutionListTable",
                ViewData = new ViewDataDictionary<List<IFSolution>>(ViewData, solutionList)
            };
        }

     

        public async Task<PartialViewResult> OnGetListAsync(int ProcessId)
        {
            var PublishList = await this.publishService.GetPublishList(ProcessId);

            return new PartialViewResult
            {
                ViewName = "_PublishHistory",
                ViewData = new ViewDataDictionary<List<PublishDto>>(ViewData, PublishList)
            };

        }

    }
}
