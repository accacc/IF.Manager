using IF.Core.Date;
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

namespace IF.Manager.Page.Pages.Menu
{
    public class MenuFormModel : PageModel
    {

        private readonly IPageService pageService;
        private readonly IProjectService projectService;

        [BindProperty, Required]
        public IFPageNavigation Form { get; set; }

        public MenuFormModel(IPageService pageService, IProjectService projectService)
        {
            this.pageService = pageService;
            this.projectService = projectService;

        }


        public async Task OnGetAddMenuAsync()
        {
            this.Form = new IFPageNavigation();
            await this.SetFormDefaults();
        }     

        public async Task<PartialViewResult> OnPostAddMenuAsync()
        {
            try
            {

                await this.pageService.AddPageNavigation(this.Form);
            }
            catch (System.Exception ex)
            {

                throw;
            }



            var list = await this.pageService.GetPageNavigationList(this.Form.IFProjectId);           

            return Partial("_MenuListTable", list);
        }


        public async Task OnGetUpdateMenuAsync(int Id)
        {
            try
            {
                this.Form = await this.pageService.GetPageNavigation(Id);
                await this.SetFormDefaults();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<PartialViewResult> OnPostUpdateMenuAsync()
        {
            try
            {
                await this.pageService.UpdatePageNavigation(this.Form);
            }
            catch (System.Exception ex)
            {

                throw;
            }


            var list = await this.pageService.GetPageNavigationList(this.Form.IFProjectId);

            return Partial("_MenuListTable", list);
        }


        public async Task<PartialViewResult> OnPostDeleteMenuAsync()
        {
            try
            {
                await this.pageService.DeletePageNavigation(this.Form.Id);
            }
            catch (System.Exception ex)
            {

                throw;
            }


            var list = await this.pageService.GetPageNavigationList(this.Form.IFProjectId);

            return Partial("_MenuListTable", list);
        }


        private async Task SetFormDefaults()
        {
            var entities = await this.projectService.GetProjectList(Contracts.Enum.ProjectType.Web);

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var entity in entities)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.IFProjectId ==  entity.Id)
                {
                    item.Selected = true;
                }

                item.Text = entity.Name;
                item.Value = entity.Id.ToString();
                items.Add(item);
            }

            ViewData["projects"] = items;
        }

    }
}
