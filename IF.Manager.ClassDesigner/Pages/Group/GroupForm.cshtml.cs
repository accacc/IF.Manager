using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.Entity.Pages.Group
{
    public class GroupFormModel : PageModel
    {
        private readonly IEntityService entityService;

        public GroupFormModel(IEntityService entityService)
        {
            this.entityService = entityService;
        }

        [BindProperty, Required]
        public EntityGroupDto Form { get; set; }
        public void OnGetAddAsync()
        {            
            this.Form = new EntityGroupDto();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.entityService.GetEntityGroup(Id);

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.entityService.AddEntityGroup(this.Form);


            var list = await this.entityService.GetEntityGroupList();

            return new PartialViewResult
            {
                ViewName = "_EntityGroupListTable",
                ViewData = new ViewDataDictionary<List<EntityGroupDto>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.entityService.UpdateEntityGroup(this.Form);


            var list = await this.entityService.GetEntityGroupList();

            return new PartialViewResult
            {
                ViewName = "_EntityGroupListTable",
                ViewData = new ViewDataDictionary<List<EntityGroupDto>>(ViewData, list)
            };
        }
    }
}
