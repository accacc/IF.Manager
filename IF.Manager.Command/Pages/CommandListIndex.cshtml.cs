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

namespace IF.Manager.Command.Pages
{
    public class CommandListIndexModel : PageModel
    {
        private readonly ICommandService CommandService;

        public List<IFCommand> CommandList { get; set; }

        public CommandListIndexModel(ICommandService CommandService)
        {
            this.CommandService = CommandService;
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
            this.CommandList = await this.CommandService.GetCommandList();
        }
    }
}
