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


    }
}

