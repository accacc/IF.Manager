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

        //public async Task AddClassGroup(EntityGroupDto form)
        //{
        //    CustomClassGroup entity = new CustomClassGroup();
        //    entity.Id = form.Id;
        //    entity.Name = form.Name;
        //    entity.Prefix = form.Prefix;
        //    this.Add(entity);
        //    await this.UnitOfWork.SaveChangesAsync();
        //    form.Id = entity.Id;
        //}

        //public async Task UpdateClassGroup(EntityGroupDto form)
        //{

        //    var entity = await this.GetQuery<CustomClassGroup>()
        //       .SingleOrDefaultAsync(k => k.Id == form.Id);

        //    if (entity == null) { throw new BusinessException(" No such entity exists"); }

        //    entity.Name = form.Name;
        //    entity.Prefix = form.Prefix;
        //    this.Update(entity);
        //    await this.UnitOfWork.SaveChangesAsync();
        //}

        //public async Task<EntityGroupDto> GetClassGroup(int id)
        //{
        //    var entity = await this.GetQuery<CustomClassGroup>()
        //    .Select(x => new EntityGroupDto
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //        Prefix = x.Prefix,
        //    }).SingleOrDefaultAsync(k => k.Id == id);

        //    if (entity == null) { throw new BusinessException("CustomClassGroup : No such entity exists"); }

        //    return entity;
        //}


        //public async Task<List<EntityGroupDto>> GetClassGroupList()
        //{
        //    var data = await this.GetQuery<CustomClassGroup>()
        //                        .Select(x => new EntityGroupDto
        //                        {
        //                            Id = x.Id,
        //                            Name = x.Name,
        //                            Prefix = x.Prefix,
        //                        }).ToListAsync();

        //    return data;
        //}

        //public async Task<List<List<EntityDto>>> GetEntityListGrouped()
        //{


        //    var entitylist = await this.GetEntityList();
        //    var list = entitylist.GroupBy(l => l.GroupId).Select(s => s.ToList()).ToList();

        //    return list;
        //}

        //public async Task<List<EntityDto>> GetEntityList()
        //{



        //    var list = await GetQuery<CustomClass>()
        //        .Include(e => e.Group)
        //        .Include(e => e.CustomClassProperties)
        //        .Include(e => e.Relations).ThenInclude(r=>r.MainCustomClass)//.ThenInclude(r => r.ForeignKeyIFEntityProperty).ThenInclude(r => r.Entity)
        //        .Include(e => e.ReverseRelations).ThenInclude(r => r.RelatedCustomClass)



        //       .Select(e => new EntityDto
        //       {
        //           Id = e.Id,
        //           Description = e.Description,
        //           Name = e.Name,
        //           Prefix = e.Group.Prefix,
        //           GroupId = e.Group.Id,
        //           GroupName = e.Group.Name,


        //           IsAudited = e.IsAudited,
        //           Properties = e.CustomClassProperties.Select(p => new EntityPropertyDto
        //           {
        //               Id = p.Id,
        //               //IFEntityId = p.EntityId,
        //               //IsIdentity = p.IsIdentity,
        //               //MaxValue = p.MaxValue,
        //               Name = p.Name,
        //               Type = p.Type,
        //               IsNullable = p.IsNullable,
        //               //IsMultiLanguage = p.IsMultiLanguage,
        //               //IsAudited = p.IsAudited

        //           }).ToList(),
        //           ReverseRelations = e.ReverseRelations.Select(r => new EntityRelationDto
        //           {
        //               //EntityRelationType = r.Type,
        //               Id = r.Id,
        //               //IFRelatedEntityId = r.RelationId,
        //               //RelatedEntityName = r.Entity.Name,
        //               //IFEntityId = r.EntityId,
        //               //EntityName = r.Relation.Name,
        //               //To = r.From,
        //               //From = r.To

        //           }).ToList(),
        //           Relations = e.Relations.Select(r => new EntityRelationDto
        //           {
        //               //EntityRelationType = r.Type,
        //               Id = r.Id,
        //               //IFRelatedEntityId = r.RelationId,
        //               //RelatedEntityName = r.Relation.Name,
        //               //IFEntityId = r.EntityId,
        //               //EntityName = r.Entity.Name,
        //               //ForeignKeyPropertyId = r.ForeignKeyIFEntityPropertyId,
        //               //ForeignKeyPropertyName = r.ForeignKeyIFEntityProperty.Name
        //               //To = r.To,
        //               //From = r.From

        //           }).ToList()

        //       })
        //       .ToListAsync();



        //    return list;
        //}


        //public async Task UpdateClassRelations(List<IFCustomClassRelation> relations, int classId)
        //{
        //    try
        //    {
        //        var entity = await this.GetQuery<IFCustomClass>()
        //           .Include(e => e)
        //       .SingleOrDefaultAsync(k => k.Id == classId);

        //        if (entity == null) { throw new BusinessException(" No such entity exists"); }

        //        foreach (var dto in relations)
        //        {
        //            if (dto.Id > 0) continue;

        //            IFCustomClassRelation relation = new IFCustomClassRelation();
        //            relation.MainIFCustomClassId = classId;
        //            relation.RelatedIFCustomClassId = dto.RelatedIFCustomClassId;
        //            relation.RelationType = dto.RelationType;
        //            relation.Name = dto.Name;

        //            DbContext.Entry(relation).State = EntityState.Added;
        //        }

        //        await UnitOfWork.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}


        //public async Task<List<IFCustomClassRelation>> GetClassRelationList(int classId)
        //{
        //    var list = await GetQuery<IFCustomClassRelation>(c=>c.MainIFCustomClassId == classId)

        //          .ToListAsync();
        //    return list;
        //}

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

        //public async Task<List<EntityPropertyDto>> GetClassPropertyList(int classId)
        //{
        //    var list = await GetQuery<CustomClassProperty>().Where(e => e.CustomClassId == classId)
        //       .Select(e => new EntityPropertyDto
        //       {
        //           Id = e.Id,
        //           Name = e.Name,
        //           //IsAudited = e.IsAudited,
        //           //IsMultiLanguage = e.IsMultiLanguage,
        //           //IFEntityId = e.EntityId,
        //           //IsIdentity = e.IsIdentity,
        //           //MaxValue = e.MaxValue,
        //           Type = e.Type,
        //           IsNullable = e.IsNullable

        //       })
        //       .ToListAsync();



        //    return list;
        //}

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

        //public async Task<List<IFCustomClassProperty>> GetClassPropertyList(int classId)
        //{
        //    var list = await GetQuery<IFCustomClassProperty>().Where(e => e.IFCustomClassId == classId)
              
        //       .ToListAsync();



        //    return list;
        //}

        public async Task UpdateClassProperties(List<ClassControlTreeDto> dtos, int classId)
        {
            try
            {
                var entity = await this.GetQuery<IFKClass>()
               .SingleOrDefaultAsync(k => k.Id == classId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }


                foreach (var dto in dtos)
                {

                    if (dto.Id <= 0)
                    {
                        IFKClass entityProperty = new IFKClass();
                        //entityProperty.IsIdentity = dto.IsIdentity;
                        //entityProperty.MaxValue = dto.MaxValue;
                        entityProperty.Name = dto.Name;
                        entityProperty.Id = dto.Id;
                        entityProperty.Type = dto.Type;
                        entity.IsPrimitive = false;
                        entity.ParentId = classId;
                        //entityProperty.IsAudited = dto.IsAudited;
                        //entityProperty.IsMultiLanguage = dto.IsMultiLanguage;
                        //entityProperty.EntityId = classId;
                        entityProperty.IsNullable = dto.IsNullable;
                        entity.Description = dto.Description;
                        this.Add(entityProperty);
                    }
                    else
                    {
                        var entityProperty = await this.GetQuery<IFKClass>(p => p.Id == dto.Id).SingleOrDefaultAsync();
                        //entityProperty.IsIdentity = dto.IsIdentity;
                        //entityProperty.MaxValue = dto.MaxValue;
                        entityProperty.Name = dto.Name;
                        entity.IsPrimitive = false;
                        entity.Description = dto.Description;
                        entityProperty.Type = entityProperty.Type;
                        //entityProperty.IsAudited = dto.IsAudited;
                        //entityProperty.IsMultiLanguage = dto.IsMultiLanguage;
                        entityProperty.IsNullable = dto.IsNullable;
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
