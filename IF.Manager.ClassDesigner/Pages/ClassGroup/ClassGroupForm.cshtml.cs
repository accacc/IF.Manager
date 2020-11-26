using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.ClassDesigner.Pages.ClassGroup
{
    public class ClassGroupFormModel : PageModel
    {
        private readonly IClassService classService;

        public ClassGroupFormModel(IClassService classService)
        {
            this.classService = classService;
        }

        [BindProperty, Required]
        public EntityGroupDto Form { get; set; }
        public void OnGetAddAsync()
        {            
            this.Form = new EntityGroupDto();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.classService.GetClassGroup(Id);

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.classService.AddClassGroup(this.Form);


            var list = await this.classService.GetClassGroupList();

            return new PartialViewResult
            {
                ViewName = "_ClassGroupListTable",
                ViewData = new ViewDataDictionary<List<EntityGroupDto>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.classService.UpdateClassGroup(this.Form);


            var list = await this.classService.GetClassGroupList();

            return new PartialViewResult
            {
                ViewName = "_EntityGroupListTable",
                ViewData = new ViewDataDictionary<List<EntityGroupDto>>(ViewData, list)
            };
        }
    }
}
