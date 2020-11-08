using DatabaseSchemaReader.DataSchema;

using IF.Core.Data;
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
using System.Reflection;
using System.Threading.Tasks;

namespace IF.Manager.Service
{
    public class ClassService : GenericRepository, IClassService
    {

        public ClassService(ManagerDbContext dbContext) : base(dbContext)
        {

           


        }

        public async Task<List<IFCustomClass>> GetClassList()
        {


            var list = await this.GetQuery<IFCustomClass>().ToListAsync();

            return list;
        }

        public async Task AddClass(IFCustomClass form)
        {
            IFCustomClass entity = new IFCustomClass();
            entity.Id = form.Id;
            string name = DirectoryHelper.AddAsLastWord(form.Name, "CustomClass");
            entity.Name = name;
            entity.Description = form.Description;
            this.Add(entity);

            await this.UnitOfWork.SaveChangesAsync();
            form.Id = entity.Id;
        }


        public async Task UpdateClass(IFCustomClass form)
        {

            try
            {
                var entity = await this.GetQuery<IFCustomClass>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException($"{nameof(IFCustomClass)} : No such entity exists"); }

                string name = DirectoryHelper.AddAsLastWord(form.Name, "CustomClass");
                entity.Name = name;
                entity.Description = form.Description;
                this.Update(entity);


                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IFCustomClass> GetClass(int id)
        {
            var entity = await this.GetQuery<IFCustomClass>()
            .Include(g => g.IFCustomClassProperties)

           .SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException($"{nameof(IFCustomClass)} : No such entity exists"); }

            return entity;
        }


        public async Task<List<IFCustomClassProperty>> GetClassPropertyList(int classId)
        {
            var list = await GetQuery<IFCustomClassProperty>().Where(e => e.IFCustomClassId == classId)
              
               .ToListAsync();



            return list;
        }

        public async Task UpdateClassProperties(List<IFCustomClassProperty> properties, int classId)
        {
            try
            {
                var @class = await this.GetQuery<IFCustomClass>()
               .SingleOrDefaultAsync(k => k.Id == classId);

                if (@class == null) { throw new BusinessException(" No such entity exists"); }


                foreach (var property in properties)
                {

                    if (property.Id <= 0)
                    {
                        IFCustomClassProperty entityProperty = new IFCustomClassProperty();
                        entityProperty.Name = property.Name;
                        entityProperty.Id = property.Id;
                        entityProperty.Type = property.Type;
                        entityProperty.IsNullable = property.IsNullable;
                        entityProperty.IFCustomClassId = classId;
                        this.Add(entityProperty);
                    }
                    else
                    {
                        var entityProperty = await this.GetQuery<IFCustomClassProperty>(p => p.Id == property.Id).SingleOrDefaultAsync();
                        entityProperty.Name = property.Name;
                        entityProperty.Type = entityProperty.Type;
                        entityProperty.IsNullable = property.IsNullable;
                        this.Update(entityProperty);
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
