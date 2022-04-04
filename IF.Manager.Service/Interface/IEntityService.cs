using IF.Core.Persistence;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IEntityService: IRepository
    {

        Task<List<EntityDto>> GetEntityAllRelations(int id);
        Task<List<EntityDto>> GetEntityList();

        Task<List<IFEntity>> GetShadowAuditEntityList();

        Task<List<List<EntityDto>>> GetEntityListGrouped();
        //Task<List<EntityDto>> GetEntityListWithProperties();
        Task AddEntity(EntityDto form);
        Task UpdateEntity(EntityDto dto);
        Task<List<EntityPropertyDto>> GetEntityPropertyList(int entityId);

        Task UpdateEntityProperties(List<EntityPropertyDto> dtos, int entityId);
        Task<ModelClassTreeDto> GetEntityTree(int entityId);
        string[] GetPrimitives();

        List<Type> GetAllEntityTypes();
        Task UpdateEntityRelations(List<EntityRelationDto> form, int entityId);
        
        Task<List<EntityRelationDto>> GetEntityRelationList(int entityId);
        Task AddEntityGroup(EntityGroupDto form);
        Task<EntityDto> GetEntity(int id);
        Task<IFEntity> GetEntityWithProperties(int id);
        Task<EntityGroupDto> GetEntityGroup(int id);
        Task UpdateEntityGroup(EntityGroupDto form);
        Task<List<EntityGroupDto>> GetEntityGroupList();

        Task<bool> EntityIsExistByName(string name);



    }
}
