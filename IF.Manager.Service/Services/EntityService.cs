using IF.Core.Data;
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


        public async Task<EntityTreeDto> GetEntityTree(int entityId)
        {

            var entity = await GetQuery<IFEntity>()             
             .SingleOrDefaultAsync(e => e.Id == entityId);


            EntityTreeDto tree = new EntityTreeDto();
            tree.Id = entity.Id;
            tree.ClientId = "self-" + entityId;
            tree.Name = entity.Name;
            tree.Type = entity.Name;
            

            var properties = await this.GetEntityPropertyList(entityId);

            foreach (var property in properties)
            {
                EntityTreeDto child = new EntityTreeDto();
                child.Id = property.Id;
                child.ClientId = $"{property.Id}-{entityId}";
                child.Name = property.Name;
                child.ParentId = entityId;
                child.Type = property.Type;
                child.IsNullable = property.IsNullable;
                tree.Childs.Add(child);
            }


            await ToTreeRelations(tree);
            await ToTreeReverseRelations(tree);

            return tree;

        }

        private async Task ToTreeRelations(EntityTreeDto parent)
        {
            var entity = await GetQuery<IFEntity>()
                .Include(e => e.Relations).ThenInclude(r => r.Relation)             
             .SingleOrDefaultAsync(e => e.Id == parent.Id);

            foreach (var relation in entity.Relations)
            {
                EntityTreeDto child = new EntityTreeDto();
                child.Id = relation.RelationId;
                child.ClientId = "relation-" + relation.RelationId;
                child.Name = relation.Relation.Name;
                child.ParentId = parent.Id;
                child.IsRelation = true;
                child.Type = relation.Relation.Name;

                if (relation.Type == Contracts.Enum.EntityRelationType.OneToMany)
                {
                    child.IsList = true;
                }

                parent.Childs.Add(child);

                var properties = await this.GetEntityPropertyList(child.Id);

                foreach (var property in properties)
                {
                    EntityTreeDto childProperty = new EntityTreeDto();
                    childProperty.Id = property.Id;
                    childProperty.ClientId = $"{property.Id}-{child.Id}";
                    childProperty.Name = property.Name;
                    childProperty.Type = property.Type;
                    childProperty.ParentId = child.Id;
                    childProperty.IsNullable = property.IsNullable;

                    child.Childs.Add(childProperty);
                }


                await this.ToTreeRelations(child);

            }

            
        }

        private async Task ToTreeReverseRelations(EntityTreeDto parent)
        {
            var entity = await GetQuery<IFEntity>()
                .Include(e => e.ReverseRelations).ThenInclude(r => r.Entity)
             .SingleOrDefaultAsync(e => e.Id == parent.Id);

            foreach (var relation in entity.ReverseRelations)
            {
                EntityTreeDto child = new EntityTreeDto();
                child.Id = relation.EntityId;
                child.ClientId = "relation-" + relation.EntityId;
                child.Name = relation.Entity.Name;
                child.ParentId = parent.Id;
                child.IsRelation = true;
                child.Type = relation.Entity.Name;

                if (relation.Type == Contracts.Enum.EntityRelationType.OneToMany)
                {
                    child.IsList = true;
                }

                parent.Childs.Add(child);

                var properties = await this.GetEntityPropertyList(child.Id);

                foreach (var property in properties)
                {
                    EntityTreeDto childProperty = new EntityTreeDto();
                    childProperty.Id = property.Id;
                    childProperty.ClientId = $"{property.Id}-{child.Id}";
                    childProperty.Name = property.Name;
                    childProperty.Type = property.Type;
                    childProperty.ParentId = child.Id;
                    childProperty.IsNullable = property.IsNullable;

                    child.Childs.Add(childProperty);
                }


                await this.ToTreeReverseRelations(child);

            }


        }

        public async Task<List<EntityDto>> GetEntityList()
        {



            var list = await GetQuery<IFEntity>()
                .Include(e => e.Group)
                .Include(e => e.Properties)
                .Include(e => e.Relations)
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
                       To = r.From,
                       From = r.To
                   }).ToList(),
                   Relations = e.Relations.Select(r => new EntityRelationDto
                   {
                       EntityRelationType = r.Type,
                       Id = r.Id,
                       IFRelatedEntityId = r.RelationId,
                       RelatedEntityName = r.Relation.Name,
                       IFEntityId = r.EntityId,
                       To = r.To,
                       From = r.From

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

            string name = DirectoryHelper.AddAsLastWord(dto.Name, "Entity");

            IFEntity entity = new IFEntity();
            entity.Description = dto.Description;
            entity.Name = name;
            entity.GroupId = dto.GroupId;
            entity.IsAudited = dto.IsAudited;
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

                if (dtos.Any(d => d.Id <= 0))
                {
                    EntityPropertyDto primaryKeyProperty = new EntityPropertyDto();
                    primaryKeyProperty.IsIdentity = true;
                    primaryKeyProperty.Name = "Id";
                    primaryKeyProperty.Type = "int";
                    primaryKeyProperty.IsNullable = false;
                    dtos.Add(primaryKeyProperty);
                }

                foreach (var dto in dtos)
                {

                    if (dto.Id <= 0)
                    {
                        IFEntityProperty entityProperty = new IFEntityProperty();
                        entityProperty.IsIdentity = dto.IsIdentity;
                        entityProperty.MaxValue = dto.MaxValue;
                        entityProperty.Name = dto.Name;
                        entityProperty.Id = dto.Id;
                        entityProperty.Type = dto.Type;
                        entityProperty.IsAudited = dto.IsAudited;
                        entityProperty.IsMultiLanguage = dto.IsMultiLanguage;
                        entityProperty.EntityId = entityId;
                        entityProperty.IsNullable = dto.IsNullable;
                        this.Add(entityProperty);
                    }
                    else
                    {
                        var entityProperty = await this.GetQuery<IFEntityProperty>(p => p.Id == dto.Id).SingleOrDefaultAsync();
                        entityProperty.IsIdentity = dto.IsIdentity;
                        entityProperty.MaxValue = dto.MaxValue;
                        entityProperty.Name = dto.Name;
                        entityProperty.Type = entityProperty.Type;
                        entityProperty.IsAudited = dto.IsAudited;
                        entityProperty.IsMultiLanguage = dto.IsMultiLanguage;
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


        public async Task UpdateEntity(EntityDto dto)
        {


            var entity = await this.GetQuery<IFEntity>()

                .SingleOrDefaultAsync(k => k.Id == dto.Id);

            if (entity == null) { throw new BusinessException(" No such entity exists"); }

            string name = DirectoryHelper.AddAsLastWord(dto.Name,"Entity");

            entity.Name = name;
            entity.Description = dto.Description;
            entity.IsAudited = dto.IsAudited;
            entity.GroupId = dto.GroupId;
            this.Update(entity);
            await this.UnitOfWork.SaveChangesAsync();

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



        public async Task UpdateEntityRelations(List<EntityRelationDto> dtos, int entityId)
        {
            try
            {
                var entity = await this.GetQuery<IFEntity>()
                   .Include(e => e.Relations)
               .SingleOrDefaultAsync(k => k.Id == entityId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                foreach (var dto in dtos)
                {
                    if (dto.Id > 0) continue;

                    IFEntityRelation relation = new IFEntityRelation();
                    relation.EntityId = entityId;
                    relation.RelationId = dto.IFRelatedEntityId;
                    relation.Type = dto.EntityRelationType;

                    switch (dto.EntityRelationType)
                    {
                        case Contracts.Enum.EntityRelationType.None:
                            break;
                        case Contracts.Enum.EntityRelationType.OneToMany:
                            relation.From = Contracts.Enum.EntityRelationDirectionType.One;
                            relation.To = Contracts.Enum.EntityRelationDirectionType.Many;

                            var relatedEntity = await this.GetQuery<IFEntity>()
                                                        .Include(e => e.Properties)
                                                        .SingleOrDefaultAsync(k => k.Id == dto.IFRelatedEntityId);

                            if (entity == null) { throw new BusinessException(" No such entity exists"); }

                            string name = DirectoryHelper.AddAsLastWord(entity.Name, "Entity");

                            relatedEntity.Properties.Add(new IFEntityProperty()
                            {
                                //Caglar:Foreign Key
                                Name = $"{name}Id",
                                Type = "int"

                            });


                            break;
                        case Contracts.Enum.EntityRelationType.OneToOne:
                            relation.From = Contracts.Enum.EntityRelationDirectionType.One;
                            relation.To = Contracts.Enum.EntityRelationDirectionType.One;
                            break;
                        case Contracts.Enum.EntityRelationType.ManyToMany:
                            relation.From = Contracts.Enum.EntityRelationDirectionType.Many;
                            relation.To = Contracts.Enum.EntityRelationDirectionType.Many;
                            break;
                        default:
                            break;
                    }

                    DbContext.Entry(relation).State = EntityState.Added;
                }

                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<EntityRelationDto>> GetEntityRelationList(int entityId)
        {
            var list = await GetQuery<IFEntityRelation>()
                .Where(r => r.EntityId == entityId)
                  .Select(e => new EntityRelationDto
                  {
                      Id = e.Id,
                      EntityRelationType = e.Type,
                      IFRelatedEntityId = e.RelationId
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
            entity.Id = form.Id;
            entity.Name = form.Name;
            entity.Prefix = form.Prefix;
            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();
            form.Id = entity.Id;
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
                IsAudited = x.IsAudited
            }).SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException("Entity : No such entity exists"); }

            return entity;
        }


    }
}
