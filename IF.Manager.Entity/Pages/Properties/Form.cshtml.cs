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

namespace IF.Manager.Entity.Pages.Properties
{
    public class FormModel : PageModel
    {

        private readonly IEntityService entityService;

        [BindProperty, Required]
        public List<EntityPropertyDto> Form { get; set; }

        [BindProperty(SupportsGet =true), Required]
        public int EntityId { get; set; }

        public FormModel(IEntityService entityService)
        {
            this.entityService = entityService;
        }


        public async Task OnGet()
        {
            this.Form = await this.entityService.GetEntityPropertyList(this.EntityId);
            
            if (!this.Form.Any())
            {
                SetEmptyForm();
            }

            SetFromDefaults();

        }

      

        public PartialViewResult OnGetEmptyFormItemPartialAsync()
        {
            SetFromDefaults();

            var emptyFormItem = new EntityPropertyDto();          

            return new PartialViewResult
            {
                ViewName = "_FormItem",
                ViewData = new ViewDataDictionary<EntityPropertyDto>(ViewData,emptyFormItem )
            };            
        }

        public async Task<PartialViewResult> OnPost()
        {
            await this.entityService.UpdateEntityProperties(this.Form, this.EntityId);
            

            var entityList = await this.entityService.GetEntityListGrouped();

            this.SetFromDefaults();

            return new PartialViewResult
            {
                ViewName = "_EntityListTable",
                ViewData = new ViewDataDictionary<List<List<EntityDto>>>(ViewData, entityList)
            };

        }

        private void SetEmptyForm()
        {

            var emptyFormItem = new EntityPropertyDto();
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
