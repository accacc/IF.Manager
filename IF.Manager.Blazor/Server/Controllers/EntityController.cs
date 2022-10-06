using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IF.Manager.Contracts.Services;
using IF.Manager.Contracts.Dto;

namespace IF.Manager.Blazor.Server.Controllers
{
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly IEntityService entityService;

        public EntityController(IEntityService entityService)
        {
            this.entityService = entityService;
        }

        [HttpGet]
        [Route("api/entity/getall")]
        public async Task<List<List<EntityDto>>> GetEntityListGrouped()
        {
            try
            {
                return await entityService.GetEntityListGrouped();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
