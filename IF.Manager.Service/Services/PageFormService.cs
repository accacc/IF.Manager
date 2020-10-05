using IF.Core.Control;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
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
    public class PageFormService : GenericRepository, IPageFormService
    {

        private readonly IModelService modelService;
        public PageFormService(ManagerDbContext dbContext,IModelService modelService) : base(dbContext)
        {
            this.modelService = modelService;
        }
        public async Task<List<IFPageForm>> GetFormList()
        {
            var data = await this.GetQuery<IFPageForm>().ToListAsync();

            return data;
        }

        public async Task<List<IFPageFormLayout>> GetFormLayouts()
        {
            var data = await this.GetQuery<IFPageFormLayout>().ToListAsync();

            return data;
        }


        public async Task AddForm(IFPageForm form)
        {
            IFPageForm entity = new IFPageForm();
            string name = DirectoryHelper.AddAsLastWord(form.Name, "PageForm");
            entity.Name = name;
            entity.ControlType = PageControlType.Form;
            entity.IFModelId = form.IFModelId;
            entity.IFQueryId = form.IFQueryId;
            entity.Description = form.Description;
            entity.FormLayoutId = form.FormLayoutId;
            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();
            form.Id = entity.Id;
        }

        public async Task UpdateForm(IFPageForm form)
        {

            try
            {
                var entity = await this.GetQuery<IFPageForm>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException($"{nameof(IFPageForm)} : No such entity exists"); }

                string name = DirectoryHelper.AddAsLastWord(form.Name, "PageForm");
                entity.Name = name;
                entity.IFModelId = form.IFModelId;
                entity.IFQueryId = form.IFQueryId;
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

        public async Task<IFPageForm> GetForm(int id)
        {
            var entity = await this.GetQuery<IFPageForm>()
            .Include(f => f.IFQuery)
            .Include(f => f.IFModel)
            .SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException($"{nameof(IFPageForm)} : No such entity exists"); }

            return entity;
        }

        public async Task<List<ModelPropertyDto>> GetFormModelProperties(int Id)
        {
            var form = await this.GetForm(Id);

            List<ModelPropertyDto> modelProperties;

            if (form.IFModelId.HasValue)
            {
                modelProperties = await this.modelService.GetModelPropertyList(form.IFModelId.Value);
            }
            else if (form.IFQueryId.HasValue)
            {
                modelProperties = await this.modelService.GetModelPropertyList(form.IFQuery.ModelId);
            }
            else
            {
                throw new BusinessException("Bu form için model tanımı yok");
            }

            return modelProperties;
        }

        public async Task<List<IFPageFormItem>> GetFormItems()
        {
            var data = await this.GetQuery<IFPageFormItem>().ToListAsync();

            return data;
        }

        public async Task<List<IFPageControlItemModelProperty>> GetPageFormItemModelProperties(int id)
        {
            var data = await this.GetQuery<IFPageControlItemModelProperty>(c => c.ObjectId == id).OrderBy(a=>a.Sequence).ToListAsync();

            return data;
        }

        public async Task<IFPageControlItemModelProperty> GetPageControlItemModelProperty(int iFPageFormItemModelPropertyId)
        {
            var data = await this.GetQuery<IFPageControlItemModelProperty>(c => c.Id == iFPageFormItemModelPropertyId).SingleOrDefaultAsync();

            return data;
        }

        public async Task UpdateFormItemModelProperties(List<IFPageControlItemModelProperty> dtos, int formId)
        {
            try
            {
                var entity = await this.GetQuery<IFPageForm>()
               .Include(e => e.IFPageFormItemModelProperties)
               .SingleOrDefaultAsync(k => k.Id == formId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                for (int i = 0; i < entity.IFPageFormItemModelProperties.Count; i++)
                {
                    if (!dtos.Any(d => d.Id == entity.IFPageFormItemModelProperties.ElementAt(i).Id))
                    {
                        this.Delete(entity.IFPageFormItemModelProperties.ElementAt(i));
                    }
                }

                foreach (var dto in dtos)
                {

                    if (dto.Id <= 0)
                    {
                        IFPageControlItemModelProperty property = new IFPageControlItemModelProperty();
                        property.IFModelPropertyId = dto.IFModelPropertyId;
                        property.ObjectId = formId;
                        property.IFPageFormItemId = dto.IFPageFormItemId;
                        entity.IFPageFormItemModelProperties.Add(property);
                    }
                    else
                    {
                        var property = entity.IFPageFormItemModelProperties.SingleOrDefault(p => p.Id == dto.Id);
                        property.IFModelPropertyId = dto.IFModelPropertyId;
                        property.ObjectId = formId;
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

        public async Task AddPageControlItemModelProperty(IFPageControlItemModelProperty form)
        {
            await this.AddAsync(form);
            await this.UnitOfWork.SaveChangesAsync();
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

        public void MoveModelItemUp(int Id)
        {
            IFPageControlItemModelProperty entity = this.GetPageFormItemModelProperty(Id);
            
            if (entity != null)
            {
                this.MoveUpOne<IFPageControlItemModelProperty>(entity.Sequence);
            }
        }

        public void MoveModelItemDown(int Id)
        {
            IFPageControlItemModelProperty entity = this.GetPageFormItemModelProperty(Id);

            if (entity != null)
            {
                this.MoveDownOne<IFPageControlItemModelProperty>(entity.Sequence);
            }
        }

        private IFPageControlItemModelProperty GetPageFormItemModelProperty(int Id)
        {       
        
            return this.GetByKey<IFPageControlItemModelProperty>(Id);
        
        }
    }
}

