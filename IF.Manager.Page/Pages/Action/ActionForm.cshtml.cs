using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.Page.Pages
{
    public class ActionFormModel : PageModel
    {
        private readonly IPageActionService pageActionService;
        private readonly IQueryService queryService;
        private readonly ICommandService commandService;
        private readonly IModelService modelService;
        private readonly IPageService pageService;
        public ActionFormModel(IPageActionService pageActionService, IQueryService queryService, ICommandService commandService, IModelService modelService, IPageService pageService)
        {
            this.pageActionService = pageActionService;
            this.queryService = queryService;
            this.commandService = commandService;
            this.modelService = modelService;
            this.pageService = pageService;
        }

        [BindProperty, Required]
        public IFPageAction Form { get; set; }
        public async Task OnGetAddAsync()
        {            
            this.Form = new IFPageAction();
            await this.SetFormDefaults();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.pageActionService.GetAction(Id);
            await this.SetFormDefaults();

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            await this.pageActionService.AddAction(this.Form);


            var list = await this.pageActionService.GetActionList();

            return new PartialViewResult
            {
                ViewName = "_ActionListTable",
                ViewData = new ViewDataDictionary<List<IFPageAction>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            await this.pageActionService.UpdateAction(this.Form);


            var list = await this.pageActionService.GetActionList();

            return new PartialViewResult
            {
                ViewName = "_ActionListTable",
                ViewData = new ViewDataDictionary<List<IFPageAction>>(ViewData, list)
            };
        }


        private async Task SetFormDefaults()
        {
            await this.SetQueries();
            await this.SetCommands();
            await SetModels();
            await SetControls();
        }

        private async Task SetControls()
        {
            var datas = await this.pageService.GetPageControlList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in datas)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.IFPageControlId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["page_controls"] = items;
        }

        private async Task SetModels()
        {
            var datas = await this.modelService.GetModelList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in datas)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.IFModelId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["models"] = items;
        }
        private async Task SetQueries()
        {
            var entities = await this.queryService.GetQueryList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in entities)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.QueryId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["queries"] = items;
        }

        private async Task SetCommands()
        {
            var entities = await this.commandService.GetCommandList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in entities)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.CommandId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["commands"] = items;
        }




    }
}
