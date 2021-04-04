using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.Interface;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.Entity.Pages.BasicGenerator
{
    public class FormModel : PageModel
    {
        private readonly IEntityService entityService;
        private readonly IDbFirstService dbFirstService;
        private readonly IProjectService projectService;
        public FormModel(IEntityService entityService, IDbFirstService dbFirstService, IProjectService projectService)
        {
            this.entityService = entityService;
            this.dbFirstService = dbFirstService;
            this.projectService = projectService;
        }

        [BindProperty, Required]
        public int ProcessId  { get; set; }


        [BindProperty(SupportsGet =true), Required]
        public int EntityId { get; set; }


        public async Task OnGetAsync()
        {
            await this.SetFormDefaults();
        }


        private async Task SetFormDefaults()
        {
            await SetProceses();


        }


        private async Task SetProceses()
        {
            var process = await this.projectService.GetProcessList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in process)
            {
                SelectListItem item = new SelectListItem();

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["process"] = items;
        }

        public async Task<IActionResult> OnPostAsync()
        {

           var entity = await this.entityService.GetEntityWithProperties(this.EntityId);


            await this.dbFirstService.GenerateQueryAndCommands(this.ProcessId, entity.Name, entity);

            return new EmptyResult();
        }
    }
}
