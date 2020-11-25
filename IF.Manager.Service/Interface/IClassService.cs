﻿using DatabaseSchemaReader.DataSchema;
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


        Task<List<EntityGroupDto>> GetEntityGroupList();
        Task<List<List<EntityDto>>> GetEntityListGrouped();
        Task<List<IFCustomClass>> GetClassList();
        Task<IFCustomClass> GetClass(int id);

        Task AddClass(IFCustomClass form);
        Task UpdateClass(IFCustomClass dto);

        Task<List<IFCustomClassProperty>> GetClassPropertyList(int classId);

        Task UpdateClassProperties(List<IFCustomClassProperty> dtos, int classId);

        Task<List<IFCustomClassRelation>> GetClassRelationList(int classId);

        Task UpdateClassRelations(List<IFCustomClassRelation> relations, int classId);
    }
}
