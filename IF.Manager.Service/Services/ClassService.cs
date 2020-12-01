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


        public async Task<List<ClassControlTreeDto>> GetClassPropertyList(int classId)
        {
            var list = await GetQuery<IFKClass>().Where(e => e.ParentId == classId)
               .Select(e => new ClassControlTreeDto
               {
                   Id = e.Id,
                   Name = e.Name,
                   Type = e.Type,
                   IsNullable = e.IsNullable,
                   Description = e.Description
                   

               })
               .ToListAsync();



            return list;
        }

      

        public async Task<List<IFKClass>> GetClassList()
        {


            var list = await this.GetQuery<IFKClass>(c=>c.ParentId == null).ToListAsync();

            return list;
        }

        public async Task AddClass(IFKClass form)
        {
            try
            {
                IFKClass entity = new IFKClass();
                entity.Id = form.Id;
                string name = DirectoryHelper.AddAsLastWord(form.Name, "CustomClass");
                entity.Name = name;
                entity.Type = "Class";
                entity.IsPrimitive = true;
                entity.ParentId = form.ParentId;
                entity.Description = form.Description;
                this.Add(entity);

                await this.UnitOfWork.SaveChangesAsync();
                form.Id = entity.Id;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task<List<ClassControlTreeDto>> GetClassTreeList(int ParentId)
        {
            List<ClassControlTreeDto> tree = null;

            try
            {
                var list = await this.GetQuery<IFKClass>().Select
                    
               (map =>
                new ClassControlTreeDto
                {

                    Name = map.Name,
                    Id = map.Id,
                    ParentId = map.ParentId,
                    Type = map.Type,
                    IsPrimitive = map.IsPrimitive,
                    Description = map.Description,
                    IsNullable = map.IsNullable

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


        public async Task UpdateClass(IFKClass form)
        {

            try
            {
                var entity = await this.GetQuery<IFKClass>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException($"{nameof(IFKClass)} : No such entity exists"); }

                string name = DirectoryHelper.AddAsLastWord(form.Name, "IFKClass");
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

        public async Task<IFKClass> GetClass(int id)
        {
            var entity = await this.GetQuery<IFKClass>()
          .SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException("IFKClass : No such entity exists"); }

            return entity;
        }

        public async Task UpdateClassProperties(List<ClassControlTreeDto> dtos, int classId)
        {
            try
            {
                var @class = await this.GetQuery<IFKClass>()
               .SingleOrDefaultAsync(k => k.Id == classId);

                if (@class == null) { throw new BusinessException(" No such entity exists"); }


                foreach (var dto in dtos)
                {

                    if (dto.Id <= 0)
                    {
                        IFKClass property = new IFKClass();
                        property.Name = dto.Name;
                        property.Id = dto.Id;
                        property.Type = dto.Type;
                        property.IsPrimitive = false;
                        property.ParentId = classId;
                        property.IsNullable = dto.IsNullable;
                        property.Description = dto.Description;
                        this.Add(property);
                    }
                    else
                    {
                        var property = await this.GetQuery<IFKClass>(p => p.Id == dto.Id && p.ParentId == classId).SingleOrDefaultAsync();
                        property.Name = dto.Name;
                        property.IsPrimitive = false;
                        property.ParentId = classId;
                        property.Description = dto.Description;
                        property.Type = property.Type;
                        property.IsNullable = dto.IsNullable;
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

        public async Task GenerateClass(int classId)
        {
            var mainClass = await this.GetQuery<IFKClass>(c => c.Id == classId).SingleOrDefaultAsync();

            var childClasses = await this.GetQuery<IFKClass>(c => c.ParentId == classId).ToListAsync();


        }
    }


}
