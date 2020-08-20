using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.Page.Pages
{
    public class GridFormModel : PageModel
    {
        private readonly IPageGridService pageGridService;
        private readonly IQueryService queryService;
        private readonly IProjectService projectService;
        public GridFormModel(IPageGridService pageGridService, IQueryService queryService, IProjectService projectService)
        {
            this.pageGridService = pageGridService;
            this.queryService = queryService;
            this.projectService = projectService;
        }

        [BindProperty, Required]
        public IFPageGrid Form { get; set; }
        public async Task OnGetAddAsync()
        {            
            this.Form = new IFPageGrid();
            await this.SetFormDefaults();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.pageGridService.GetGrid(Id);
            await this.SetFormDefaults();

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.pageGridService.AddGrid(this.Form);


            var list = await this.pageGridService.GetGridList();

            return new PartialViewResult
            {
                ViewName = "_GridListTable",
                ViewData = new ViewDataDictionary<List<IFPageGrid>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.pageGridService.UpdateGrid(this.Form);


            var list = await this.pageGridService.GetGridList();

            return new PartialViewResult
            {
                ViewName = "_GridListTable",
                ViewData = new ViewDataDictionary<List<IFPageGrid>>(ViewData, list)
            };
        }


        private async Task SetFormDefaults()
        {
            await SetQueries();
            await SetGridLayouts();
        }


        private async Task SetQueries()
        {
            var process = await this.queryService.GetQueryList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in process)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.QueryId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["queries"] = items;
        }

     
        private async Task SetGridLayouts()
        {
            List<IFPageGridLayout> layouts = await this.pageGridService.GetGridLayouts();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in layouts)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.GridLayoutId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["grid_layouts"] = items;
        }
    }
}
