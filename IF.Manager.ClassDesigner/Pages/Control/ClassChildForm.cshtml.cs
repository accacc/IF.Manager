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

namespace IF.Manager.ClassDesigner.Pages.Control
{
    public class ClassChildFormModel : PageModel
    {

        private readonly IClassService classService;
        private readonly IEntityService entityService;

        [BindProperty, Required]
        public List<ClassControlTreeDto> Form { get; set; }

        //[BindProperty(SupportsGet =true), Required]
        //public int ClassId { get; set; }

        [BindProperty(SupportsGet = true), Required]
        public int? ParentId { get; set; }


        //[BindProperty(SupportsGet = true), Required]
        //public int? TreeSelectedId { get; set; }

        public ClassChildFormModel(IClassService classService, IEntityService entityService)
        {
            this.classService = classService;
            this.entityService = entityService;
        }


        public async Task OnGet()
        {
            this.Form = await this.classService.GetClassPropertyList(this.ParentId.Value);
            
            if (!this.Form.Any())
            {
                SetEmptyForm();
            }

            SetFromDefaults();

        }

      

        public PartialViewResult OnGetEmptyFormItemPartialAsync()
        {
            SetFromDefaults();

            var emptyFormItem = new ClassControlTreeDto();          

            return new PartialViewResult
            {
                ViewName = "_ClassChildFormItem",
                ViewData = new ViewDataDictionary<ClassControlTreeDto>(ViewData,emptyFormItem )
            };            
        }

       

            public async Task<PartialViewResult> OnPostAddPrimitivePropertyAsync()
        {
            await this.classService.UpdateClassProperties(this.Form, this.ParentId.Value);

            ClassMapModel model = await GetTreeModel();

            return new PartialViewResult
            {
                ViewName = "_ClassTreeMain",
                ViewData = new ViewDataDictionary<ClassMapModel>(ViewData, model)
            };

        }


        public async Task<PartialViewResult> OnPostAddClassPropertyAsync()
        {
            await this.classService.UpdateClassProperties(this.Form, this.ParentId.Value);

            ClassMapModel model = await GetTreeModel();

            return new PartialViewResult
            {
                ViewName = "_ClassTreeMain",
                ViewData = new ViewDataDictionary<ClassMapModel>(ViewData, model)
            };

        }
        private async Task<ClassMapModel> GetTreeModel()
        {
            ClassMapModel model = new ClassMapModel();

            model.IsModal = true;

            var @class = await this.classService.GetClass(this.ParentId.Value);

            if (@class != null)
            {
                var tree = await this.classService.GetClassTreeList(@class.Id);


                model.Tree = tree;
                model.ClassId = @class.Id;
            }
            else
            {
                model.Tree = new List<ClassControlTreeDto>();
            }

            return model;
        }

        private void SetEmptyForm()
        {

            var emptyFormItem = new ClassControlTreeDto();
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
