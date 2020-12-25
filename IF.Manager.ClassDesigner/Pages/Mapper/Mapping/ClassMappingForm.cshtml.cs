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


        public ClassMappingFormModel(IClassService pageService)
        {
            this.classService = pageService;
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

        private void SetEmptyForm()
        {
            var mapping = new IFClassMapping();
            this.Form = new List<IFClassMapping>();
            this.Form.Add(mapping);

        }

        private async Task SetFromDefaults()
        {
            

            var mapping = await this.classService.GetClassMapper(this.ClassMapId);

            List<IFClass> fromMaps = await this.classService.GetTreeList2(mapping.FromClassId);

            SetClasses(fromMaps, "fromMaps");

            List<IFClass> toMaps  = await this.classService.GetTreeList2(mapping.ToClassId);

            SetClasses(toMaps, "toMaps");

        }

        private void SetClasses(List<IFClass> classes,string name)
        {
            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var @class in classes)
            {
                    SelectListItem item = new SelectListItem();

                if(@class.Parent==null)
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

            ViewData[name] = items;
        }
    }
}
