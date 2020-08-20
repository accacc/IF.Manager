using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IF.Manager.Page.Pages.Navigation
{
    public class _PublishHistoryModel : PageModel
    {
        private readonly IPublishService publishService;

        public _PublishHistoryModel(IPublishService publishService)
        {
            this.publishService = publishService;
        }

        public List<PublishDto> History { get; set; }
        public async Task OnGetAsync()
        {

            this.History = await this.publishService.GetMenuPublishHistory();           

        }
    }
}
