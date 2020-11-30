using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [BindProperty, Required]
        public IFKClass Form { get; set; }
        public void OnGetAdd()
        {
            this.Form = new IFKClass();
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

            var @class = await this.classService.GetClass(this.Form.Id);

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
                    ViewData = new ViewDataDictionary<List<IFKClass>>(ViewData, list)
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
                ViewData = new ViewDataDictionary<List<IFKClass>>(ViewData, list)
            };
        }





        //private async Task SetFromDefaults()
        //{
        //    await SetPageLayouts();
        //    await SetProceses();
        //    await SetProjects();


        //}

        //private async Task SetProjects()
        //{
        //    var projects = await this.projectService.GetProjectList();

        //    List<SelectListItem> items = new List<SelectListItem>();


        //    foreach (var data in projects)
        //    {
        //        SelectListItem item = new SelectListItem();

        //        if (this.Form.IFProjectId == data.Id)
        //        {
        //            item.Selected = true;
        //        }

        //        item.Text = data.Name;
        //        item.Value = data.Id.ToString();
        //        items.Add(item);
        //    }

        //    ViewData["projects"] = items;
        //}

        //private async Task SetProceses()
        //{
        //    var process = await this.projectService.GetProcessList();

        //    List<SelectListItem> items = new List<SelectListItem>();


        //    foreach (var data in process)
        //    {
        //        SelectListItem item = new SelectListItem();

        //        if (this.Form.ProcessId == data.Id)
        //        {
        //            item.Selected = true;
        //        }

        //        item.Text = data.Name;
        //        item.Value = data.Id.ToString();
        //        items.Add(item);
        //    }

        //    ViewData["process"] = items;
        //}

        //private async Task SetPageLayouts()
        //{
        //    List<IFPageLayout> datas = await this.classService.GetPageLayoutList();

        //    List<SelectListItem> items = new List<SelectListItem>();


        //    foreach (var data in datas)
        //    {
        //        SelectListItem item = new SelectListItem();

        //        if (this.Form.PageLayoutId == data.Id)
        //        {
        //            item.Selected = true;
        //        }

        //        item.Text = data.Name;
        //        item.Value = data.Id.ToString();
        //        items.Add(item);
        //    }

        //    ViewData["page_layouts"] = items;
        //}







    }
}
