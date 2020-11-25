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


        public async Task<List<EntityGroupDto>> GetEntityGroupList()
        {
            var data = await this.GetQuery<CustomClassGroup>()
                                .Select(x => new EntityGroupDto
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Prefix = x.Prefix,
                                }).ToListAsync();

            return data;
        }

        public async Task<List<List<EntityDto>>> GetEntityListGrouped()
        {


            var entitylist = await this.GetEntityList();
            var list = entitylist.GroupBy(l => l.GroupId).Select(s => s.ToList()).ToList();

            return list;
        }

        public async Task<List<EntityDto>> GetEntityList()
        {



            var list = await GetQuery<CustomClass>()
                .Include(e => e.Group)
                .Include(e => e.CustomClassProperties)
                .Include(e => e.Relations).ThenInclude(r=>r.MainCustomClass)//.ThenInclude(r => r.ForeignKeyIFEntityProperty).ThenInclude(r => r.Entity)
                .Include(e => e.ReverseRelations).ThenInclude(r => r.RelatedCustomClass)



               .Select(e => new EntityDto
               {
                   Id = e.Id,
                   Description = e.Description,
                   Name = e.Name,
                   Prefix = e.Group.Prefix,
                   GroupId = e.Group.Id,
                   GroupName = e.Group.Name,


                   IsAudited = e.IsAudited,
                   Properties = e.CustomClassProperties.Select(p => new EntityPropertyDto
                   {
                       Id = p.Id,
                       //IFEntityId = p.EntityId,
                       //IsIdentity = p.IsIdentity,
                       //MaxValue = p.MaxValue,
                       Name = p.Name,
                       Type = p.Type,
                       IsNullable = p.IsNullable,
                       //IsMultiLanguage = p.IsMultiLanguage,
                       //IsAudited = p.IsAudited

                   }).ToList(),
                   ReverseRelations = e.ReverseRelations.Select(r => new EntityRelationDto
                   {
                       //EntityRelationType = r.Type,
                       Id = r.Id,
                       //IFRelatedEntityId = r.RelationId,
                       //RelatedEntityName = r.Entity.Name,
                       //IFEntityId = r.EntityId,
                       //EntityName = r.Relation.Name,
                       //To = r.From,
                       //From = r.To

                   }).ToList(),
                   Relations = e.Relations.Select(r => new EntityRelationDto
                   {
                       //EntityRelationType = r.Type,
                       Id = r.Id,
                       //IFRelatedEntityId = r.RelationId,
                       //RelatedEntityName = r.Relation.Name,
                       //IFEntityId = r.EntityId,
                       //EntityName = r.Entity.Name,
                       //ForeignKeyPropertyId = r.ForeignKeyIFEntityPropertyId,
                       //ForeignKeyPropertyName = r.ForeignKeyIFEntityProperty.Name
                       //To = r.To,
                       //From = r.From

                   }).ToList()

               })
               .ToListAsync();



            return list;
        }


        public async Task UpdateClassRelations(List<IFCustomClassRelation> relations, int classId)
        {
            try
            {
                var entity = await this.GetQuery<IFCustomClass>()
                   .Include(e => e)
               .SingleOrDefaultAsync(k => k.Id == classId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                foreach (var dto in relations)
                {
                    if (dto.Id > 0) continue;

                    IFCustomClassRelation relation = new IFCustomClassRelation();
                    relation.MainIFCustomClassId = classId;
                    relation.RelatedIFCustomClassId = dto.RelatedIFCustomClassId;
                    relation.RelationType = dto.RelationType;
                    relation.Name = dto.Name;

                    DbContext.Entry(relation).State = EntityState.Added;
                }

                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task<List<IFCustomClassRelation>> GetClassRelationList(int classId)
        {
            var list = await GetQuery<IFCustomClassRelation>(c=>c.MainIFCustomClassId == classId)
              
                  .ToListAsync();
            return list;
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
