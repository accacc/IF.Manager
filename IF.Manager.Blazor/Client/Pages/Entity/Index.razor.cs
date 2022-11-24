using Arbimed.Core.ApiClient;
using IF.Manager.Contracts.Dto;
using Microsoft.AspNetCore.Components;

namespace IF.Manager.Blazor.Client.Pages.Entity
{
    public partial class Index
    {
        [Inject]
        ApiClient apiClient { get; set; }

        public List<EntityDto> EntityList { get; set; } = new();

        protected async override Task OnInitializedAsync()
        {
            //this.EntityList = await apiClient.GetAsync<List<List<EntityDto>>>("api/entity/getall");

            EntityList.AddRange(new List<EntityDto>() { new EntityDto() { GroupId = 1, GroupName = "First Enttiy Group", Name = "Product Entity" } });
            EntityList.AddRange(new List<EntityDto>() { new EntityDto() { GroupId = 2, GroupName = "Second Enttiy Group", Name = "Category Entity" } });

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
