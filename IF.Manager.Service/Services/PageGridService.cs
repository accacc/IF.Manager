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
    public class PageGridService : GenericRepository, IPageGridService
    {
        public PageGridService(ManagerDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<List<IFPageGrid>> GetGridList()
        {
            var data = await this.GetQuery<IFPageGrid>().ToListAsync();

            return data;
        }

        public async Task<List<IFPageGridLayout>> GetGridLayouts()
        {
            var data = await this.GetQuery<IFPageGridLayout>().ToListAsync();

            return data;
        }


        public async Task AddGrid(IFPageGrid form)
        {
            IFPageGrid entity = new IFPageGrid();
            entity.Id = form.Id;
            string name = DirectoryHelper.AddAsLastWord(form.Name, "PageGrid");
            entity.Name = name;
            entity.ControlType = PageControlType.Grid;
            entity.QueryId = form.QueryId;
            entity.Description = form.Description;
            entity.GridLayoutId = entity.GridLayoutId;
            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();
            form.Id = entity.Id;
        }

        public async Task UpdateGrid(IFPageGrid form)
        {

            try
            {
                var entity = await this.GetQuery<IFPageGrid>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException($"{nameof(IFPageGrid)} : No such entity exists"); }

                string name = DirectoryHelper.AddAsLastWord(form.Name, "PageGrid");
                entity.Name = name;
                entity.Description = form.Description;
                entity.GridLayoutId = form.GridLayoutId;
                entity.QueryId = form.QueryId;
                this.Update(entity);
                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IFPageGrid> GetGrid(int id)
        {
            var entity = await this.GetQuery<IFPageGrid>()
            .Include(g => g.Query.Model.Entity)
            .Include(g => g.Query.Model.Properties).ThenInclude(g => g.EntityProperty)

           .SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException($"{nameof(IFPageGrid)} : No such entity exists"); }

            return entity;
        }

        public async Task<List<IFPagePanel>> GetPanelList()
        {
            var data = await this.GetQuery<IFPagePanel>().ToListAsync();

            return data;
        }




        public async Task AddPanel(IFPagePanel form)
        {
            IFPagePanel entity = new IFPagePanel();
            entity.Id = form.Id;
            string name = DirectoryHelper.AddAsLastWord(form.Name, "PagePanel");
            entity.Name = name;
            entity.ControlType = PageControlType.Panel;
            entity.CssClass = form.CssClass;
            entity.Description = form.Description;
            
            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();
            form.Id = entity.Id;
        }

        public async Task UpdatePanel(IFPagePanel form)
        {

            try
            {
                var entity = await this.GetQuery<IFPagePanel>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException($"{nameof(IFPagePanel)} : No such entity exists"); }

                string name = DirectoryHelper.AddAsLastWord(form.Name, "PagePanel");
                entity.Name = name;
                entity.Description = form.Description;
                entity.CssClass = form.CssClass;
                
                this.Update(entity);
                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IFPagePanel> GetPanel(int id)
        {
            var entity = await this.GetQuery<IFPagePanel>()
            

           .SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException($"{nameof(IFPagePanel)} : No such entity exists"); }

            return entity;
        }
    }
}

