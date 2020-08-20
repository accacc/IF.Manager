using IF.Manager.Contracts.Dto;
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
    public class PublishProjectFormModel : PageModel
    {



        [BindProperty, Required]
        public PublishDto Form { get; set; }

        private readonly IEntityService entityService;
        private readonly IProjectService projectService;

        public PublishProjectFormModel(IEntityService entityService, IProjectService projectService)
        {
            this.entityService = entityService;
            this.projectService = projectService;
        }


        //public void OnGetAsync(int ProcessId)
        //{
        //    this.Form = new PublishDto();
        //    this.Form.ProcessId = ProcessId;

        //}


        public void OnGetAsync(int ProjectId)
        {
            this.Form = new PublishDto();
            this.Form.ProjectId = ProjectId;

        }




        public async Task<PartialViewResult> OnPostAsync()
        {

            try
            {
                await this.projectService.PublishProject(this.Form);
            }
            catch (Exception ex)
            {

                throw;
            }

            var list = await this.entityService.GetEntityListGrouped();

            return new PartialViewResult
            {
                ViewName = "_EntityListTable",
                ViewData = new ViewDataDictionary<List<List<EntityDto>>>(ViewData, list)
            };
        }

     

        public async Task<PartialViewResult> OnGetListAsync(int ProcessId)
        {
            var PublishList = await this.projectService.GetPublishList(ProcessId);

            return new PartialViewResult
            {
                ViewName = "_PublishHistory",
                ViewData = new ViewDataDictionary<List<PublishDto>>(ViewData, PublishList)
            };

        }

    }
}
