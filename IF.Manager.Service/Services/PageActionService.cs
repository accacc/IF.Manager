using IF.Core.Control;
using IF.Core.Exception;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.CodeGen;
using IF.Persistence.EF;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Service.Services
{
    public class PageActionService : GenericRepository, IPageActionService
    {
        public PageActionService(ManagerDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<List<IFPageAction>> GetActionList()
        {
            var data = await this.GetQuery<IFPageAction>().ToListAsync();

            return data;
        }

        public async Task AddAction(IFPageAction form)
        {
            IFPageAction entity = new IFPageAction();
            string name = ObjectNamerHelper.AddAsLastWord(form.Name, "PageAction");
            entity.Name = name;
            entity.ControlType = PageControlType.Action;
            entity.Text = form.Text;
            entity.Url = form.Url;
            entity.WidgetRenderType = form.WidgetRenderType;
            entity.WidgetType = form.WidgetType;
            entity.ActionType = form.ActionType;
            entity.SortOrder = form.SortOrder;
            entity.Description = form.Description;
            entity.QueryId = form.QueryId;
            entity.CommandId = form.CommandId;
            entity.IFModelId = form.IFModelId;
            entity.IFPageControlId = form.IFPageControlId;

            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();
            form.Id = entity.Id;
        }

        public async Task UpdateAction(IFPageAction form)
        {

            try
            {
                var entity = await this.GetQuery<IFPageAction>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException($"{nameof(IFPageAction)} : No such entity exists"); }

                string name = ObjectNamerHelper.AddAsLastWord(form.Name, "PageAction");

                entity.Name = name;
                entity.Description = form.Description;
                entity.Text = form.Text;
                entity.Url = form.Url;
                entity.WidgetRenderType = form.WidgetRenderType;
                entity.WidgetType = form.WidgetType;
                entity.ActionType = form.ActionType;
                entity.SortOrder = form.SortOrder;
                entity.QueryId = form.QueryId;
                entity.CommandId = form.CommandId;
                entity.IFModelId = form.IFModelId;
                entity.IFPageControlId = form.IFPageControlId;
                this.Update(entity);
                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public async Task<IFPageAction> GetAction(int id)
        {
            var entity = await this.GetQuery<IFPageAction>()
            .SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException($"{nameof(IFPageAction)} : No such entity exists"); }

            return entity;
        }

        public async Task<List<IFPageActionRouteValue>> GetPageActionRouteValues(int id)
        {

            var data = await this.GetQuery<IFPageActionRouteValue>(c => c.IFPageActionId == id).ToListAsync();

            return data;
        }

        public async Task UpdatePageActionRouteValues(List<IFPageActionRouteValue> dtos, int formId)
        {
            try
            {
                var entity = await this.GetQuery<IFPageAction>()
               .Include(e => e.IFPageActionRouteValues)
               .SingleOrDefaultAsync(k => k.Id == formId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                for (int i = 0; i < entity.IFPageActionRouteValues.Count; i++)
                {
                    if (!dtos.Any(d => d.Id == entity.IFPageActionRouteValues.ElementAt(i).Id))
                    {
                        this.Delete(entity.IFPageActionRouteValues.ElementAt(i));
                    }
                }

                foreach (var dto in dtos)
                {

                    if (dto.Id <= 0)
                    {
                        IFPageActionRouteValue property = new IFPageActionRouteValue();
                        property.IFPageActionId = formId;
                        property.IFPageParameterId = dto.IFPageParameterId;
                        property.IFModelPropertyId = dto.IFModelPropertyId;
                        entity.IFPageActionRouteValues.Add(property);
                    }
                    else
                    {
                        var property = entity.IFPageActionRouteValues.SingleOrDefault(p => p.Id == dto.Id);
                        property.IFPageActionId = formId;
                        property.IFPageParameterId = dto.IFPageParameterId;
                        property.IFModelPropertyId = dto.IFModelPropertyId;
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

