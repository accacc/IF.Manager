using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.ClassDesigner.Pages.Mapper.Mapping
{
    public class ClassMappingFormModel : PageModel
    {

        private readonly IClassService classService;
        private readonly IEntityService entityService;
        private readonly IModelService modelService;


        public ClassMappingFormModel(IClassService classService, IEntityService entityService, IModelService modelService)
        {
            this.classService = classService;
            this.entityService = entityService;
            this.modelService = modelService;
        }

        [BindProperty(SupportsGet = true), Required]
        public int ClassMapId { get; set; }



        [BindProperty, Required]
        public List<IFClassMapping> Form { get; set; }

        public async Task OnGet()
        {
            this.Form = await this.classService.GetClassMappings(this.ClassMapId);

            if (!this.Form.Any())
            {
                SetEmptyForm();
            }

            await SetFromDefaults();

        }

        public async Task<PartialViewResult> OnPost()
        {
            await this.classService.UpdateClassMapping(this.Form, this.ClassMapId);

            var list = await this.classService.GetClassMapperList();

            return new PartialViewResult
            {
                ViewName = "_ClassMapperListTable",
                ViewData = new ViewDataDictionary<List<IFClassMapper>>(ViewData, list)
            };

        }

        private void SetEmptyForm()
        {
            var mapping = new IFClassMapping();
            this.Form = new List<IFClassMapping>();
            this.Form.Add(mapping);

        }

        private async Task SetFromDefaults()
        {


            var mapping = await this.classService.GetClassMapper(this.ClassMapId);


            await SetClasses(mapping.IFClassId.Value);

            var model = await this.modelService.GetModelPropertyList(mapping.IFModelId.Value);

            SetModels(model);

        }

        private void SetModels(List<ModelPropertyDto> models)
        {
            List<SelectListItem> items = new List<SelectListItem>();



            foreach (var property in models)
            {
                SelectListItem item = new SelectListItem();

                item.Text = property.EntityName + " - " + property.Name;
                item.Value = property.ModelPropertyId.ToString();
                items.Add(item);
            }



            ViewData["models"] = items;
        }

        public async Task<PartialViewResult> OnGetEmptyFormItemPartialAsync(int ParentId)
        {
            await SetFromDefaults();

            var emptyFormItem = new IFClassMapping();

            return new PartialViewResult
            {
                ViewName = "_ClassMappingFormItem",
                ViewData = new ViewDataDictionary<IFClassMapping>(ViewData, emptyFormItem)
            };
        }

        private async Task SetClasses(int classId)
        {

            List<IFClass> classes = await this.classService.GetTreeList2(classId);


            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var @class in classes)
            {
                SelectListItem item = new SelectListItem();

                if (@class.Parent == null)
                {
                    item.Text = @class.Name;
                }
                else
                {
                    item.Text = @class.Parent.Name + " - " + @class.Name;
                }

                item.Value = @class.Id.ToString();
                items.Add(item);

            }

            ViewData["classes"] = items;
        }
    }
}
