using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.Page.Pages
{
    public class PageControlInsertFormModel : PageModel
    {

        [BindProperty(SupportsGet = true), Required]
        public int PageControlId { get; set; }

        [BindProperty(SupportsGet = true), Required]
        public int RootControlMapId { get; set; }

        [BindProperty(SupportsGet = true), Required]
        public int? TreeSelectedId { get; set; }


        private readonly IPageService pageService;
        public PageControlInsertFormModel(IPageService pageService)
        {
            this.pageService = pageService;

        }
        
      
        public async Task OnGetAddControlAsync()
        {
            
            await this.SetFromDefaults();
        }

        public async Task<PartialViewResult> OnPostAddControlAsync()
        {
            try
            {
                await this.pageService.AddPageContolMap(this.PageControlId,this.TreeSelectedId);
            }
            catch (System.Exception ex)
            {

                throw;
            }


            var tree = await this.pageService.GetPageControlMapTreeList(this.RootControlMapId);
            PageControlMapModel model = new PageControlMapModel();
            model.Tree = tree;
            model.RootControlMapId = this.RootControlMapId;
            
            return new PartialViewResult
            {
                ViewName = "Control/_PageControlTreeList",
                ViewData = new ViewDataDictionary<PageControlMapModel>(ViewData, model)
            };
        }     


       


        private async Task SetFromDefaults()
        {
            List<IFPageControl> pageControls = await this.pageService.GetPageControlList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in pageControls)
            {
                SelectListItem item = new SelectListItem();

                if (this.PageControlId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name + " -" + data.ControlType;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["controls"] = items;


        }

        
    }
}
