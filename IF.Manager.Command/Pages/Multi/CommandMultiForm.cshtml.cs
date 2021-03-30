using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Command.Pages.Multi
{


    public class CommandMultiModel : PageModel
    {

        private readonly IClassService classService;
        private readonly ICommandService commandService;
        

        [BindProperty, Required]
        public List<IFCommand> Form { get; set; }


        [BindProperty(SupportsGet = true), Required]
        public int CommandId { get; set; }


        public CommandMultiModel(IClassService entityService, ICommandService commandService)
        {
            this.classService = entityService;
            this.commandService = commandService;
        }


        public async Task OnGet(int Id,int? ParentId)
        {

            if (ParentId.HasValue) Id = ParentId.Value;

            this.Form = await this.commandService.GetCommandMultiItems(Id);
            

            if (!this.Form.Any())
            {

                SetEmptyForm(Id);
            }
            else
            {
                this.Form = this.Form.First().Childrens.ToList();
            }

            await SetFromDefaults(Id);

        }



        public async Task<PartialViewResult> OnGetEmptyFormItemPartialAsync(int Id)
        {
            await SetFromDefaults(Id);

            var emptyFormItem = new IFCommand();

            return new PartialViewResult
            {
                ViewName = "_CommandMultiFormItem",
                ViewData = new ViewDataDictionary<IFCommand>(ViewData, emptyFormItem)
            };
        }

        public async Task<PartialViewResult> OnPost()
        {
            await this.commandService.UpdateCommandMulties(this.Form,this.CommandId);


            var CommandList = await this.commandService.GetCommandList();
            

            return new PartialViewResult
            {
                ViewName = "_CommandListTable",
                ViewData = new ViewDataDictionary<List<IFCommand>>(ViewData, CommandList)
            };

        }

        private void SetEmptyForm(int Id)
        {
            this.Form = new List<IFCommand>();
            this.CommandId = Id;
            var item = new IFCommand();
            
            this.Form.Add(item);            

        }

        private async Task SetFromDefaults(int Id)
        {
            this.CommandId = Id;
            await SetMappers();
            await SetCommands();
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

        private async Task SetCommands()
        {
            var entities = await this.commandService.GetCommandList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var data in entities)
            {
                SelectListItem item = new SelectListItem();
                item.Text = data.Name;
                item.Value = data.Id.ToString();
                items.Add(item);
            }

            ViewData["commands"] = items;
        }


    }
}
