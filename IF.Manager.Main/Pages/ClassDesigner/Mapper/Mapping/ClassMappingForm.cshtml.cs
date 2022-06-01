using IF.Core.Algorithms;
using IF.Manager.Contracts.Dto;
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

            await SetFormDefaults(false);

        }

        public async Task OnGetAutoMapping()
        {
            this.Form = new List<IFClassMapping>();
            await SetFormDefaults(true);

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

        private async Task SetFormDefaults(bool IsAutoFill = false)
        {
            var mapping = await this.classService.GetClassMapper(this.ClassMapId);

            List<IFClass> classes = await this.classService.GetClassFlattenList(mapping.IFClassId.Value);

            var models = await this.modelService.GetModelPropertyList(mapping.IFModelId.Value);

            if (IsAutoFill)
            {
                AutoMapping(classes, models);

            }

            SetModels(models);
            SetClasses(classes);

        }

        private void AutoMapping(List<IFClass> classes, List<ModelPropertyDto> models)
        {
            foreach (var model in models)
            {
                List<LevenshteinDistanceResult> levenshteinDistanceResultList = new List<LevenshteinDistanceResult>();

                foreach (var @class in classes)
                {
                    if (!@class.IsPrimitive) continue;

                    LevenshteinDistanceResult levenshteinDistanceResult = new LevenshteinDistanceResult();

                    int score = LevenshteinDistance.Calculate(model.Name, @class.Name);


                    levenshteinDistanceResult.score = score;
                    levenshteinDistanceResult.string2 = model.Name;
                    levenshteinDistanceResult.string1 = @class.Name;
                    levenshteinDistanceResult.id1 = @class.Id;
                    levenshteinDistanceResult.id2 = model.ModelPropertyId;

                    levenshteinDistanceResultList.Add(levenshteinDistanceResult);
                }

                var min = levenshteinDistanceResultList.Min(l => l.score);

                if (min <= 3)
                {

                    var bestMatch = levenshteinDistanceResultList.Where(r => r.score == min).FirstOrDefault();

                    this.Form.Add(new IFClassMapping { FromPropertyId = bestMatch.id1, ToPropertyId = bestMatch.id2 });
                }
            }
        }

        public async Task<PartialViewResult> OnGetEmptyFormItemPartialAsync()
        {
            await SetFormDefaults();

            var emptyFormItem = new IFClassMapping();

            return new PartialViewResult
            {
                ViewName = "_ClassMappingFormItem",
                ViewData = new ViewDataDictionary<IFClassMapping>(ViewData, emptyFormItem)
            };
        }

        private void SetClasses(List<IFClass> classes)
        {


            List<DropDownClass> items = new List<DropDownClass>();


            foreach (var @class in classes)
            {
                DropDownClass item = new DropDownClass();

                item.Text = @class.GetRootPathWithoutRoot() + "." + @class.Name;
                item.Value = @class.Id.ToString();

                if (this.Form.Any(m => m.FromPropertyId == @class.Id))
                {
                    item.Choosen = true;
                }

                items.Add(item);

            }

            ViewData["classes"] = items;
        }


        private void SetModels(List<ModelPropertyDto> models)
        {
            List<DropDownClass> items = new List<DropDownClass>();

            foreach (var model in models)
            {
                DropDownClass item = new DropDownClass();

                item.Text = model.EntityName + " - " + model.Name;
                item.Value = model.ModelPropertyId.ToString();

                if (this.Form.Any(m => m.ToPropertyId == model.ModelPropertyId))
                {
                    item.Choosen = true;
                }

                items.Add(item);
            }

            ViewData["models"] = items;
        }
    }

    public class DropDownClass
    {

        public string Text { get; set; }
        public string Value { get; set; }

        public bool Selected { get; set; }

        public bool Choosen { get; set; }
    }

    public class LevenshteinDistanceResult
    {
        public string string1 { get; set; }
        public string string2 { get; set; }

        public int id1 { get; set; }

        public int id2 { get; set; }

        public int score { get; set; }
    }
}
