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
    public class PageFormModel : PageModel
    {
        private readonly IPageService pageService;
        private readonly IModelService modelService;
        private readonly IProjectService projectService;

        public PageFormModel(IPageService pageService, IModelService modelService, IProjectService projectService)
        {
            this.pageService = pageService;
            this.modelService = modelService;
            this.projectService = projectService;
        }

        [BindProperty, Required]
        public IFPage Form { get; set; }
        public async Task OnGetAddAsync()
        {
            this.Form = new IFPage();
            await this.SetFromDefaults();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.pageService.GetPage(Id);
            await this.SetFromDefaults();

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {


            await this.pageService.AddPage(this.Form);

            var list = await this.pageService.GetPageList();

            return new PartialViewResult
            {
                ViewName = "_PageListTable",
                ViewData = new ViewDataDictionary<List<IFPage>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            try
            {
                await this.pageService.UpdatePage(this.Form);
            }
            catch (System.Exception ex)
            {

                throw;
            }


            var list = await this.pageService.GetPageList();

            return new PartialViewResult
            {
                ViewName = "_PageListTable",
                ViewData = new ViewDataDictionary<List<IFPage>>(ViewData, list)
            };
        }





        private async Task SetFromDefaults()
        {
            await SetPageLayouts();
            await SetProceses();
            await SetProjects();


        }

        private async Task SetProjects()
        {
            var projects = await this.projectService.GetProjectList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in projects)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.IFProjectId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["projects"] = items;
        }

        private async Task SetProceses()
        {
            var process = await this.projectService.GetProcessList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in process)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.ProcessId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["process"] = items;
        }

        private async Task SetPageLayouts()
        {
            List<IFPageLayout> datas = await this.pageService.GetPageLayoutList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in datas)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.PageLayoutId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["page_layouts"] = items;
        }







    }
}
