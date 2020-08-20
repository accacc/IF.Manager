using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Entity.Pages
{
    public class FormModel : PageModel
    {

        private readonly IEntityService entityService;

        public FormModel(IEntityService entityService)
        {
            this.entityService = entityService;
        }

        [BindProperty, Required]
        public EntityDto Form { get; set; }
        public async Task OnGetAddAsync()
        {
            this.Form = new EntityDto();
            await this.SetFromDefaults();
            
        }

        public async Task OnGetUpdateAsync(int Id)
        {            
            this.Form = await this.entityService.GetEntity(Id);
            await this.SetFromDefaults();
        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.entityService.AddEntity(this.Form);


            var list = await this.entityService.GetEntityListGrouped();

            return new PartialViewResult
            {
                ViewName = "_EntityListTable",
                ViewData = new ViewDataDictionary<List<List<EntityDto>>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.entityService.UpdateEntity(this.Form);


            var list = await this.entityService.GetEntityListGrouped();

            return new PartialViewResult
            {
                ViewName = "_EntityListTable",
                ViewData = new ViewDataDictionary<List<List<EntityDto>>>(ViewData, list)
            };
        }

        private async Task SetFromDefaults() 
        {
            var groups = await this.entityService.GetEntityGroupList();
            
            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var group in groups)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.GroupId.HasValue && this.Form.GroupId == group.Id)
                {
                    item.Selected = true;
                }

                item.Text = group.Name;
                item.Value = group.Id.ToString();
                items.Add(item);
            }
            
            ViewData["groups"] = items;
        }
    }
}
