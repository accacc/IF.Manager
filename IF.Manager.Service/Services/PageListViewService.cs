using IF.Core.Control;
using IF.Core.Exception;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Service.Services
{
    public class PageListViewService : GenericRepository, IPageListViewService
    {
        public PageListViewService(ManagerDbContext dbContext) : base(dbContext)
        {

        }
            public async Task<List<IFPageListView>> GetListViewList()
            {
                var data = await this.GetQuery<IFPageListView>().ToListAsync();

                return data;
            }

        //public async Task<List<IFPageListViewLayout>> GetListViewLayouts()
        //{
        //    var data = await this.GetQuery<IFPageListViewLayout>().ToListAsync();

        //    return data;
        //}


        public async Task AddListView(IFPageListView form)
            {
            IFPageListView entity = new IFPageListView();
            string name = DirectoryHelper.AddAsLastWord(form.Name, "ListView");
            entity.Name = name;
            entity.ControlType = PageControlType.ListView;
            entity.IFQueryId = form.IFQueryId;
                entity.Description = form.Description;
                entity.FormLayoutId = form.FormLayoutId;
                this.Add(entity);
                await this.UnitOfWork.SaveChangesAsync();
                form.Id = entity.Id;
            }

            public async Task UpdateListView(IFPageListView form)
            {

                try
                {
                    var entity = await this.GetQuery<IFPageListView>()
                .SingleOrDefaultAsync(k => k.Id == form.Id);

                    if (entity == null) { throw new BusinessException($"{nameof(IFPageListView)} : No such entity exists"); }

                entity.IFQueryId = form.IFQueryId;
                string name = DirectoryHelper.AddAsLastWord(form.Name, "ListView");
                entity.Name = name;
                entity.Description = form.Description;
                    entity.FormLayoutId = form.FormLayoutId;
                    this.Update(entity);
                    await this.UnitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            public async Task<IFPageListView> GetListView(int id)
            {
                var entity = await this.GetQuery<IFPageListView>()

                .Select(x => new IFPageListView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IFQueryId = x.IFQueryId,
                    FormLayoutId = x.FormLayoutId,


                }).SingleOrDefaultAsync(k => k.Id == id);

                if (entity == null) { throw new BusinessException($"{nameof(IFPageListView)} : No such entity exists"); }

                return entity;
            }

        //public async Task<List<IFPageListViewItem>> GetFormItems()
        //{
        //    var data = await this.GetQuery<IFPageListViewItem>().ToListAsync();

        //    return data;
        //}

        //public async Task<List<IFPageListViewItemModelProperty>> GetPageFormItemModelProperties(int id)
        //{
        //    var data = await this.GetQuery<IFPageListViewItemModelProperty>(c=>c.IFPageListViewId == id).ToListAsync();

        //    return data;
        //}

        //public async Task UpdateFormItemModelProperties(List<IFPageListViewItemModelProperty> dtos, int formId)
        //{
        //    try
        //    {
        //        var entity = await this.GetQuery<IFPageListView>()
        //       .Include(e => e.IFPageListViewItemModelProperties)
        //       .SingleOrDefaultAsync(k => k.Id == formId);

        //        if (entity == null) { throw new BusinessException(" No such entity exists"); }

        //        for (int i = 0; i < entity.IFPageListViewItemModelProperties.Count; i++)
        //        {
        //            if (!dtos.Any(d => d.Id == entity.IFPageListViewItemModelProperties.ElementAt(i).Id))
        //            {
        //                this.Delete(entity.IFPageListViewItemModelProperties.ElementAt(i));
        //            }
        //        }

        //        foreach (var dto in dtos)
        //        {

        //            if (dto.Id <= 0)
        //            {
        //                IFPageListViewItemModelProperty property = new IFPageListViewItemModelProperty();
        //                property.IFModelPropertyId = dto.IFModelPropertyId;
        //                property.IFPageListViewId = formId;
        //                property.IFPageListViewItemId = dto.IFPageListViewItemId;
        //                entity.IFPageListViewItemModelProperties.Add(property);
        //            }
        //            else
        //            {
        //                var property = entity.IFPageListViewItemModelProperties.SingleOrDefault(p => p.Id == dto.Id);
        //                property.IFModelPropertyId = dto.IFModelPropertyId;
        //                property.IFPageListViewId = formId;
        //                property.IFPageListViewItemId = dto.IFPageListViewItemId;
        //                this.Update(property);
        //            }
        //        }

        //        await UnitOfWork.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}
    }
    }

