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
    public class PageControlUpdateForm : PageModel
    {

        [BindProperty(SupportsGet = true), Required]
        public int? PageControlMapId { get; set; }

        [BindProperty(SupportsGet = true), Required]
        public int RootControlMapId { get; set; }

        [BindProperty(SupportsGet = true), Required]
        public int TreeSelectedId { get; set; }


        private readonly IPageService pageService;
        public PageControlUpdateForm(IPageService pageService)
        {
            this.pageService = pageService;

        }

        public async Task OnGetUpdateControlAsync()
        {
            try
            {
                IFPageControlMap pageControl = await this.pageService.GetPageControlMap(this.TreeSelectedId);
                
                
                this.PageControlMapId = pageControl.ParentId;
                

                await this.SetFromDefaults();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<PartialViewResult> OnPostUpdateControlAsync()
        {
            try
            {
                await this.pageService.UpdatePageContolMap(this.TreeSelectedId, this.PageControlMapId.Value);
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


        public async Task<PartialViewResult> OnPostDeleteControlAsync()
        {
            try
            {
                await this.pageService.DeletePageContolMap(this.TreeSelectedId);
            }
            catch (System.Exception ex)
            {

                throw;
            }


            var tree = await this.pageService.GetPageControlMapTreeList(this.RootControlMapId);
            PageControlMapModel model = new PageControlMapModel();
            model.Tree = tree;
            model.RootControlMapId = this.RootControlMapId;

            return Partial("Control/_PageControlTreeList", model);


            //return new PartialViewResult
            //{
            //    ViewName = ,
            //    ViewData = new ViewDataDictionary<PageControlMapModel>(ViewData, model)
            //};

        }


        private async Task SetFromDefaults()
        {
            List<IFPageControlMap> pageControls = await this.pageService.GetPageControlMapList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in pageControls)
            {
                SelectListItem item = new SelectListItem();

                if (this.PageControlMapId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.IFPageControl.Name + " -" + data.IFPageControl.ControlType;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["controls"] = items;


        }

        
    }
}
