using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.ClassDesigner.Pages
{
    public class ClassListIndexModel : PageModel
    {
        private readonly IClassService classService;

        public List<IFKClass> ClassList { get; set; }

        public ClassListIndexModel(IClassService pageService)
        {
            this.classService = pageService;
        }
        public async Task OnGetAsync()
        {
           await SetModel();
        }

        public async Task<PartialViewResult> OnGetPageListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_ClassListTable",
                ViewData = new ViewDataDictionary<List<IFKClass>>(ViewData, this.ClassList)
            };

        }

        //public async Task<PartialViewResult> OnGetPageControlTreeListAsync(int Id)
        //{
        //    PageControlMapModel model = new PageControlMapModel();
        //    model.IsModal = true;

        //    var pageControl = await this.classService.GetPageControlMapByControlId(Id);

        //    if (pageControl != null)
        //    {
        //        var tree = await this.classService.GetPageControlMapTreeList(pageControl.Id);


        //        model.Tree = tree;
        //        model.RootControlMapId = pageControl.Id;
        //    }
        //    else
        //    {
        //        model.Tree = new List<PageControlTreeDto>();
        //    }


        //    return new PartialViewResult
        //    {
        //        ViewName = "_PageTree",
        //        ViewData = new ViewDataDictionary<PageControlMapModel>(ViewData,model)
        //    };
        //}

        private async Task SetModel()
        {
            this.ClassList = await this.classService.GetClassList();
        }
    }
}
