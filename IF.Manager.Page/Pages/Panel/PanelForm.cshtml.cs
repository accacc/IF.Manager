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

namespace IF.Manager.Page.Pages.Panel
{
    public class PanelFormModel : PageModel
    {
        private readonly IPageGridService pageGridService;
        private readonly IQueryService queryService;
        private readonly IProjectService projectService;
        public PanelFormModel(IPageGridService pageGridService, IQueryService queryService, IProjectService projectService)
        {
            this.pageGridService = pageGridService;
            this.queryService = queryService;
            this.projectService = projectService;
        }

        [BindProperty, Required]
        public IFPagePanel Form { get; set; }
        public void OnGetAddAsync()
        {            
            this.Form = new IFPagePanel();
            //await this.SetFormDefaults();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.pageGridService.GetPanel(Id);
            //await this.SetFormDefaults();

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.pageGridService.AddPanel(this.Form);


            var list = await this.pageGridService.GetPanelList();

            return new PartialViewResult
            {
                ViewName = "_PanelListTable",
                ViewData = new ViewDataDictionary<List<IFPagePanel>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.pageGridService.UpdatePanel(this.Form);


            var list = await this.pageGridService.GetPanelList();

            return new PartialViewResult
            {
                ViewName = "_PanelListTable",
                ViewData = new ViewDataDictionary<List<IFPagePanel>>(ViewData, list)
            };
        }


        //private async Task SetFormDefaults()
        //{
        //    await SetQueries();
        //    await SetPanelLayouts();
        //}


        //private async Task SetQueries()
        //{
        //    var process = await this.queryService.GetQueryList();

        //    List<SelectListItem> items = new List<SelectListItem>();


        //    foreach (var data in process)
        //    {
        //        SelectListItem item = new SelectListItem();

        //        if (this.Form.QueryId == data.Id)
        //        {
        //            item.Selected = true;
        //        }

        //        item.Text = data.Name;
        //        item.Value = data.Id.ToString();
        //        items.Add(item);
        //    }

        //    ViewData["queries"] = items;
        //}

     
        //private async Task SetPanelLayouts()
        //{
        //    List<IFPageGridLayout> layouts = await this.pageGridService.GetGridLayouts();

        //    List<SelectListItem> items = new List<SelectListItem>();


        //    foreach (var data in layouts)
        //    {
        //        SelectListItem item = new SelectListItem();

        //        if (this.Form.GridLayoutId == data.Id)
        //        {
        //            item.Selected = true;
        //        }

        //        item.Text = data.Name;
        //        item.Value = data.Id.ToString();
        //        items.Add(item);
        //    }

        //    ViewData["grid_layouts"] = items;
        //}
    }
}
