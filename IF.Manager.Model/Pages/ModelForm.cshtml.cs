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

namespace IF.Manager.Model.Pages
{
    public class ModelFormModel : PageModel
    {
        private readonly IModelService modelService;
        private readonly IEntityService entityService;

        public ModelFormModel(IModelService modelService,IEntityService entityService)
        {
            this.modelService = modelService;
            this.entityService = entityService;
        }

        [BindProperty, Required]
        public ModelDto Form { get; set; }
        public async Task OnGetAddAsync()
        {

            this.Form = new ModelDto();
            await this.SetFromDefaults();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.modelService.GetModel(Id);
            await this.SetFromDefaults();
        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.modelService.AddModel(this.Form);


            var list = await this.modelService.GetModelList();

            return new PartialViewResult
            {
                ViewName = "_ModelListTable",
                ViewData = new ViewDataDictionary<List<ModelDto>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.modelService.UpdateModel(this.Form);


            var list = await this.modelService.GetModelList();

            return new PartialViewResult
            {
                ViewName = "_ModelListTable",
                ViewData = new ViewDataDictionary<List<ModelDto>>(ViewData, list)
            };
        }

        private async Task SetFromDefaults()
        {
            var entities = await this.entityService.GetEntityList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var entity in entities)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.EntityId == entity.Id)
                {
                    item.Selected = true;
                }

                item.Text = entity.Name;
                item.Value = entity.Id.ToString();
                items.Add(item);
            }

            ViewData["entities"] = items;
        }
    }
}
