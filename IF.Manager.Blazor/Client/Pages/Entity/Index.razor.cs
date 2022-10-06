using Arbimed.Core.ApiClient;
using IF.Manager.Contracts.Dto;
using Microsoft.AspNetCore.Components;

namespace IF.Manager.Blazor.Client.Pages.Entity
{
    public partial class Index
    {
        [Inject]
        ApiClient apiClient { get; set; }

        public List<List<EntityDto>> EntityList { get; set; }

        protected async override Task OnInitializedAsync()
        {
            this.EntityList = await apiClient.GetAsync<List<List<EntityDto>>>("api/entity/getall");
          
        }

        //public EntityManagerIndexModel(IEntityService entityService)
        //{
        //    this.entityService = entityService;
        //}
        //public async Task OnGetAsync()
        //{
        //    await SetModel();
        //}

        //public async Task<PartialViewResult> OnGetEntityListPartialAsync()
        //{
        //    await SetModel();

        //    return new PartialViewResult
        //    {
        //        ViewName = "_EntityListTable",
        //        ViewData = new ViewDataDictionary<List<List<EntityDto>>>(ViewData, this.EntityList)
        //    };

        //    //return Partial("",this.EntityList);
        //}

        //private async Task SetModel()
        //{
        //    var list = await this.entityService.GetEntityListGrouped();

        //    this.EntityList = list;
        //}

    }
}
