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
    public class PublishProcessFormModel : PageModel
    {



        [BindProperty, Required]
        public PublishDto Form { get; set; }

        private readonly IEntityService entityService;
        private readonly IProjectService projectService;
        private readonly IPublishService publishService;

        public PublishProcessFormModel(IEntityService entityService, IProjectService projectService, IPublishService publishService)
        {
            this.entityService = entityService;
            this.projectService = projectService;
            this.publishService = publishService;
        }


        public void OnGetAsync(int ProcessId)
        {
            this.Form = new PublishDto();
            this.Form.ProcessId = ProcessId;

        }

        public async Task<PartialViewResult> OnPostAsync()
        {

            try
            {
                await this.publishService.PublishProcess(this.Form);
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
            var PublishList = await this.publishService.GetPublishList(ProcessId);

            return new PartialViewResult
            {
                ViewName = "_PublishHistory",
                ViewData = new ViewDataDictionary<List<PublishDto>>(ViewData, PublishList)
            };

        }

    }
}
