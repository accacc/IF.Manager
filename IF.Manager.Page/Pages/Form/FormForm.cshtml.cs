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
    public class FormFormModel : PageModel
    {
        private readonly IPageFormService pageFormService;
        private readonly IQueryService queryService;
        private readonly IModelService modelService;
        public FormFormModel(IPageFormService pageFormService, IQueryService queryService, IModelService projectService)
        {
            this.pageFormService = pageFormService;
            this.queryService = queryService;
            this.modelService = projectService;
        }

        [BindProperty, Required]
        public IFPageForm Form { get; set; }
        public async Task OnGetAddAsync()
        {            
            this.Form = new IFPageForm();
            await this.SetFormDefaults();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.pageFormService.GetForm(Id);
            await this.SetFormDefaults();

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.pageFormService.AddForm(this.Form);


            var list = await this.pageFormService.GetFormList();

            return new PartialViewResult
            {
                ViewName = "_FormListTable",
                ViewData = new ViewDataDictionary<List<IFPageForm>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.pageFormService.UpdateForm(this.Form);


            var list = await this.pageFormService.GetFormList();

            return new PartialViewResult
            {
                ViewName = "_FormListTable",
                ViewData = new ViewDataDictionary<List<IFPageForm>>(ViewData, list)
            };
        }


        private async Task SetFormDefaults()
        {
            await SetFormLayouts();
            await SetModels();
            await SetQueries();
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


        private async Task SetModels()
        {
            var  models = await this.modelService.GetModelList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in models)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.IFModelId == data.Id)
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
