using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IF.Manager.Command.Pages
{
    public class CommandFormModel : PageModel
    {
        private readonly ICommandService commandService;
        private readonly IModelService modelService;
        private readonly IProjectService projectService;
        private readonly IClassService classService;

        public CommandFormModel(ICommandService commandService, IModelService modelService, IProjectService projectService, IClassService classService)
        {
            this.commandService = commandService;
            this.modelService = modelService;
            this.projectService = projectService;
            this.classService = classService;
        }

        [BindProperty, Required]
        public IFCommand Form { get; set; }
        public async Task OnGetAddAsync()
        {
            this.Form = new IFCommand();
            await this.SetFromDefaults();
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            this.Form = await this.commandService.GetCommand(Id);
            await this.SetFromDefaults();

        }

        public async Task<PartialViewResult> OnPostAddAsync()
        {

            try
            {
                await this.commandService.AddCommand(this.Form);
            }
            catch (System.Exception ex)
            {

                throw;
            }


            var list = await this.commandService.GetCommandList();

            return new PartialViewResult
            {
                ViewName = "_CommandListTable",
                ViewData = new ViewDataDictionary<List<IFCommand>>(ViewData, list)
            };
        }

        public async Task<PartialViewResult> OnPostUpdateAsync()
        {

            try
            {
                await this.commandService.UpdateCommand(this.Form);
            }
            catch (System.Exception ex)
            {

                throw;
            }


            var list = await this.commandService.GetCommandList();

            return new PartialViewResult
            {
                ViewName = "_CommandListTable",
                ViewData = new ViewDataDictionary<List<IFCommand>>(ViewData, list)
            };
        }


        private async Task SetFromDefaults()
        {
            await SetModels();
            await SetProceses();
            await SetMappers();


        }


        private async Task SetProceses()
        {
            var process = await this.projectService.GetProcessList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in process)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.ProcessId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["process"] = items;
        }

        private async Task SetModels()
        {
            var datas = await this.modelService.GetModelList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in datas)
            {
                SelectListItem item = new SelectListItem();

                if (this.Form.ModelId == data.Id)
                {
                    item.Selected = true;
                }

                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["models"] = items;
        }

        private async Task SetMappers()
        {
            var entities = await this.classService.GetClassMapperList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in entities)
            {
                SelectListItem item = new SelectListItem();
                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["mappers"] = items;
        }



    }
}
