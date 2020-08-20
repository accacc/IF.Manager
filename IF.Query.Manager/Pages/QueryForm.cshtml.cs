using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.Query.Pages
{
    public class QueryFormModel : PageModel
    {
        private readonly IQueryService queryService;
        private readonly IModelService modelService;
        private readonly IProjectService projectService;
        public QueryFormModel(IQueryService queryService, IModelService modelService, IProjectService projectService)
        {
            this.queryService = queryService;
            this.modelService = modelService;
            this.projectService = projectService;
        }

        [BindProperty, Required]
        public QueryDto Form { get; set; }
        public async Task OnGetAddAsync()
        {            
            this.Form = new QueryDto();
            await this.SetFromDefaults();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.queryService.GetQuery(Id);
            await this.SetFromDefaults();

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.queryService.AddQuery(this.Form);


            var list = await this.queryService.GetQueryList();

            return new PartialViewResult
            {
                ViewName = "_QueryListTable",
                ViewData = new ViewDataDictionary<List<QueryDto>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.queryService.UpdateQuery(this.Form);


            var list = await this.queryService.GetQueryList();

            return new PartialViewResult
            {
                ViewName = "_QueryListTable",
                ViewData = new ViewDataDictionary<List<QueryDto>>(ViewData, list)
            };
        }


        private async Task SetFromDefaults()
        {
            await SetModels();
            await SetFormModels();
            await SetProceses();


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

        private async Task SetModels()
        {
            var models = await this.modelService.GetModelList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in models)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.ModelId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["models"] = items;
        }


        private async Task SetFormModels()
        {
            var formModels = await this.queryService.GetFormModelList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in formModels)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.FormModelId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["formmodels"] = items;
        }
    }
}
