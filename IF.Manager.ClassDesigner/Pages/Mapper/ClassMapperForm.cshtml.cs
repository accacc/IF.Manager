using IF.Manager.Contracts.Services;
using IF.Manager.Service.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.ClassDesigner.Pages.Mapper
{
    public class ClassMapperFormModel : PageModel
    {
        private readonly IClassService classService;
       

        public ClassMapperFormModel(IClassService classService)
        {
            this.classService = classService;
        }

        [BindProperty, Required]
        public IFClassMapper Form { get; set; }
        public void OnGetAddAsync()
        {
            this.Form = new IFClassMapper();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.classService.GetClassMapper(Id);
            await SetClasses();

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.classService.AddClassMapper(this.Form);


            var list = await this.classService.GetClassMapperList();

            return new PartialViewResult
            {
                ViewName = "_ClassMapperListTable",
                ViewData = new ViewDataDictionary<List<IFClassMapper>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.classService.UpdateClassMapper(this.Form);


            var list = await this.classService.GetClassMapperList();

            return new PartialViewResult
            {
                ViewName = "_ClassMapperListTable",
                ViewData = new ViewDataDictionary<List<IFClassMapper>>(ViewData, list)
            };
        }


        public async Task<PartialViewResult> OnGetDropDownFromClassPartialAsync()
        {
            await SetClasses();

            return new PartialViewResult
            {
                ViewName = "_DropDownFromClass",
                ViewData = new ViewDataDictionary<IFClassMapper>(ViewData, this.Form)
            };

        }

        public async Task<PartialViewResult> OnGetDropDownToClassPartialAsync()
        {
            await SetClasses();

            return new PartialViewResult
            {
                ViewName = "_DropDownToClass",
                ViewData = new ViewDataDictionary<IFClassMapper>(ViewData, this.Form)
            };

        }

        private async Task SetClasses()
        {
            var classList = await this.classService.GetClassList();


            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in classList)
            {
                SelectListItem item = new SelectListItem();

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["classes"] = items;
        }

    }
}
