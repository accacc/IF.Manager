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

namespace IF.Manager.ClassDesigner.Pages.Properties
{
    public class FormModel : PageModel
    {

        private readonly IClassService ClassService;
        private readonly IEntityService entityService;

        [BindProperty, Required]
        public List<IFCustomClassProperty> Form { get; set; }

        [BindProperty(SupportsGet =true), Required]
        public int ClassId { get; set; }

        public FormModel(IClassService ClassService, IEntityService entityService)
        {
            this.ClassService = ClassService;
            this.entityService = entityService;
        }


        public async Task OnGet()
        {
            this.Form = await this.ClassService.GetClassPropertyList(this.ClassId);
            
            if (!this.Form.Any())
            {
                SetEmptyForm();
            }

            SetFromDefaults();

        }

      

        public PartialViewResult OnGetEmptyFormItemPartialAsync()
        {
            SetFromDefaults();

            var emptyFormItem = new IFCustomClassProperty();          

            return new PartialViewResult
            {
                ViewName = "_FormItem",
                ViewData = new ViewDataDictionary<IFCustomClassProperty>(ViewData,emptyFormItem )
            };            
        }

        public async Task<PartialViewResult> OnPost()
        {
            await this.ClassService.UpdateClassProperties(this.Form, this.ClassId);
            

            var ClassList = await this.ClassService.GetClassList();

            this.SetFromDefaults();

            return new PartialViewResult
            {
                ViewName = "_ClassListTable",
                ViewData = new ViewDataDictionary<List<IFCustomClassProperty>>(ViewData, ClassList)
            };

        }

        private void SetEmptyForm()
        {

            var emptyFormItem = new IFCustomClassProperty();
            this.Form.Add(emptyFormItem);
        }

        private void SetFromDefaults()
        {
            var primitives = this.entityService.GetPrimitives();


            List<SelectListItem> primitiveList = new List<SelectListItem>();


            foreach (var name in primitives)
            {
                SelectListItem item = new SelectListItem();                

                item.Text = name;
                item.Value = name;
                primitiveList.Add(item);
            }
            
            ViewData["primitives"] = primitiveList;
        }


    }
}
