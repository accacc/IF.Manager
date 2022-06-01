using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Page.Pages
{


    public class PageParameterFormModel : PageModel
    {

        private readonly IPageService pageService;
        private readonly IEntityService entityService;
        private readonly IModelService modelService;

        [BindProperty, Required]
        public List<IFPageParameter> Form { get; set; }


        [BindProperty(SupportsGet = true), Required]
        public int ObjectId { get; set; }

        [BindProperty(SupportsGet = true), Required]
        public PageParameterType ObjectType { get; set; }


        public PageParameterFormModel(IPageService pageService, IEntityService entityService, IModelService modelService)
        {
            this.pageService = pageService;
            this.entityService = entityService;
            this.modelService = modelService;
        }


        public async Task OnGet()
        {

            this.Form = await this.pageService.GetPageParameters(this.ObjectId);           
            

            if (!this.Form.Any())
            {
                SetEmptyForm();
            }

            SetFromDefaults();

        }



        public PartialViewResult OnGetEmptyFormItemPartialAsync()
        {
            SetFromDefaults();

            var parameter = new IFPageParameter();
            var primitives = this.entityService.GetPrimitives();
            parameter.Type = primitives.First();

            return new PartialViewResult
            {
                ViewName = "_PageParameterFormItem",
                ViewData = new ViewDataDictionary<IFPageParameter>(ViewData, parameter)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {
            await this.pageService.UpdatePageParameters(this.Form, this.ObjectId,this.ObjectType);


            var list = await this.pageService.GetPageList();

            return new PartialViewResult
            {
                ViewName = "_PageListTable",
                ViewData = new ViewDataDictionary<List<IFPage>>(ViewData, list)
            };

        }

        private void SetEmptyForm()
        {
            var primitives = this.entityService.GetPrimitives();
            var parameter = new IFPageParameter();
            parameter.Type = primitives.First();            
            this.Form = new List<IFPageParameter>();
            this.Form.Add(parameter);

        }

        private void SetFromDefaults()
        {
            SetPrimitives();

        }

        private void SetPrimitives()
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
