
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.ClassDesigner.Pages
{
    public class ClassFormModel : PageModel
    {
        private readonly IClassService classService;
       

        public ClassFormModel(IClassService pageService)
        {
            this.classService = pageService;
        }

        [BindProperty(SupportsGet =true), Required]
        public int? ParentId { get; set; }


        [BindProperty(SupportsGet = true), Required]
        public int RootParentId { get; set; }

        [BindProperty, Required]
        public IFClass Form { get; set; }
        public void OnGetAdd()
        {
            this.Form = new IFClass();
            this.Form.ParentId = this.ParentId;
            //await this.SetFromDefaults();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.classService.GetClass(Id);
            //await this.SetFromDefaults();

        }

        private async Task<ClassMapModel> GetTreeModel()
        {
            ClassMapModel model = new ClassMapModel();

            model.IsModal = true;

            var @class = await this.classService.GetClass(this.RootParentId);

            if (@class != null)
            {
                var tree = await this.classService.GetClassTreeList(this.RootParentId);


                model.Tree = tree;
                model.ClassId = @class.Id;
            }
            else
            {
                model.Tree = new List<ClassControlTreeDto>();
            }

            return model;
        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {


            await this.classService.AddClass(this.Form);

            if (this.Form.ParentId.HasValue)
            {
                ClassMapModel model = await GetTreeModel();

                return new PartialViewResult
                {
                    ViewName = "_ClassTreeMain",
                    ViewData = new ViewDataDictionary<ClassMapModel>(ViewData, model)
                };
            }

            else
            {

                var list = await this.classService.GetClassList();

                return new PartialViewResult
                {
                    ViewName = "_ClassListTable",
                    ViewData = new ViewDataDictionary<List<IFClass>>(ViewData, list)
                };

            }
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            try
            {
                await this.classService.UpdateClass(this.Form);
            }
            catch (System.Exception ex)
            {

                throw;
            }


            var list = await this.classService.GetClassList();

            return new PartialViewResult
            {
                ViewName = "_ClassListTable",
                ViewData = new ViewDataDictionary<List<IFClass>>(ViewData, list)
            };
        }


        public async Task<PartialViewResult> OnPostDeleteAsync(int ClassId)
        {

            try
            {
                await this.classService.DeleteClass(ClassId);
            }
            catch (System.Exception ex)
            {

                throw;
            }


            var list = await this.classService.GetClassList();

            return new PartialViewResult
            {
                ViewName = "_ClassListTable",
                ViewData = new ViewDataDictionary<List<IFClass>>(ViewData, list)
            };
        }
    }
}
