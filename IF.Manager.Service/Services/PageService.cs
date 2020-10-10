using IF.CodeGeneration.Core;
using IF.Core.Control;
using IF.Core.Data;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.Web.Page;
using IF.Persistence.EF;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Service.Services
{
    public class PageService : GenericRepository, IPageService
    {


        private readonly FileSystemCodeFormatProvider fileSystem;
        private readonly VsManager vs;
        private readonly VsHelper vsHelper;
        private readonly IPageGridService pageGridService;

        public PageService(ManagerDbContext dbContext, IPageGridService pageGridService) : base(dbContext)
        {
            this.fileSystem = new FileSystemCodeFormatProvider(DirectoryHelper.GetTempGeneratedDirectoryName());
            this.vs = new VsManager();
            this.vsHelper = new VsHelper(DirectoryHelper.GetTempGeneratedDirectoryName());
            this.pageGridService = pageGridService;
        }


        public void MovePageControlModelItemUp(int Id,int objectId)
        {
            IFPageControlItemModelProperty entity = this.GetPageFormItemModelProperty(Id);

            if (entity != null)
            {
                this.MoveUpOne<IFPageControlItemModelProperty>(entity.Sequence,e=>e.ObjectId== objectId);
            }
        }

        public void MovePageControlModelItemDown(int Id, int objectId)
        {
            IFPageControlItemModelProperty entity = this.GetPageFormItemModelProperty(Id);

            if (entity != null)
            {
                this.MoveDownOne<IFPageControlItemModelProperty>(entity.Sequence, e => e.ObjectId == objectId);
            }
        }

        private IFPageControlItemModelProperty GetPageFormItemModelProperty(int Id)
        {

            return this.GetByKey<IFPageControlItemModelProperty>(Id);

        }


        public async Task UpdatePageControlItemModelProperty(IFPageControlItemModelProperty form)
        {

            try
            {
                var entity = await this.GetQuery<IFPageControlItemModelProperty>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException($"{nameof(IFPageControlItemModelProperty)} : No such entity exists"); }


                entity.IFQueryId = form.IFQueryId; ;
                entity.NameIFModelPropertyId = form.NameIFModelPropertyId;

                entity.ValueIFModelPropertyId = form.ValueIFModelPropertyId;
                this.Update(entity);
                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IFPageControlItemModelProperty> GetPageControlItemModelProperty(int iFPageFormItemModelPropertyId)
        {
            var data = await this.GetQuery<IFPageControlItemModelProperty>(c => c.Id == iFPageFormItemModelPropertyId).SingleOrDefaultAsync();

            return data;
        }


        public async Task MovePageSequenceUp(int Id)
        {
            IFPageControlMap entity = await this.GetPageControlMap(Id);

            if (entity != null)
            {
                this.MoveUpOne<IFPageControlMap>(entity.Sequence, e => e.ParentId == entity.ParentId);
            }
        }

        public async Task MovePageSequenceDown(int Id)
        {
            IFPageControlMap entity = await this.GetPageControlMap(Id);

            if (entity != null)
            {
                this.MoveDownOne<IFPageControlMap>(entity.Sequence, e => e.ParentId == entity.ParentId);
            }
        }
        public async Task AddPage(IFPage form)
        {
            IFPageControlMap entity = new IFPageControlMap();

            entity.ParentId = null;
            entity.IFPageControlId = form.Id;
            IFPage page = new IFPage();
            string name = DirectoryHelper.AddAsLastWord(form.Name, "Page");
            page.Name = name;
            page.PageLayoutId = form.PageLayoutId;
            page.ProcessId = form.ProcessId;
            page.Description = form.Description;
            page.IFProjectId = form.IFProjectId;
            page.ControlType = PageControlType.Page;
            entity.IFPageControl = page;

            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();


            form.Id = entity.Id;
        }

        public async Task<List<IFPageControlItemModelProperty>> GetPageControlItemModelProperties(int id)
        {
            var data = await this.GetQuery<IFPageControlItemModelProperty>(c => c.ObjectId == id)
                .Include(p => p.IFPageForm)
                .Include(p => p.IFPageGrid)

                .OrderBy(a => a.Sequence).ToListAsync();

            return data;
        }

        public async Task UpdatePage(IFPage form)
        {

            try
            {
                var entity = await this.GetQuery<IFPage>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                string name = DirectoryHelper.AddAsLastWord(form.Name, "Page");
                entity.Name = name;
                entity.Description = form.Description;
                entity.PageLayoutId = form.PageLayoutId;
                entity.ProcessId = form.ProcessId;
                entity.IFProjectId = form.IFProjectId;

                this.Update(entity);
                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IFPage> GetPage(int id)
        {
            var entity = await this.GetQuery<IFPage>()

            .Select(x => new IFPage
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                PageLayoutId = x.PageLayoutId,
                ProcessId = x.ProcessId,
                IFProjectId = x.IFProjectId


            }).SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException("Page : No such entity exists"); }

            return entity;
        }


        public async Task<List<IFPage>> GetPageList()
        {
            var data = await this.GetQuery<IFPage>()
                                .Select(x => new IFPage
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Description = x.Description,
                                }).ToListAsync();

            return data;
        }

        public async Task<List<IFPageLayout>> GetPageLayoutList()
        {
            var data = await this.GetQuery<IFPageLayout>()
                          .Select(x => new IFPageLayout
                          {
                              Id = x.Id,
                              Name = x.Name,

                              Description = x.Description
                          }).ToListAsync();

            return data;
        }

        public async Task<List<IFPageControlMap>> GetPageControlListTree()
        {


            try
            {
                var list = await this.GetQuery<IFPageControlMap>()
                    .Include(p => p.IFPageControl)
                    .Include(p => p.Parent.IFPageControl)
                    .Include(p => p.Childrens).ThenInclude(c => c.IFPageControl)


                        .ToListAsync();

                return list;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task<List<PageControlTreeDto>> GetPageControlMapTreeList(int ParentId)
        {
            List<PageControlTreeDto> tree = null;

            try
            {
                var list = await this.GetQuery<IFPageControlMap>()
                    .Include(p => p.IFPageControl).Select
               (map =>
                new PageControlTreeDto
                {

                    Name = map.IFPageControl.Name,
                    Id = map.Id,
                    ParentId = map.ParentId,
                    PageControlId = map.IFPageControlId,
                    PageControl = map.IFPageControl,
                    SortOrder = map.Sequence

                }).ToListAsync();

                var parents = list.Where(c => c.Id == ParentId).ToList();


                tree = list.ToTree(parents);
            }
            catch (Exception ex)
            {

                throw;
            }

            return tree;
        }

        public async Task<IFPageControlMap> GetPageTree(int pageId)
        {

            try
            {
                List<IFPageControlMap> all = await GetAllPageList();

                var page = all.Where(c => c.IFPageControlId == pageId).SingleOrDefault();

                return page;

            }
            catch (Exception ex)
            {

                throw;
            }


        }

        private async Task<List<IFPageControlMap>> GetAllPageList()
        {
            var list = await this.GetQuery<IFPageControlMap>()
                   .Include(e => e.IFPageControl)
                    .Include(p => p.Parent.IFPageControl)
                    .Include(p => p.Childrens).ThenInclude(c => c.IFPageControl)
                .Include(p => p.IFPageControl).ThenInclude(e => ((IFPage)e).PageLayout)
                .Include(p => p.IFPageControl).ThenInclude(e => ((IFPage)e).Process.Project.Solution)
                .Include(p => p.IFPageControl).ThenInclude(k => ((IFPage)k).IFProject.Solution)
                .Include(p => p.IFPageControl).ThenInclude(k => ((IFPageGrid)k).Query.Model.Properties).ThenInclude(c => c.EntityProperty.Entity)
                .Include(p => p.IFPageControl).ThenInclude(k => ((IFPageAction)k).IFModel)
                .Include(p => p.IFPageControl).ThenInclude(k => ((IFPageAction)k).IFPageControl)
                .Include(p => p.IFPageControl).ThenInclude(k => ((IFPageAction)k).IFPageActionRouteValues).ThenInclude(r => r.IFModelProperty.EntityProperty)
                .Include(p => p.IFPageControl).ThenInclude(k => ((IFPageAction)k).IFPageActionRouteValues).ThenInclude(r => r.IFModelProperty.Entity)
                .Include(p => p.IFPageControl).ThenInclude(k => ((IFPageAction)k).IFPageActionRouteValues).ThenInclude(r => r.IFPageParameter)
                .Include(p => p.IFPageControl).ThenInclude(k => ((IFPageAction)k).Query.Model)
                .Include(p => p.IFPageControl).ThenInclude(k => ((IFPageAction)k).Command.Model)
                .Include(p => p.IFPageControl).ThenInclude(k => ((IFPageAction)k).Query.QueryFilterItems).ThenInclude(f => f.EntityProperty)
                .Include(p => p.IFPageControl).ThenInclude(k => ((IFPageListView)k).IFQuery.Model.Properties).ThenInclude(f => f.EntityProperty.Entity)
                .Include(p => p.IFPageControl).ThenInclude(e => ((IFPageForm)e).IFModel)
                .Include(p => p.IFPageControl).ThenInclude(e => ((IFPageForm)e).IFQuery)
                .Include(p => p.IFPageControl).ThenInclude(e => ((IFPageForm)e).IFPageFormItemModelProperties).ThenInclude(f => f.IFModelProperty.EntityProperty)
                .Include(p => p.IFPageControl).ThenInclude(e => ((IFPageForm)e).IFPageFormItemModelProperties).ThenInclude(f => f.IFPageFormItem)



            .ToListAsync();
            return list;
        }

        private async Task<List<IFPageControlMap>> GetAllPageControlList()
        {
            var pagecontrolList = await this.GetQuery<IFPageControlMap>()
                    .Include(p => p.IFPageControl).ToListAsync();

            return pagecontrolList;
        }

        public async Task<List<PageControlTreeDto>> GetPageControlMapMenuList(int projectId)
        {
            List<PageControlTreeDto> list = null;

            try
            {
                list = await this.GetQuery<IFPageControlMap>(c => c.IFPageControl.ControlType == PageControlType.Page || c.IFPageControl.ControlType == PageControlType.MenuItem)
                    .Where(k => ((IFPage)k.IFPageControl).IFProjectId == projectId || ((IFPageNavigation)k.IFPageControl).IFProjectId == projectId)
                    .Include(p => p.IFPageControl.IFPageControlMap)
                    .Select(map =>
                        new PageControlTreeDto
                        {

                            Name = map.IFPageControl.Name,
                            Id = map.Id,
                            ParentId = map.ParentId,
                            PageControlId = map.IFPageControlId,
                            PageControl = map.IFPageControl,
                        }).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

            return list;
        }

        public async Task<List<PageControlTreeDto>> GetPageControlMapMenuTree(int? projectId)
        {
            List<PageControlTreeDto> tree = null;

            try
            {
                List<PageControlTreeDto> list = new List<PageControlTreeDto>();


                var list1 = await this.GetQuery<IFPageNavigation>(c => c.IFProjectId == projectId).Include(p => p.IFPageControlMap)

               .Select(map =>
                new PageControlTreeDto
                {

                    Name = map.Name,
                    PageControlId = map.Id,
                    ParentId = map.IFPageControlMap.ParentId,
                    Id = map.IFPageControlMap.Id,
                    PageControl = map.IFPageControlMap.IFPageControl


                }).ToListAsync();

                list1 = list1.Where(c => c.Id != 0).ToList();

                list.AddRange(list1);

                var list2 = await this.GetQuery<IFPage>(c => c.IFProjectId == projectId).Include(p => p.IFPageControlMap)

                 .Select(map =>
                new PageControlTreeDto
                {

                    Name = map.Name,
                    PageControlId = map.Id,
                    ParentId = map.IFPageControlMap.ParentId,
                    Id = map.IFPageControlMap.Id,
                    PageControl = map.IFPageControlMap.IFPageControl,


                }).ToListAsync();

                list2 = list2.Where(c => c.Id != 0).ToList();

                list.AddRange(list2);

                tree = list.ToTree();
            }
            catch (Exception ex)
            {

                throw;
            }

            return tree;
        }

        public async Task<List<PageControlTreeDto>> GetPageControlMapMenuTree()
        {
            List<PageControlTreeDto> tree = null;

            try
            {
                var list = await this.GetQuery<IFPageControlMap>(c => c.IFPageControl.ControlType == PageControlType.Page || c.IFPageControl.ControlType == PageControlType.MenuItem)
                    .Include(p => p.IFPageControl).Select
               (map =>
                new PageControlTreeDto
                {

                    Name = map.IFPageControl.Name,
                    Id = map.Id,
                    ParentId = map.ParentId,
                    PageControlId = map.IFPageControlId,
                    PageControl = map.IFPageControl,


                }).ToListAsync();

                tree = list.ToTree();
            }
            catch (Exception ex)
            {

                throw;
            }

            return tree;
        }

        public async Task<List<IFPageControl>> GetPageControlList()
        {
            var data = await this.GetQuery<IFPageControl>().ToListAsync();

            return data;
        }

        public async Task<List<IFPageControlMap>> GetPageControlMapList()
        {
            var data = await this.GetQuery<IFPageControlMap>()
                             .Include(c => c.IFPageControl)
                             .ToListAsync();

            return data;
        }




        public async Task AddPageContolMap(int pageControlId, int? parentPageControlId)
        {
            IFPageControlMap entity = new IFPageControlMap();

            entity.ParentId = parentPageControlId;
            entity.IFPageControlId = pageControlId;

            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();

        }


        public async Task UpdatePageContolMap(int pageControlId, int parentPageControlId)
        {

            try
            {
                var entity = await this.GetQuery<IFPageControlMap>()
            .SingleOrDefaultAsync(k => k.Id == pageControlId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                entity.ParentId = parentPageControlId;


                this.Update(entity);
                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task DeletePageContolMap(int id)
        {
            try
            {
                var entity = await this.GetQuery<IFPageControlMap>()
            .SingleOrDefaultAsync(k => k.Id == id);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }


                this.Delete(entity);
                await this.UnitOfWork.SaveChangesAsync();


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IFPageControlMap> GetPageControlMap(int Id)
        {
            var entity = await this.GetQuery<IFPageControlMap>()
                .Include(e => e.IFPageControl)
                    .Include(p => p.Parent.IFPageControl)
                    .Include(p => p.Childrens).ThenInclude(c => c.IFPageControl)
                .SingleOrDefaultAsync(k => k.Id == Id);

            if (entity == null) { throw new BusinessException($"{nameof(IFPageControlMap)} : No such entity exists"); }

            return entity;
        }

        public async Task<IFPageControlMap> GetPageControlMapByControlId(int PageControlId)
        {

            var entity = await this.GetQuery<IFPageControlMap>()
                .Include(e => e.IFPageControl)
                .Include(p => p.Parent.IFPageControl)
                .Include(p => p.Childrens).ThenInclude(c => c.IFPageControl)
                .SingleOrDefaultAsync(k => k.IFPageControlId == PageControlId);

            //if (entity == null) { throw new BusinessException($"{nameof(IFPageControlMap)} : No such entity exists"); }

            return entity;

        }





        public async Task<List<PageControlTreeDto>> GetPageNavigationMapTreeList()
        {

            List<PageControlTreeDto> tree = null;

            try
            {


                var list = await this.GetQuery<IFPageControlMap>(c => c.IFPageControl.ControlType == PageControlType.MenuItem).Include(p => p.IFPageControl).Select
               (map =>
                new PageControlTreeDto
                {

                    Name = map.IFPageControl.Name,
                    Id = map.Id,
                    ParentId = map.ParentId,
                    PageControlId = map.IFPageControlId,
                    PageControl = map.IFPageControl,


                }).ToListAsync();


                tree = list.ToTree();
            }
            catch (Exception ex)
            {

                throw;
            }

            return tree;

        }

        public async Task AddPageNavigation(IFPageNavigation form)
        {
            try
            {
                IFPageNavigation entity = new IFPageNavigation();
                entity.Name = form.Name;
                entity.Description = form.Description;
                entity.ControlType = PageControlType.MenuItem;
                entity.IFProjectId = form.IFProjectId;

                this.Add(entity);

                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<IFPageNavigation>> GetPageNavigationList()
        {
            var data = await this.GetQuery<IFPageNavigation>(c => c.ControlType == PageControlType.MenuItem).ToListAsync();

            return data;
        }

        public async Task<List<IFPageNavigation>> GetPageNavigationList(int? projectId)
        {

            var data = await this.GetQuery<IFPageNavigation>(c => c.ControlType == PageControlType.MenuItem && c.IFProjectId == projectId).ToListAsync();

            return data;
        }

        public async Task<IFPageNavigation> GetPageNavigation(int id)
        {

            var entity = await this.GetQuery<IFPageNavigation>().SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException($"{nameof(IFPageNavigation)} : No such entity exists"); }

            return entity;
        }

        public async Task UpdatePageNavigation(IFPageNavigation form)
        {
            try
            {
                var entity = await this.GetQuery<IFPageNavigation>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                entity.Name = form.Name;
                entity.Description = form.Description;
                entity.IFProjectId = form.IFProjectId;

                this.Update(entity);
                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task DeletePageNavigation(int id)
        {
            try
            {
                var entity = await this.GetQuery<IFPageNavigation>()
            .SingleOrDefaultAsync(k => k.Id == id);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }


                this.Delete(entity);
                await this.UnitOfWork.SaveChangesAsync();


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Task<List<IFPageParameter>> GetPageParameters(int id)
        {
            return this.GetQuery<IFPageParameter>(o => o.ObjectId == id).ToListAsync();
        }


        public async Task UpdateControlItemModelProperties(List<IFPageControlItemModelProperty> dtos, int objectId)
        {
            try
            {
                var IFPageControlItemModelProperties = await this.GetQuery<IFPageControlItemModelProperty>(k => k.ObjectId == objectId)
               
               .ToListAsync();

                //if (entity == null) { throw new BusinessException(" No such entity exists"); }

                for (int i = 0; i < IFPageControlItemModelProperties.Count; i++)
                {
                    if (!dtos.Any(d => d.Id == IFPageControlItemModelProperties.ElementAt(i).Id))
                    {
                        this.Delete(IFPageControlItemModelProperties.ElementAt(i));
                    }
                }

                foreach (var dto in dtos)
                {

                    if (dto.Id <= 0)
                    {
                        IFPageControlItemModelProperty property = new IFPageControlItemModelProperty();
                        property.IFModelPropertyId = dto.IFModelPropertyId;
                        property.ObjectId = objectId;
                        property.IFPageFormItemId = dto.IFPageFormItemId;
                        this.Add(property);
                    }
                    else
                    {
                        var property = IFPageControlItemModelProperties.SingleOrDefault(p => p.Id == dto.Id);
                        property.IFModelPropertyId = dto.IFModelPropertyId;
                        property.ObjectId = objectId;
                        property.IFPageFormItemId = dto.IFPageFormItemId;
                        this.Update(property);
                    }
                }

                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task UpdatePageParameters(List<IFPageParameter> dtos, int ObjectId, PageParameterType ObjectType)
        {
            try
            {
                var IFPageParameters = await this.GetQuery<IFPageParameter>(k => k.ObjectId == ObjectId && k.ObjectType == ObjectType)
               .ToListAsync();

                //if (IFPageParameters == null) { throw new BusinessException(" No such entity exists"); }

                for (int i = 0; i < IFPageParameters.Count; i++)
                {
                    if (!dtos.Any(d => d.Id == IFPageParameters.ElementAt(i).Id))
                    {
                        this.Delete(IFPageParameters.ElementAt(i));
                    }
                }

                foreach (var dto in dtos)
                {

                    if (dto.Id <= 0)
                    {
                        IFPageParameter property = new IFPageParameter();

                        property.ObjectId = ObjectId;
                        property.Type = dto.Type;
                        property.Name = dto.Name;
                        property.ObjectType = ObjectType;
                        //IFPageParameters.Add(property);
                        this.Add(property);
                    }
                    else
                    {
                        var property = IFPageParameters.SingleOrDefault(p => p.Id == dto.Id);
                        property.ObjectId = ObjectId;
                        property.Type = dto.Type;
                        property.Name = dto.Name;
                        this.Update(property);
                    }
                }



                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

}
