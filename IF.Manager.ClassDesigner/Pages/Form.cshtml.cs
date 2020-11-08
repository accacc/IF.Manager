using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.ClassDesigner.Pages
{
    public class FormModel : PageModel
    {

        private readonly IClassService classService;

        public FormModel(IClassService classService)
        {
            this.classService = classService;
        }

        [BindProperty, Required]
        public IFCustomClass Form { get; set; }
        public async Task OnGetAddAsync()
        {
            this.Form = new IFCustomClass();
           // await this.SetFromDefaults();
            
        }

        public async Task OnGetUpdateAsync(int Id)
        {            
            this.Form = await this.classService.GetClass(Id);
           // await this.SetFromDefaults();
        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.classService.AddClass(this.Form);


            var list = await this.classService.GetClassList();

            return new PartialViewResult
            {
                ViewName = "_ClassListTable",
                ViewData = new ViewDataDictionary<List<IFCustomClass>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.classService.UpdateClass(this.Form);


            var list = await this.classService.GetClassList();

            return new PartialViewResult
            {
                ViewName = "_ClassListTable",
                ViewData = new ViewDataDictionary<List<IFCustomClass>>(ViewData, list)
            };
        }

    }
}
