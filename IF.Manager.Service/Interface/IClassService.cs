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
        Task<List<IFCustomClass>> GetClassList();
        Task<IFCustomClass> GetClass(int id);

        Task AddClass(IFCustomClass form);
        Task UpdateClass(IFCustomClass dto);

        Task<List<IFCustomClassProperty>> GetClassPropertyList(int classId);

        Task UpdateClassProperties(List<IFCustomClassProperty> dtos, int entityId);
    }
}
