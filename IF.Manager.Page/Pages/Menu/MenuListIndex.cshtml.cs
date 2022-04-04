using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.Page.Pages.Menu
{
    public class MenuListIndexModel : PageModel
    {
        private readonly IPageService pageService;
        private readonly IProjectService projectService;

        public List<IFPageNavigation> MenuList { get; set; }

        public MenuListIndexModel(IPageService actionService, IProjectService projectService)
        {
            this.pageService = actionService;
            this.projectService = projectService;
        }
        public async Task OnGetAsync()
        {           
            await SetModel(null);                
            await SetFromDefaults();
        }

        private async Task SetFromDefaults()
        {
            var entities = await this.projectService.GetProjectList(Contracts.Enum.ProjectType.Web);

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var entity in entities)
            {
                SelectListItem item = new SelectListItem();

                item.Text = entity.Name;
                item.Value = entity.Id.ToString();
                items.Add(item);
            }

            ViewData["projects"] = items;
        }

        public async Task<PartialViewResult> OnGetMenuListPartialAsync(int IFProjectId)
        {
            await SetModel(IFProjectId);

            return Partial("_MenuListTable", this.MenuList);

        }

        private async Task SetModel(int? IFProjectId)
        {
            this.MenuList = await this.pageService.GetPageNavigationList(IFProjectId);
        }
    }
}
