using DatabaseSchemaReader.DataSchema;
using IF.Core.Persistence;
using IF.Manager.Contracts.Dto;
using IF.Manager.Service;
using IF.Manager.Service.Model;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IClassService: IRepository
    {


        Task<EntityDto> GetClass(int id);
        //Task UpdateClassGroup(EntityGroupDto form);
        //Task AddClassGroup(EntityGroupDto form);
        //Task<EntityGroupDto> GetClassGroup(int id);
        //Task<List<EntityGroupDto>> GetClassGroupList();
        //Task<List<List<EntityDto>>> GetEntityListGrouped();
        Task<List<IFCustomClass>> GetClassList();

        Task AddClass(EntityDto form);
        Task UpdateClass(EntityDto dto);

        Task<List<EntityPropertyDto>> GetClassPropertyList(int classId);

        Task UpdateClassProperties(List<EntityPropertyDto> dtos, int classId);

        //Task<List<IFCustomClassRelation>> GetClassRelationList(int classId);

        //Task UpdateClassRelations(List<IFCustomClassRelation> relations, int classId);
    }
}
