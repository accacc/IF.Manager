
using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Core.Data;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.Model;
using IF.Persistence.EF;
using IF.Manager.Service.Extensions;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;

namespace IF.Manager.Service
{
    public class ClassService : GenericRepository, IClassService
    {
        private readonly FileSystemCodeFormatProvider fileSystem;
        public ClassService(ManagerDbContext dbContext) : base(dbContext)
        {

            this.fileSystem = new FileSystemCodeFormatProvider(DirectoryHelper.GetTempGeneratedDirectoryName());


        }



        public async Task JsonToClass(string name,string json)
        {
            JsonClassGenerator gen = GetTypes(name,json);

            var types = gen.Types;

            IFClass mainClass = new IFClass();
            mainClass.Name = name;
            mainClass.Type = name;
            mainClass.IsPrimitive = true;

            IFClass list = new IFClass();

            mainClass.Childrens.Add(list);
            var rootObject = types.Where(t => t.IsRoot).SingleOrDefault();
            var rchilds = rootObject.Fields.Select(s => s.MemberName).ToList();
            var rootChilds = types.Where(t => rchilds.Contains(t.AssignedName)).ToList();
            GenerateClasses(list, rootChilds, types);

            await this.AddClass(mainClass);
        }

        private void GenerateClasses(IFClass @class, List<JsonType> rootChilds, IList<JsonType> types)
        {
            foreach (var type in rootChilds)
            {
                //IFClass @class = new IFClass();

                HandleType(type, type.AssignedName, @class);

                if (type.Type == JsonTypeEnum.Object)
                {
                    @class.IsPrimitive = false;

                    foreach (var field in type.Fields)
                    {
                        IFClass property = new IFClass();

                        HandleType(field.Type, field.MemberName, property);

                        @class.Childrens.Add(property);

                        if (field.Type.Type == JsonTypeEnum.Object || (field.Type.InternalType != null && field.Type.InternalType.Type == JsonTypeEnum.Object))
                        {
                            GenerateClasses(property, types.Where(t => t.AssignedName == field.MemberName).ToList(), types);

                        }

                    }


                }

            }


        }

        private void HandleType(JsonType item, string name, IFClass cls)
        {
            cls.Name = name;
            cls.Type = item.Type.ToString();
            cls.IsPrimitive = true;
            cls.Description = name;

            if (item.Type == JsonTypeEnum.Array)
            {
                cls.GenericType = "List";

                if (item.InternalType.Type == JsonTypeEnum.Object)
                {
                    cls.Type = name;
                }
            }

            if (item.Type == JsonTypeEnum.Object)
            {
                cls.IsPrimitive = false;
                cls.Type = name;
            }



            if (item.Type == JsonTypeEnum.NullableSomething)
            {
                cls.Type = "string";
            }

            if (item.Type == JsonTypeEnum.Integer)
            {
                cls.Type = "int";
            }
        }

        private JsonClassGenerator GetTypes(string jsondata,string name)
        {
            var gen = new JsonClassGenerator();
            gen.Example = jsondata;
            gen.InternalVisibility = false;
            gen.CodeWriter = new CSharpCodeWriter();
            gen.ExplicitDeserialization = false;// chkExplicitDeserialization.Checked && gen.CodeWriter is CSharpCodeWriter;
            gen.Namespace = "Example";//string.IsNullOrEmpty(edtNamespace.Text) ? null : edtNamespace.Text;
            gen.NoHelperClass = false;
            gen.SecondaryNamespace = null;
            gen.TargetFolder = @"C:\Users\Lenovo\Desktop\Temp";
            gen.UseProperties = true;
            gen.MainClass = name;
            gen.UsePascalCase = true;
            

            gen.UseNestedClasses = false;
            gen.ApplyObfuscationAttributes = false;
            gen.SingleFile = true;
            gen.ExamplesInDocumentation = false;

            gen.GenerateClasses();
            return gen;
        }

        public async Task<List<IFClassMapper>> GetClassMapperList()
        {
            var data = await this.GetQuery<IFClassMapper>().ToListAsync();

            return data;
        }


        public async Task AddClassMapper(IFClassMapper form)
        {
            IFClassMapper entity = new IFClassMapper();
            string name = DirectoryHelper.AddAsLastWord(form.Name, "IFClassMapper");
            entity.Name = name;
            entity.Description = form.Description;
            entity.IFModelId = form.IFModelId;
            entity.IFClassId = form.IFClassId;
            this.Add(entity);
            await this.UnitOfWork.SaveChangesAsync();
            form.Id = entity.Id;
        }

        public async Task UpdateClassMapper(IFClassMapper form)
        {

            try
            {
                var entity = await this.GetQuery<IFClassMapper>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException($"{nameof(IFClassMapper)} : No such entity exists"); }

                string name = DirectoryHelper.AddAsLastWord(form.Name, "IFClassMapper");
                entity.Name = name;
                entity.Description = form.Description;
                entity.IFModelId = form.IFModelId;
                entity.IFClassId = form.IFClassId;
                this.Update(entity);
                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IFClassMapper> GetClassMapper(int id)
        {
            var entity = await this.GetQuery<IFClassMapper>()
            .SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException($"{nameof(IFClassMapper)} : No such entity exists"); }

            return entity;
        }


        public async Task<List<ClassControlTreeDto>> GetClassPropertyList(int classId)
        {
            var list = await GetQuery<IFClass>().Where(e => e.ParentId == classId)
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



        public async Task<List<IFClass>> GetClassList()
        {


            var list = await this.GetQuery<IFClass>(c => c.ParentId == null).ToListAsync();

            return list;
        }

        public async Task AddClass(IFClass form)
        {
            try
            {
                IFClass entity = new IFClass();
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
                var list = await this.GetQuery<IFClass>().Select

               (map => new ClassControlTreeDto
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


        public async Task UpdateClass(IFClass form)
        {

            try
            {
                var entity = await this.GetQuery<IFClass>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException($"{nameof(IFClass)} : No such entity exists"); }

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

        public async Task<IFClass> GetClass(int id)
        {
            var entity = await this.GetQuery<IFClass>()
          .SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException("IFKClass : No such entity exists"); }

            return entity;
        }

        public async Task UpdateClassProperties(List<ClassControlTreeDto> dtos, int classId)
        {
            try
            {
                var @class = await this.GetQuery<IFClass>()
               .SingleOrDefaultAsync(k => k.Id == classId);

                if (@class == null) { throw new BusinessException(" No such entity exists"); }


                foreach (var dto in dtos)
                {

                    if (dto.Id <= 0)
                    {
                        IFClass property = new IFClass();
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
                        var property = await this.GetQuery<IFClass>(p => p.Id == dto.Id && p.ParentId == classId).SingleOrDefaultAsync();
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

            var tree = await this.GetClassTreeList(classId);

            var parent = tree.First();

            List<CSClass> allClass = new List<CSClass>();
            

            CSClass csClass = new CSClass();

            csClass.Usings.Add("System");
            csClass.Usings.Add("System.Collections.Generic");

            GenerateClassTree(parent, csClass,allClass);


            StringBuilder code = new StringBuilder();

            foreach (var @class in allClass)
            {
                code.AppendLine(@class.GenerateCode().Template);
            }


            fileSystem.FormatCode(code.ToString(), "cs",parent.Name);
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

        public async Task<List<IFClassMapping>> GetClassMappings(int classMapId)
        {

            var list = await this.GetQuery<IFClassMapping>(c => c.Id == classMapId).ToListAsync();

            return list;
        }

        public async Task<List<IFClass>> GetClassTree(int classId)
        {
            var list =await this.GetQuery<IFClass>(t => t.Id == classId).ToListAsync();

            foreach (var item in list)
            {
                item.Childrens = await GetChildrenByParentId(item.Id);
            }

            return list;
        }

        private async Task<List<IFClass>> GetChildrenByParentId(int parentId)
        {
            var children = new List<IFClass>();

            var threads = await this.GetQuery<IFClass>(x => x.ParentId == parentId).ToListAsync();

            foreach (var t in threads)
            {
                t.Childrens = await GetChildrenByParentId(t.Id);
                

                children.Add(t);
            }

            return children;
        }

        public async Task<List<IFClass>> GetTreeList2(int classId)
        {
            var list = await GetClassTree(classId);

            var flat = Flatten(list.First());

            return flat.ToList();

        }
        private IEnumerable<IFClass> Flatten(IFClass @class)
        {
            yield return @class;

            foreach (var node in @class.Childrens.SelectMany(child => Flatten(child)))
            {
                yield return node;
            }
        }

        public async Task DeleteClass(int id)
        {
            var list = await GetTreeList2(id);

            foreach (var item in list)
            {
                Delete(item);
            }
            await this.UnitOfWork.SaveChangesAsync();

        }

        //public async Task DeleteRecursive(IFClass cls)
        //{
        //    foreach (var child in cls.Childrens.ToArray<IFClass>())
        //    {
        //        this.Delete(child);
        //        await DeleteRecursive(child);
        //    }

        //   await  this.UnitOfWork.SaveChangesAsync();
        //}
    }


}
