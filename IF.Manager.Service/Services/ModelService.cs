using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.Model;
using IF.Persistence.EF;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Service.Services
{
    public class ModelService : GenericRepository, IModelService
    {
        private readonly IClassService classService;
        public ModelService(ManagerDbContext dbContext, IClassService classService) : base(dbContext)
        {
            this.classService = classService;
        }




        public async Task SaveModel(ModelUpdateDto dtos, int queryId)
        {
            try
            {
                var entity = await this.GetQuery<IFModel>()
               .Include(e => e.Properties)
               .SingleOrDefaultAsync(k => k.Id == queryId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                var selectedProperties = dtos.GetProperties();

                for (int i = 0; i < entity.Properties.Count; i++)
                {
                    if (!selectedProperties.Any(d => d.ModelPropertyId == entity.Properties.ElementAt(i).Id))
                    {
                        this.Delete(entity.Properties.ElementAt(i));
                    }
                }

                foreach (var selectedProperty in selectedProperties)
                {

                    if (selectedProperty.ModelPropertyId <= 0)
                    {
                        IFModelProperty modelProperty = new IFModelProperty();
                        modelProperty.EntityId = selectedProperty.EntityId;
                        modelProperty.EntityPropertyId = selectedProperty.EntityPropertyId;
                        entity.Properties.Add(modelProperty);
                    }
                    else
                    {
                        var modelProperty = entity.Properties.SingleOrDefault(p => p.Id == selectedProperty.ModelPropertyId);
                        modelProperty.EntityId = selectedProperty.EntityId;
                        modelProperty.EntityPropertyId = selectedProperty.EntityPropertyId;
                        this.Update(modelProperty);
                    }
                }

                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<ModelDto>> GetModelList()
        {
            var data = await this.GetQuery<IFModel>()
                                .Select(x => new ModelDto
                                {
                                    Id = x.Id,
                                    Name = x.Name,

                                }).OrderBy(m => m.Name).ToListAsync();

            return data;
        }

        public async Task AddModel(ModelDto form)
        {

            string name = DirectoryHelper.AddAsLastWord(form.Name, "DataModel");

            IFModel entity = new IFModel();
            entity.Id = form.Id;
            entity.Name = name;
            entity.EntityId = form.EntityId;
            entity.Description = form.Description;
            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();
            form.Id = entity.Id;
        }

        public async Task UpdateModel(ModelDto form)
        {

            try
            {
                var modelEntity = await this.GetQuery<IFModel>()
              .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (modelEntity == null) { throw new BusinessException(" No such entity exists"); }


                string name = DirectoryHelper.AddAsLastWord(form.Name, "DataModel");


                modelEntity.EntityId = form.EntityId;
                modelEntity.Name = name;
                modelEntity.Description = form.Description;
                this.Update(modelEntity);
                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ModelDto> GetModel(int id)
        {
            var entity = await this.GetQuery<IFModel>()
            .Select(x => new ModelDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                EntityId = x.EntityId



            }).SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException("Model : No such entity exists"); }

            return entity;
        }

        public async Task<List<ModelPropertyDto>> GetModelPropertyList(int modelId)
        {

            var data = await this.GetQuery<IFModelProperty>(p => p.ModelId == modelId)
                                .Include(m => m.EntityProperty)
                                .Include(m => m.Entity)
                                .Select(x => new ModelPropertyDto
                                {
                                    EntityId = x.EntityId,
                                    EntityPropertyId = x.EntityPropertyId,
                                    Name = x.EntityProperty.Name,
                                    EntityName = x.Entity.Name,
                                    ModelPropertyId = x.Id


                                })
                                .OrderBy(m => m.EntityName)
                                .ToListAsync();

            return data;
        }
    }
}
