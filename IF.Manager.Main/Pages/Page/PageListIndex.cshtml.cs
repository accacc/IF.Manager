using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Page.Pages
{
    public class PageListIndexModel : PageModel
    {
        private readonly IPageService pageService;

        public List<IFPage> PageList { get; set; }

        public PageListIndexModel(IPageService pageService)
        {
            this.pageService = pageService;
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
                ViewName = "_PageListTable",
                ViewData = new ViewDataDictionary<List<IFPage>>(ViewData, this.PageList)
            };

        }

        public async Task<PartialViewResult> OnGetPageControlTreeListAsync(int Id)
        {
            PageControlMapModel model = new PageControlMapModel();
            model.IsModal = true;

            var pageControl = await this.pageService.GetPageControlMapByControlId(Id);

            if (pageControl != null)
            {
                var tree = await this.pageService.GetPageControlMapTreeList(pageControl.Id);


                model.Tree = tree;
                model.RootControlMapId = pageControl.Id;
            }
            else
            {
                model.Tree = new List<PageControlTreeDto>();
            }
            

            return new PartialViewResult
            {
                ViewName = "_PageTree",
                ViewData = new ViewDataDictionary<PageControlMapModel>(ViewData,model)
            };
        }

        private async Task SetModel()
        {
            this.PageList = await this.pageService.GetPageList();
        }
    }
}
