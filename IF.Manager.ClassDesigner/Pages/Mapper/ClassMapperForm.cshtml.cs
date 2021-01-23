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
        private readonly IModelService modelService;


        public ClassMapperFormModel(IClassService classService, IModelService modelService)
        {
            this.classService = classService;
            this.modelService = modelService;
        }

        [BindProperty, Required]
        public IFClassMapper Form { get; set; }
        public async Task OnGetAddAsync()
        {
            this.Form = new IFClassMapper();
            await SetForm();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.classService.GetClassMapper(Id);
            await SetForm();

        }

        private async Task SetForm()
        {
            await SetClasses();
            await SetModels();
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



        public async Task<PartialViewResult> OnGetShow()
        {


            QueryFilterMapModel model = await GetTreeModel();

            ShowFilterModel showFilterModel = new ShowFilterModel();


            FilterContext filterContext = new FilterContext();
            filterContext.FilterItems = model.Tree.ToList();
            FilterRuleEngine filterRuleEngine = new FilterRuleEngine(filterContext);
            filterRuleEngine.Execute();

            showFilterModel.Filter = filterRuleEngine.GetFilter();

            return new PartialViewResult
            {
                ViewName = "_ShowFilter",
                ViewData = new ViewDataDictionary<ShowFilterModel>(ViewData, showFilterModel)
            };

        }


        private async Task SetModels()
        {
            var entities = await this.modelService.GetModelList();


            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var entity in entities)
            {
                SelectListItem item = new SelectListItem();

                item.Text = entity.Name;
                item.Value = entity.Id.ToString();
                items.Add(item);
            }

            ViewData["models"] = items;
        }


        private async Task SetClasses()
        {
            var entities = await this.classService.GetClassList();

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var entity in entities)
            {
                SelectListItem item = new SelectListItem();

                item.Text = entity.Name;
                item.Value = entity.Id.ToString();
                items.Add(item);
            }

            ViewData["classes"] = items;
        }

    }
}
