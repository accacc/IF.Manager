using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.Command.Pages
{
    public class CommandListIndexModel : PageModel
    {
        private readonly ICommandService commandService;

        public List<IFCommand> CommandList { get; set; }

        public CommandListIndexModel(ICommandService CommandService)
        {
            this.commandService = CommandService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetCommandListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_CommandListTable",
                ViewData = new ViewDataDictionary<List<IFCommand>>(ViewData, this.CommandList)
            };

        }

        private async Task SetModel()
        {
            this.CommandList = await this.commandService.GetCommandList();
        }

        public async Task<PartialViewResult> OnGetCommandTreeListAsync(int Id)
        {
            CommandMapModel model = new CommandMapModel();

            model.IsModal = true;

            var command = await this.commandService.GetCommand(Id);

            if (command != null)
            {
                var tree = await this.commandService.GetCommandTreeList(command.Id);


                model.Tree = tree;
                model.CommandId = command.Id;
            }
            else
            {
                model.Tree = new List<CommandControlTreeDto>();
            }


            return new PartialViewResult
            {
                ViewName = "Multi/_CommandTreeMain",
                ViewData = new ViewDataDictionary<CommandMapModel>(ViewData, model)
            };
        }
    }
}
