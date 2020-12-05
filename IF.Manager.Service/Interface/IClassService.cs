using DatabaseSchemaReader.DataSchema;
using IF.Core.Persistence;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Service;
using IF.Manager.Service.Model;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IClassService: IRepository
    {


        Task<IFClass> GetClass(int id);
        //Task UpdateClassGroup(EntityGroupDto form);
        //Task AddClassGroup(EntityGroupDto form);
        //Task<EntityGroupDto> GetClassGroup(int id);
        //Task<List<EntityGroupDto>> GetClassGroupList();
        //Task<List<List<EntityDto>>> GetEntityListGrouped();
        Task<List<IFClass>> GetClassList();

        Task AddClass(IFClass form);
        Task<List<IFClassMapper>> GetClassMapperList();
        Task UpdateClass(IFClass dto);


        Task UpdateClassProperties(List<ClassControlTreeDto> dtos, int classId);

        Task<List<ClassControlTreeDto>> GetClassTreeList(int ParentId);
        Task<IFClassMapper> GetClassMapper(int ıd);
        Task GenerateClass(int classId);
        Task<List<ClassControlTreeDto>> GetClassPropertyList(int classId    );
        Task AddClassMapper(IFClassMapper form);
        Task UpdateClassMapper(IFClassMapper form);

        //Task<List<IFCustomClassRelation>> GetClassRelationList(int classId);

        //Task UpdateClassRelations(List<IFCustomClassRelation> relations, int classId);
    }
}
