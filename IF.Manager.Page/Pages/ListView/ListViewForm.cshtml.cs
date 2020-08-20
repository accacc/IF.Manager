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

namespace IF.Manager.Page.Pages.ListView
{
    public class ListViewFormModel : PageModel
    {
        private readonly IPageListViewService pageListViewService;
        private readonly IQueryService queryService;
        private readonly IModelService modelService;
        private readonly IPageFormService pageFormService;
        public ListViewFormModel(IPageListViewService pageListViewService, IQueryService queryService, IModelService projectService, IPageFormService pageFormService)
        {
            this.pageListViewService = pageListViewService;
            this.queryService = queryService;
            this.modelService = projectService;
            this.pageFormService = pageFormService;
        }

        [BindProperty, Required]
        public IFPageListView Form { get; set; }
        public async Task OnGetAddAsync()
        {            
            this.Form = new IFPageListView();
            await this.SetFormDefaults();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.pageListViewService.GetListView(Id);
            await this.SetFormDefaults();

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.pageListViewService.AddListView(this.Form);


            var list = await this.pageListViewService.GetListViewList();

            return new PartialViewResult
            {
                ViewName = "_ListViewListTable",
                ViewData = new ViewDataDictionary<List<IFPageListView>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.pageListViewService.UpdateListView(this.Form);


            var list = await this.pageListViewService.GetListViewList();

            return new PartialViewResult
            {
                ViewName = "_ListViewListTable",
                ViewData = new ViewDataDictionary<List<IFPageListView>>(ViewData, list)
            };
        }


        private async Task SetFormDefaults()
        {
            await SetFormLayouts();
            await SetQueries();
        }



        private async Task SetFormLayouts()
        {
            List<IFPageFormLayout> layouts = await this.pageFormService.GetFormLayouts();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in layouts)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.FormLayoutId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["Form_layouts"] = items;
        }

        private async Task SetQueries()
        {
            var entities = await this.queryService.GetQueryList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in entities)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.IFQueryId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["queries"] = items;
        }

        private async Task SetModels()
        {
            var  models = await this.modelService.GetModelList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in models)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.IFQueryId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["models"] = items;
        }
    }
}
