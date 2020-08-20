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
    public class PublishMenuFormModel : PageModel
    {



        [BindProperty, Required]
        public PublishDto Form { get; set; }

        private readonly IEntityService entityService;
        private readonly IPublishService publishService;

        public PublishMenuFormModel(IEntityService entityService, IPublishService publishService)
        {
            this.entityService = entityService;
            this.publishService = publishService;
        }        

        public void OnGetAsync(int IFProjectId)
        {
            this.Form = new PublishDto();
            this.Form.ProjectId = IFProjectId;

        }

        public async Task OnPostAsync()
        {

            try
            {
                await this.publishService.PublishMenu(this.Form);
            }
            catch (Exception ex)
            {

                throw;
            }            
        }

     

        

    }
}
