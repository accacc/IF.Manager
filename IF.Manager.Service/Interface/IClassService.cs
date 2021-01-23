using IF.Core.Persistence;
using IF.Manager.Contracts.Dto;
using IF.Manager.Service.Model;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IClassService: IRepository
    {

        Task JsonToClass(string name, string json);
        Task<IFClass> GetClass(int id);
        Task<List<IFClass>> GetClassList();

        Task AddClass(IFClass form);
        Task<List<IFClassMapper>> GetClassMapperList();
        Task UpdateClass(IFClass dto);


        Task UpdateClassProperties(List<ClassControlTreeDto> dtos, int classId);

        Task<List<ClassControlTreeDto>> GetClassTreeList(int ParentId);
        Task<List<IFClassMapping>> GetClassMappings(int classMapId);
        Task<IFClassMapper> GetClassMapper(int ıd);
        Task GenerateClass(int classId);
        Task<List<ClassControlTreeDto>> GetClassPropertyList(int classId    );
        Task AddClassMapper(IFClassMapper form);
        Task UpdateClassMapper(IFClassMapper form);
        Task<List<IFClass>> GetClassTree(int classId);

        Task<List<IFClass>> GetTreeList2(int classId);
        Task DeleteClass(int ıd);
        Task UpdateClassMapping(List<IFClassMapping> form,int classMapId);
        Task GenerateMapper(int classMapId);
    }
}
