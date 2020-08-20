using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Model.Pages.Property
{
    public class ModelTreeModel
    {
        public List<EntityTreeDto> EntityTree { get; set; }

        public List<string> Selecteds { get; set; }
    }
    public class ModelListModel : PageModel
    {
        private readonly IEntityService entityService;
        private readonly IModelService modelService;

        [BindProperty]
        public List<int> ids { get; set; } = new List<int>();

        [BindProperty(SupportsGet = true), Required]
        public int ModelId { get; set; }

        public ModelTreeModel TreeList { get; set; }


        [BindProperty, Required]
        public ModelUpdateDto CreateModel { get; set; }

        public ModelListModel(IEntityService entityService, IModelService modelService)
        {
            this.entityService = entityService;
            this.modelService = modelService;
            this.TreeList = new ModelTreeModel();
        }

        public async Task OnGetAsync()
        {

            try
            {

                var model = await this.modelService.GetModel(this.ModelId);
                ModelTreeModel pageModel = new ModelTreeModel();
                pageModel.EntityTree = new List<EntityTreeDto>();
                pageModel.EntityTree.Add(await this.entityService.GetEntityTree(model.EntityId));
                pageModel.Selecteds = new List<string>();

                var properties = await this.modelService.GetModelPropertyList(model.Id);

                pageModel.Selecteds.AddRange(properties.Select(p => $"{p.EntityPropertyId}-{p.EntityId}").ToArray());

                this.TreeList = pageModel;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<PartialViewResult> OnPostCreateModelAsync()
        {
            try
            {
                await this.modelService.SaveModel(this.CreateModel, this.ModelId);

                var ModelList = await this.modelService.GetModelList();

                return new PartialViewResult
                {
                    ViewName = "_ModelListTable",
                    ViewData = new ViewDataDictionary<List<ModelDto>>(ViewData, ModelList)
                };
            }
            catch (Exception ex)
            {

                throw;
            }


        }
    }
}
