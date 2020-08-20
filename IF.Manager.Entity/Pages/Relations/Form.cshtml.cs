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

namespace IF.Manager.Entity.Pages.Relations
{
    public class FormModel : PageModel
    {

        private readonly IEntityService entityService;

        [BindProperty, Required]
        public List<EntityRelationDto> Form { get; set; }


        //[BindProperty(SupportsGet = true), Required]
        //public int CurrentFormItemIndex { get; set; }


        [BindProperty(SupportsGet =true), Required]
        public int EntityId { get; set; }

        public FormModel(IEntityService entityService)
        {
            this.entityService = entityService;
        }


        public async Task OnGet()
        {
            this.Form = await this.entityService.GetEntityRelationList(this.EntityId);
            
            if (!this.Form.Any())
            {
                SetEmptyForm();
            }

            await SetFromDefaults();

        }

      

        public async Task<PartialViewResult> OnGetEmptyFormItemPartialAsync()
        {
            await SetFromDefaults();

            var emptyFormItem = new EntityRelationDto();
            //this.CurrentFormItemIndex++;
            //emptyFormItem.Index = this.CurrentFormItemIndex;
            

            return new PartialViewResult
            {
                ViewName = "_FormItem",
                ViewData = new ViewDataDictionary<EntityRelationDto>(ViewData,emptyFormItem )
            };

            //return Partial("",this.EntityList);
        }

        public async Task<PartialViewResult> OnPost()
        {            

            await this.entityService.UpdateEntityRelations(this.Form, this.EntityId);

            var entityList = await this.entityService.GetEntityListGrouped();

            await SetFromDefaults();

            return new PartialViewResult
            {
                ViewName = "_EntityListTable",
                ViewData = new ViewDataDictionary<List<List<EntityDto>>>(ViewData, entityList)
            };

        }

        private void SetEmptyForm()
        {
            //this.Form = new List<EntityPropertyDto>();
            var emptyFormItem = new EntityRelationDto();
            //this.CurrentFormItemIndex++;
            //emptyFormItem.Index = this.CurrentFormItemIndex;
            this.Form.Add(emptyFormItem);
        }



        private async Task SetFromDefaults()
        {
            //var primitives = this.entityService.GetPrimitives();
            //var pList = primitives.Select(s => new SelectListItem { Text = s.Name, Value = s.Name }).ToList();
            //ViewData["primitives"] = pList;

            var entities = await this.entityService.GetEntityList();
            var eList = entities.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() }).ToList();
            ViewData["entities"] = eList;
        }


    }
}
