using IF.Core.Persistence;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IPageService: IRepository
    {

        //Task<List<PageControlTreeDto>> GetPageControlParent(int parentId);
        //Task<List<PageControlTreeDto>> GetPageControlChilds(int parentId);

        Task<List<IFPageControlMap>> GetPageControlListTree();

        Task<List<PageControlTreeDto>> GetPageControlMapMenuList(int projectId);

        Task<List<IFPageControlMap>> GetPageControlMapList();
        Task<IFPageControlMap> GetPageControlMapByControlId(int PageControlId);

        Task<List<PageControlTreeDto>> GetPageControlMapTreeList(int ParentId);

        Task<List<PageControlTreeDto>> GetPageControlMapMenuTree();

        Task<List<PageControlTreeDto>> GetPageControlMapMenuTree(int? IFProjectId);

        //Task<List<PageControlMapDto>> GetPageNavigationMapTreeList();

        Task AddPage(IFPage form);

        Task<IFPage> GetPage(int id);
        Task UpdatePage(IFPage form);
        Task<List<IFPage>> GetPageList();
        Task<List<IFPageLayout>> GetPageLayoutList();
        //Task GetPageControlMapMenuTreeList(int? iFProjectId);
        Task<List<IFPageControl>> GetPageControlList();
        Task AddPageContolMap(int pageControlId, int? parentPageControlId);
        
        Task<IFPageControlMap> GetPageControlMap(int Id);
        Task<List<IFPageParameter>> GetPageParameters(int id);
        Task<List<IFPageNavigation>> GetPageNavigationList();
        Task UpdatePageContolMap(int pageControlId, int parentPageControlId);
        Task<List<IFPageNavigation>> GetPageNavigationList(int? iFProjectId);
        

        //Task PublishPage(PublishDto form);
        Task DeletePageContolMap(int id);
        Task<IFPageNavigation> GetPageNavigation(int id);
        Task UpdatePageNavigation(IFPageNavigation form);
        Task DeletePageNavigation(int id);

        Task AddPageNavigation(IFPageNavigation form);


        Task<IFPageControlMap> GetPageTree(int ParentId);
        Task UpdatePageParameters(List<IFPageParameter> dtos, int ObjectId, PageParameterType ObjectType);
    }
}
