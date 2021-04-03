
using IF.Core.Data;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.CodeGen;
using IF.Persistence.EF;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IF.Manager.Service
{
    public class EntityService : GenericRepository, IEntityService
    {

        public EntityService(ManagerDbContext dbContext) : base(dbContext)
        {

        }


        public async Task<List<EntityDto>> GetEntityAllRelations(int id)
        {

            List<EntityDto> list = new List<EntityDto>();

            var entity = await GetQuery<IFEntity>()
           .Include(e => e.Relations).ThenInclude(r => r.Relation)
           .SingleOrDefaultAsync(e => e.Id == id);

            EntityDto entityDto = new EntityDto();
            entityDto.Id = entity.Id;
            entityDto.Name = entity.Name;
            entityDto.Properties = new List<EntityPropertyDto>();

            var properties = await this.GetEntityPropertyList(id);

            foreach (var property in properties)
            {
                EntityPropertyDto entityPropertyDto = new EntityPropertyDto();
                entityPropertyDto.Id = property.Id;
                entityPropertyDto.Name = property.Name;
                entityDto.Properties.Add(property);
            }

            list.Add(entityDto);

            await ToAllRelations(entityDto, list);


            return list;
        }


        private async Task ToAllRelations(EntityDto parent, List<EntityDto> list)
        {
            var entity = await GetQuery<IFEntity>()
             .Include(e => e.Relations).ThenInclude(r => r.Relation)
             .SingleOrDefaultAsync(e => e.Id == parent.Id);

            foreach (var relation in entity.Relations)
            {
                EntityDto entityDto = new EntityDto();
                entityDto.Id = relation.RelationId;
                entityDto.Name = relation.Relation.Name;
                entityDto.Properties = new List<EntityPropertyDto>();


                var properties = await this.GetEntityPropertyList(entityDto.Id);

                foreach (var property in properties)
                {
                    EntityPropertyDto entityProperty = new EntityPropertyDto();
                    entityProperty.Id = property.Id;
                    entityProperty.Name = property.Name;
                    entityDto.Properties.Add(entityProperty);
                }

                list.Add(entityDto);

                await this.ToAllRelations(entityDto, list);

            }
        }


        public async Task<ModelClassTreeDto> GetEntityTree(int entityId)
        {

            var entity = await GetQuery<IFEntity>()
             .SingleOrDefaultAsync(e => e.Id == entityId);


            ModelClassTreeDto tree = new ModelClassTreeDto();
            tree.Id = entity.Id;
            tree.ClientId = "self-" + entityId;
            tree.Name = entity.Name;
            tree.Type = entity.Name;


            var entityProperties = await this.GetEntityPropertyList(entityId);

            foreach (var entityProperty in entityProperties)
            {
                ModelClassTreeDto child = new ModelClassTreeDto();
                child.Id = entityProperty.Id;
                child.ClientId = $"{entityProperty.Id}-{entityId}";
                child.Name = entityProperty.Name;
                child.ParentId = entityId;
                child.Type = entityProperty.Type;
                child.IsNullable = entityProperty.IsNullable;
                child.IsIdentity = entityProperty.IsIdentity;
                tree.Childs.Add(child);
            }


            await ToTreeRelations(tree);
            await ToTreeReverseRelations(tree);

            return tree;

        }

        private async Task ToTreeRelations(ModelClassTreeDto parent)
        {
            var entity = await GetQuery<IFEntity>()
                .Include(e => e.Relations).ThenInclude(r => r.Relation)
             .SingleOrDefaultAsync(e => e.Id == parent.Id);

            foreach (var relation in entity.Relations)
            {
                ModelClassTreeDto child = new ModelClassTreeDto();
                child.Id = relation.RelationId;
                child.ClientId = "relation-" + relation.RelationId;
                child.Name = relation.Relation.Name;
                child.ParentId = parent.Id;
                child.IsRelation = true;
                child.Type = relation.Relation.Name;

                if (relation.Type == Contracts.Enum.EntityRelationType.ManyToMany ||
                        relation.Type == Contracts.Enum.EntityRelationType.OneToMany)
                {
                    child.IsList = true;
                }

                parent.Childs.Add(child);

                var entityProperties = await this.GetEntityPropertyList(child.Id);

                foreach (var entityProperty in entityProperties)
                {
                    ModelClassTreeDto childProperty = new ModelClassTreeDto();
                    childProperty.Id = entityProperty.Id;
                    childProperty.ClientId = $"{entityProperty.Id}-{child.Id}";
                    childProperty.Name = entityProperty.Name;
                    childProperty.Type = entityProperty.Type;
                    childProperty.ParentId = child.Id;
                    childProperty.IsNullable = entityProperty.IsNullable;
                    child.IsIdentity = entityProperty.IsIdentity;

                    child.Childs.Add(childProperty);
                }

                if (relation.Type == EntityRelationType.OneToOne) continue;

                await this.ToTreeRelations(child);

            }


        }

        private async Task ToTreeReverseRelations(ModelClassTreeDto parent)
        {
            var entity = await GetQuery<IFEntity>()
                .Include(e => e.ReverseRelations).ThenInclude(r => r.Entity)
             .SingleOrDefaultAsync(e => e.Id == parent.Id);

            foreach (var relation in entity.ReverseRelations)
            {
                ModelClassTreeDto child = new ModelClassTreeDto();
                child.Id = relation.EntityId;
                child.ClientId = "relation-" + relation.EntityId;
                child.Name = relation.Entity.Name;
                child.ParentId = parent.Id;
                child.IsRelation = true;
                child.Type = relation.Entity.Name;

                if (relation.Type == Contracts.Enum.EntityRelationType.ManyToMany)
                {
                    child.IsList = true;
                }

                parent.Childs.Add(child);

                var entityProperties = await this.GetEntityPropertyList(child.Id);

                foreach (var entityProperty in entityProperties)
                {
                    ModelClassTreeDto childProperty = new ModelClassTreeDto();
                    childProperty.Id = entityProperty.Id;
                    childProperty.ClientId = $"{entityProperty.Id}-{child.Id}";
                    childProperty.Name = entityProperty.Name;
                    childProperty.Type = entityProperty.Type;
                    childProperty.ParentId = child.Id;
                    childProperty.IsNullable = entityProperty.IsNullable;
                    childProperty.IsIdentity = entityProperty.IsIdentity;

                    child.Childs.Add(childProperty);
                }


                if (relation.Type == EntityRelationType.OneToOne) continue;

                await this.ToTreeReverseRelations(child);

            }


        }

        public async Task<List<EntityDto>> GetEntityList()
        {
            var list = await GetQuery<IFEntity>()
                .Include(e => e.Group)
                .Include(e => e.Properties)
                .Include(e => e.Relations).ThenInclude(r => r.ForeignKeyIFEntityProperty)
                .ThenInclude(r => r.Entity)
                .Include(e => e.ReverseRelations).ThenInclude(r => r.Relation)



               .Select(e => new EntityDto
               {
                   Id = e.Id,
                   Description = e.Description,
                   Name = e.Name,
                   Prefix = e.Group.Prefix,
                   GroupId = e.Group.Id,
                   GroupName = e.Group.Name,
                   IsSoftDeleted = e.IsSoftDeleted,
                   IsAudited = e.IsAudited,
                   Properties = e.Properties.Select(p => new EntityPropertyDto
                   {
                       Id = p.Id,
                       IFEntityId = p.EntityId,
                       IsIdentity = p.IsIdentity,
                       MaxValue = p.MaxValue,
                       Name = p.Name,
                       Type = p.Type,
                       IsNullable = p.IsNullable,
                       IsMultiLanguage = p.IsMultiLanguage,
                       IsAudited = p.IsAudited

                   }).ToList(),
                   ReverseRelations = e.ReverseRelations.Select(r => new EntityRelationDto
                   {
                       EntityRelationType = r.Type,
                       Id = r.Id,
                       IFRelatedEntityId = r.RelationId,
                       RelatedEntityName = r.Entity.Name,
                       IFEntityId = r.EntityId,
                       EntityName = r.Relation.Name,

                   }).ToList(),
                   Relations = e.Relations.Select(r => new EntityRelationDto
                   {
                       EntityRelationType = r.Type,
                       Id = r.Id,
                       IFRelatedEntityId = r.RelationId,
                       RelatedEntityName = r.Relation.Name,
                       IFEntityId = r.EntityId,
                       EntityName = r.Entity.Name,
                       ForeignKeyPropertyId = r.ForeignKeyIFEntityPropertyId,
                       ForeignKeyPropertyName = r.ForeignKeyIFEntityProperty.Name

                   }).ToList()

               })
               .ToListAsync();



            return list;
        }

        public async Task<List<List<EntityDto>>> GetEntityListGrouped()
        {
            var entitylist = await this.GetEntityList();
            var list = entitylist.GroupBy(l => l.GroupId).Select(s => s.ToList()).ToList();
            return list;
        }

        public async Task<List<EntityPropertyDto>> GetEntityPropertyList(int entityId)
        {
            var list = await GetQuery<IFEntityProperty>().Where(e => e.EntityId == entityId)
               .Select(e => new EntityPropertyDto
               {
                   Id = e.Id,
                   Name = e.Name,
                   IsAudited = e.IsAudited,
                   IsMultiLanguage = e.IsMultiLanguage,
                   IFEntityId = e.EntityId,
                   IsIdentity = e.IsIdentity,
                   MaxValue = e.MaxValue,
                   Type = e.Type,
                   IsNullable = e.IsNullable

               })
               .ToListAsync();

            return list;
        }

        public async Task AddEntity(EntityDto dto)
        {
            if (await EntityIsExistByName(dto.Name))
            {
                throw new BusinessException($"{dto.Name} Entity already exist");
            }

            string name = ObjectNamerHelper.AddAsLastWord(dto.Name, "Entity");

            IFEntity entity = new IFEntity();
            entity.Description = dto.Description;
            entity.Name = name;
            entity.GroupId = dto.GroupId;
            entity.IsAudited = dto.IsAudited;
            entity.IsSoftDeleted = dto.IsSoftDeleted;

            IFEntityProperty primaryKeyProperty = new IFEntityProperty();
            primaryKeyProperty.IsIdentity = true;
            primaryKeyProperty.IsAutoNumber = true;
            primaryKeyProperty.Name = "Id";
            primaryKeyProperty.Type = "int";
            primaryKeyProperty.IsNullable = false;
            entity.Properties.Add(primaryKeyProperty);

            this.Add(entity);
            await UnitOfWork.SaveChangesAsync();
        }


        public async Task<bool> EntityIsExistByName(string name)
        {
            bool isExist = false;
            IFEntity entity = await this.GetQuery<IFEntity>(s => s.Name == name).SingleOrDefaultAsync();

            if (entity != null)
            {
                isExist = true;
            }

            return isExist;
        }

        public async Task UpdateEntityProperties(List<EntityPropertyDto> dtos, int entityId)
        {
            try
            {
                var entity = await this.GetQuery<IFEntity>()
               .SingleOrDefaultAsync(k => k.Id == entityId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }


                foreach (var dto in dtos)
                {

                    if (dto.Id <= 0)
                    {
                        IFEntityProperty entityProperty = new IFEntityProperty();
                        //entityProperty.IsIdentity = dto.IsIdentity;
                        entityProperty.MaxValue = dto.MaxValue;
                        entityProperty.Name = dto.Name;
                        entityProperty.Id = dto.Id;
                        entityProperty.Type = dto.Type;
                        entityProperty.IsAudited = dto.IsAudited;
                        entityProperty.IsMultiLanguage = dto.IsMultiLanguage;
                        entityProperty.EntityId = entityId;
                        entityProperty.IsNullable = dto.IsNullable;
                        entityProperty.IsIdentity = dto.IsIdentity;
                        this.Add(entityProperty);
                    }
                    else
                    {
                        var entityProperty = await this.GetQuery<IFEntityProperty>(p => p.Id == dto.Id).SingleOrDefaultAsync();
                        //entityProperty.IsIdentity = dto.IsIdentity;
                        entityProperty.MaxValue = dto.MaxValue;
                        entityProperty.Name = dto.Name;
                        entityProperty.Type = entityProperty.Type;
                        entityProperty.IsAudited = dto.IsAudited;
                        entityProperty.IsMultiLanguage = dto.IsMultiLanguage;
                        entityProperty.IsNullable = dto.IsNullable;
                        entityProperty.IsIdentity = dto.IsIdentity;

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


        public async Task UpdateEntity(EntityDto dto)
        {


            var entity = await this.GetQuery<IFEntity>().Include(e => e.Properties)
                .SingleOrDefaultAsync(k => k.Id == dto.Id);

            if (entity == null) { throw new BusinessException(" No such entity exists"); }

            string name = ObjectNamerHelper.AddAsLastWord(dto.Name, "Entity");

            entity.Name = name;
            entity.Description = dto.Description;
            entity.IsAudited = dto.IsAudited;
            entity.GroupId = dto.GroupId;
            entity.IsSoftDeleted = dto.IsSoftDeleted;

            //Todo:Caglar db first yuzunden iptal edildi, zaten code first de entity eklerken primary key ekleniyor otomatik
            //if(!entity.Properties.Any(p=>p.IsIdentity && p.Name == "Id"))
            //{
            //    IFEntityProperty primaryKeyProperty = new IFEntityProperty();
            //    primaryKeyProperty.IsIdentity = true;
            //    primaryKeyProperty.Name = "Id";
            //    primaryKeyProperty.Type = "int";
            //    primaryKeyProperty.IsNullable = false;
            //    entity.Properties.Add(primaryKeyProperty);
            //}

            this.Update(entity);
            await this.UnitOfWork.SaveChangesAsync();

        }





        public List<Type> GetAllEntityTypes()
        {
            Assembly assembly = Assembly.Load("IF.Manager.Contracts");

            var derivedType = typeof(Entity);

            List<Type> types = new List<Type>();

            foreach (var item in assembly.GetTypes())
            {
                if (item.BaseType != null && derivedType.FullName == item.BaseType.FullName)
                {
                    types.Add(item);
                }
            }

            return types;

        }



        public async Task UpdateEntityRelations(List<EntityRelationDto> relationDtos, int entityId)
        {
            try
            {
                var entity = await this.GetQuery<IFEntity>()
                   .Include(e => e.Relations)
               .SingleOrDefaultAsync(k => k.Id == entityId);

                if (entity == null) { throw new BusinessException(" No such entity exists code:1111"); }

                foreach (var relationDto in relationDtos)
                {
                    if (relationDto.Id > 0) continue;

                   

                    switch (relationDto.EntityRelationType)
                    {
                        case Contracts.Enum.EntityRelationType.None:
                            break;
                        case Contracts.Enum.EntityRelationType.OneToMany:

                            IFEntityRelation relation = GenerateNewRelation(entityId, relationDto.IFRelatedEntityId,relationDto.EntityRelationType,relationDto.ForeignKeyPropertyId,relationDto.Prefix);

                            await AddForeignKey(entity.Name,relationDto.IFRelatedEntityId,relation);

                            DbContext.Entry(relation).State = EntityState.Added;


                            break;
                        case Contracts.Enum.EntityRelationType.OneToOne:

                            IFEntityRelation relationOne = GenerateNewRelation(entityId, relationDto.IFRelatedEntityId, relationDto.EntityRelationType, relationDto.ForeignKeyPropertyId, relationDto.Prefix);

                            await AddForeignKey(entity.Name,relationDto.IFRelatedEntityId, relationOne);

                            DbContext.Entry(relationOne).State = EntityState.Added;


                            IFEntityRelation relationTwo = GenerateNewRelation(relationDto.IFRelatedEntityId, entityId, relationDto.EntityRelationType, relationDto.ForeignKeyPropertyId, relationDto.Prefix);

                            var entityTwo = await this.GetEntity(relationDto.IFRelatedEntityId);

                            if (entityTwo == null) { throw new BusinessException(" No such entity exists code:1110"); }

                            await AddForeignKey(entityTwo.Name,entityId, relationTwo);

                            DbContext.Entry(relationTwo).State = EntityState.Added;




                            break;
                        case Contracts.Enum.EntityRelationType.ManyToMany:
                            break;
                        default:
                            break;
                    }

                    
                }

                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static IFEntityRelation GenerateNewRelation(int entityId,int IFRelatedEntityId,EntityRelationType entityRelationType,int? ForeignKeyPropertyId, string prefix)
        {
            IFEntityRelation relation = new IFEntityRelation();
            relation.EntityId = entityId;
            relation.RelationId = IFRelatedEntityId;
            relation.Type = entityRelationType;
            relation.ForeignKeyIFEntityPropertyId = ForeignKeyPropertyId;
            relation.Prefix = prefix;
            return relation;
        }

        //private async Task AddForeignKey(int relatedEntityId, IFEntityRelation relation)
        //{
        //    if (!relation.ForeignKeyIFEntityPropertyId.HasValue || relation.ForeignKeyIFEntityPropertyId <= 0)
        //    {
        //        var relatedEntity = await this.GetQuery<IFEntity>()
        //                                    .Include(e => e.Properties)
        //                                    .SingleOrDefaultAsync(k => k.Id == relatedEntityId);

        //        //if (entity == null) { throw new BusinessException("No such entity exists"); }

        //        string name = relatedEntity.Name;

        //        var foreignKeyProperty = new IFEntityProperty()
        //        {
        //            Name = $"{name}Id",
        //            Type = "int",
        //            IsNullable= true

        //        };

        //        relatedEntity.Properties.Add(foreignKeyProperty);

        //        relation.ForeignKeyIFEntityProperty = foreignKeyProperty;

        //    }
        //}

        private async Task AddForeignKey(string name, int relatedEntityId, IFEntityRelation relation)
        {
            if (!relation.ForeignKeyIFEntityPropertyId.HasValue || relation.ForeignKeyIFEntityPropertyId <= 0)
            {
                var relatedEntity = await this.GetQuery<IFEntity>()
                                            .Include(e => e.Properties)
                                            .SingleOrDefaultAsync(k => k.Id == relatedEntityId);

                if (relatedEntity == null) { throw new BusinessException("No such entity exists"); }

                var foreignKeyProperty = new IFEntityProperty()
                {
                    Name = $"{name}Id",
                    Type = "int",
                    IsNullable = true

                };

                relatedEntity.Properties.Add(foreignKeyProperty);

                relation.ForeignKeyIFEntityProperty = foreignKeyProperty;

            }
        }

        public async Task<List<EntityRelationDto>> GetEntityRelationList(int entityId)
        {
            var list = await GetQuery<IFEntityRelation>().Include(e => e.ForeignKeyIFEntityProperty)
                .Where(r => r.EntityId == entityId)
                  .Select(e => new EntityRelationDto
                  {
                      Id = e.Id,
                      EntityRelationType = e.Type,
                      IFRelatedEntityId = e.RelationId,
                      Prefix = e.Prefix,
                      ForeignKeyPropertyId = e.ForeignKeyIFEntityPropertyId,
                      ForeignKeyPropertyName = e.ForeignKeyIFEntityProperty.Name

                  })
                  .ToListAsync();
            return list;
        }

        public async Task<List<EntityGroupDto>> GetEntityGroupList()
        {
            var data = await this.GetQuery<IFEntityGroup>()
                                .Select(x => new EntityGroupDto
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Prefix = x.Prefix,
                                }).ToListAsync();

            return data;
        }

        public async Task AddEntityGroup(EntityGroupDto form)
        {
            IFEntityGroup entity = new IFEntityGroup();
            entity.Name = form.Name;
            entity.Prefix = form.Prefix;
            this.Add(entity);
            form.Id = entity.Id;
            await this.UnitOfWork.SaveChangesAsync();
            
        }

        public async Task UpdateEntityGroup(EntityGroupDto form)
        {

            var entity = await this.GetQuery<IFEntityGroup>()
               .SingleOrDefaultAsync(k => k.Id == form.Id);

            if (entity == null) { throw new BusinessException(" No such entity exists"); }

            entity.Name = form.Name;
            entity.Prefix = form.Prefix;
            this.Update(entity);
            await this.UnitOfWork.SaveChangesAsync();
        }

        public async Task<EntityGroupDto> GetEntityGroup(int id)
        {
            var entity = await this.GetQuery<IFEntityGroup>()
            .Select(x => new EntityGroupDto
            {
                Id = x.Id,
                Name = x.Name,
                Prefix = x.Prefix,
            }).SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException("EntityGroup : No such entity exists"); }

            return entity;
        }

        public async Task<EntityDto> GetEntity(int id)
        {
            var entity = await this.GetQuery<IFEntity>()
            .Select(x => new EntityDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                GroupId = x.GroupId,
                IsAudited = x.IsAudited,
                IsSoftDeleted = x.IsSoftDeleted
            }).SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException("Entity : No such entity exists"); }

            return entity;
        }

       

      
        public string[] GetPrimitives()
        {

            return new[] {
                "string",
                "int",
                "decimal",
                "DateTime",
                "bool"
            };

        }

        
    }


}
