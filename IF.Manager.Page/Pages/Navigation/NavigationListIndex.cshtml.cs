using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Page.Pages.Navigation
{
    public class NavigationListIndexModel : PageModel
    {
        public PageControlMapModel NavigationTree;

        private readonly IProjectService projectService;
        private readonly IPageService pageService;
        public NavigationListIndexModel(IPageService pageService, IProjectService projectService)
        {
            this.pageService = pageService;
            this.projectService = projectService;
        }


        public async Task OnGet()
        {
            await SetModel(null);
            await this.SetFormDefaults();

        }

        private async Task SetModel(int? IFProjectId)
        {
            var tree = await this.pageService.GetPageControlMapMenuTree(IFProjectId);
            this.NavigationTree = new PageControlMapModel();
            this.NavigationTree.Tree = tree;
        }

        public async Task<PartialViewResult> OnGetNavigationListPartialAsync(int IFProjectId)
        {
            await SetModel(IFProjectId);

            return Partial("../Control/_PageControlTreeList", this.NavigationTree);
        }


        private async Task SetFormDefaults()
        {
            var entities = await this.projectService.GetProjectList();

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
    }
}
