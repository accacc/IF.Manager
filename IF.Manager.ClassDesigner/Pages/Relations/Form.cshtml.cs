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

namespace IF.Manager.ClassDesigner.Pages.Relations
{
    public class FormModel : PageModel
    {

        private readonly IClassService classService;

        [BindProperty, Required]
        public List<IFCustomClassRelation> Form { get; set; }




        [BindProperty(SupportsGet =true), Required]
        public int ClassId { get; set; }

        public FormModel(IClassService classService)
        {
            this.classService = classService;
        }


        public async Task OnGet()
        {
            this.Form = await this.classService.GetClassRelationList(this.ClassId);
            
            if (!this.Form.Any())
            {
                SetEmptyForm();
            }

            await SetFormDefaults();



        }

      

        public async Task<PartialViewResult> OnGetEmptyFormItemPartialAsync()
        {
            await SetFormDefaults();

            var emptyFormItem = new IFCustomClassRelation();

            return new PartialViewResult
            {
                ViewName = "_FormItem",
                ViewData = new ViewDataDictionary<IFCustomClassRelation>(ViewData,emptyFormItem )
            };
        }

       

       

        public async Task<PartialViewResult> OnPost()
        {            

          await this.classService.UpdateClassRelations(this.Form, this.ClassId);

            var ClassList = await this.classService.GetClassList();

            await SetFormDefaults();

            return new PartialViewResult
            {
                ViewName = "_ClassListTable",
                ViewData = new ViewDataDictionary<List<IFCustomClass>>(ViewData, ClassList)
            };

        }

        private void SetEmptyForm()
        {
            //this.Form = new List<ClassPropertyDto>();
            var emptyFormItem = new IFCustomClassRelation();
            //this.CurrentFormItemIndex++;
            //emptyFormItem.Index = this.CurrentFormItemIndex;
            this.Form.Add(emptyFormItem);
        }



        private async Task SetFormDefaults()
        {
            //List<SelectListItem> items = new List<SelectListItem>();
            //ViewData["properties"] = items;

            var classes = await this.classService.GetClassList();
            var eList = classes.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();
            ViewData["classes"] = eList;
        }


    }
}
