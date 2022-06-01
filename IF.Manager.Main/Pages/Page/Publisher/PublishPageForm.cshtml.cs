using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.Page.Pages
{
    public class PublishPageFormModel : PageModel
    {



        [BindProperty, Required]
        public PublishDto Form { get; set; }

        private readonly IPublishService publishService;
        private readonly IPageService pageService;

        public PublishPageFormModel(IPublishService publishService, IPageService projectService)
        {
            this.publishService = publishService;
            this.pageService = projectService;
        }


        public void OnGetAsync(int PageId)
        {
            this.Form = new PublishDto();
            this.Form.PageId = PageId;

        }

        public async Task<PartialViewResult> OnPostAsync()
        {

            try
            {
                await this.publishService.PublishPageTree(this.Form);
            }
            catch (Exception ex)
            {

                throw;
            }

            var list = await this.pageService.GetPageList();

            return new PartialViewResult
            {
                ViewName = "_PageListTable",
                ViewData = new ViewDataDictionary<List<IFPage>>(ViewData, list)
            };
        }

       

     

        //public async Task<PartialViewResult> OnGetListAsync(int PageId)
        //{
        //    var PublishList = await this.projectService.GetPublishList(PageId);

        //    return new PartialViewResult
        //    {
        //        ViewName = "_PublishHistory",
        //        ViewData = new ViewDataDictionary<List<PublishDto>>(ViewData, PublishList)
        //    };

        //}

    }
}
