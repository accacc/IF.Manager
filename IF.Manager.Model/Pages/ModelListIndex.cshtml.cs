using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.Model.Pages
{
    public class ModelListIndexModel : PageModel
    {

        private readonly IModelService modelService;

        public List<ModelDto> ModelList { get; set; }

        public ModelListIndexModel(IModelService modelService)
        {
            this.modelService = modelService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetModelListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_ModelListTable",
                ViewData = new ViewDataDictionary<List<ModelDto>>(ViewData, this.ModelList)
            };

        }

        private async Task SetModel()
        {
            try
            {
                this.ModelList = await this.modelService.GetModelList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}