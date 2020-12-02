using DatabaseSchemaReader.DataSchema;

using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
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
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Service
{
    public class ClassService : GenericRepository, IClassService
    {
        private readonly FileSystemCodeFormatProvider fileSystem;
        public ClassService(ManagerDbContext dbContext) : base(dbContext)
        {

            this.fileSystem = new FileSystemCodeFormatProvider(DirectoryHelper.GetTempGeneratedDirectoryName());


        }


        public async Task<List<ClassControlTreeDto>> GetClassPropertyList(int classId)
        {
            var list = await GetQuery<IFKClass>().Where(e => e.ParentId == classId)
               .Select(e => new ClassControlTreeDto
               {
                   Id = e.Id,
                   Name = e.Name,
                   Type = e.Type,
                   GenericType = e.GenericType,
                   IsNullable = e.IsNullable,
                   Description = e.Description


               })
               .ToListAsync();



            return list;
        }



        public async Task<List<IFKClass>> GetClassList()
        {


            var list = await this.GetQuery<IFKClass>(c => c.ParentId == null).ToListAsync();

            return list;
        }

        public async Task AddClass(IFKClass form)
        {
            try
            {
                IFKClass entity = new IFKClass();
                entity.Id = form.Id;
                string type = DirectoryHelper.AddAsLastWord(form.Type, "CustomClass");
                entity.Type = type;
                entity.Name = form.Name;
                entity.GenericType = form.GenericType;
                entity.IsPrimitive = false;
                entity.ParentId = form.ParentId;
                entity.Description = form.Description;
                entity.Childrens = form.Childrens;
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
                    GenericType = map.GenericType,
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

                string type = DirectoryHelper.AddAsLastWord(form.Type, "CustomClass");
                entity.Type = type;
                entity.Name = form.Name;
                entity.Description = form.Description;
                entity.GenericType = form.GenericType;
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
                        property.GenericType = dto.GenericType;
                        property.IsPrimitive = true;
                        property.ParentId = classId;
                        property.IsNullable = dto.IsNullable;
                        property.Description = dto.Description;
                        this.Add(property);
                    }
                    else
                    {
                        var property = await this.GetQuery<IFKClass>(p => p.Id == dto.Id && p.ParentId == classId).SingleOrDefaultAsync();
                        property.Name = dto.Name;
                        property.IsPrimitive = true;
                        property.ParentId = classId;
                        property.Description = dto.Description;
                        property.Type = property.Type;
                        property.GenericType = dto.GenericType;
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
            var map = await this.GetClass(classId);

            var dto = new ClassControlTreeDto
            {

                Name = map.Name,
                Id = map.Id,
                ParentId = map.ParentId,
                Type = map.Type,
                GenericType = map.GenericType,
                IsPrimitive = map.IsPrimitive,
                Description = map.Description,
                IsNullable = map.IsNullable

            };

           

            var childs = await this.GetClassTreeList(classId);

            dto.Childs = childs;

            List<CSClass> allClass = new List<CSClass>();
            

            CSClass csClass = new CSClass();

            GenerateClassTree(dto, csClass,allClass);


            StringBuilder code = new StringBuilder();

            foreach (var @class in allClass)
            {
                code.AppendLine(@class.GenerateCode().Template);
            }


            fileSystem.FormatCode(code.ToString(), "cs",dto.Name);
        }

        private static void GenerateClassTree(ClassControlTreeDto mainClass, CSClass csClass,List<CSClass> allClass)
        {
            allClass.Add(csClass);

            csClass.Name = mainClass.Type;
            

            foreach (var child in mainClass.Childs)
            {
                if (child.IsPrimitive)
                {
                    CSProperty property = new CSProperty("public", child.Name, child.IsNullable);
                    property.PropertyTypeString = child.Type;
                    property.GenericType = child.GenericType;
                    csClass.Properties.Add(property);
                }
                else
                {
                    CSProperty property = new CSProperty("public", child.Name, false);
                    property.PropertyTypeString = child.Type;
                    property.GenericType = child.GenericType;
                    csClass.Properties.Add(property);

                    CSClass childClass = new CSClass();                        
                    
                    GenerateClassTree(child, childClass,allClass);

                }

            }


        }
    }


}
