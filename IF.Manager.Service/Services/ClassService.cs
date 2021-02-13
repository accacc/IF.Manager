
using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Core.Data;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.Model;
using IF.Manager.Service.Services;
using IF.Persistence.EF;

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
        private readonly IEntityService entityService;
        public ClassService(ManagerDbContext dbContext, IEntityService entityService) : base(dbContext)
        {
            this.fileSystem = new FileSystemCodeFormatProvider(DirectoryHelper.GetTempGeneratedDirectoryName());
            this.entityService = entityService;
        }

        public async Task JsonToClass(string name, string json)
        {
            JsonClassGenerator gen = GetTypes(name, json);

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

        private JsonClassGenerator GetTypes(string jsondata, string name)
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
            entity.IsList = form.IsList;
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
                entity.IsList = form.IsList;

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
                .Include(m=>m.IFClassMappings)
                .Include(m => m.IFClass.Parent).ThenInclude(c=>c.Childrens)
                .Include(m => m.IFModel.Properties)
                .Include(m=>m.IFModel.Entity.Properties)
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


            var list = await this.GetQuery<IFClass>(c => c.ParentId == null)
                .OrderBy(c=>c.Name)
                .ToListAsync();

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
                var list = await this.GetQuery<IFClass>(c=>c.Id == 702 
                
                || c.ParentId == 702
                || c.ParentId == 703
                || c.ParentId == 707
                || c.Id != 708
                )
                    .Select

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

        public async Task<string> GenerateMapper(int classMapperId)
        {

            var tree = await this.GetClassTreeList(702);

            var parent = tree.First();

            CSClass cSClass = new CSClass();
            cSClass.Name = "Mapper";

            int level = 0;
            StringBuilder builder = new StringBuilder();

            var allMappers = await this.GetQuery<IFClassMapper>(m => m.IFClassId == 702)
                .Include(m => m.IFModel)
                .Include(m => m.IFClass)
                .Include(m => m.IFClassMappings).ThenInclude(m => m.FromProperty)
                .Include(m => m.IFClassMappings).ThenInclude(m => m.ToProperty)//.Model.Commands).ThenInclude(c=>c.Parent).ThenInclude(c=>c.Childrens)
                .ToListAsync();

            var command = this.GetQuery<IFCommand>(c => c.Id == 6)
                .Include(c => c.Childrens).ThenInclude(c=>c.Childrens).ThenInclude(c => c.Childrens)

                .Include(c => c.Parent).ThenInclude(c=>c.Parent).ThenInclude(c => c.Parent)
                .Include(s => s.Model.Properties).ThenInclude(s => s.EntityProperty)
                .Include(s => s.Model.Entity.Relations).SingleOrDefault();

            var parentMap = allMappers.Where(m=>m.IFModelId == command.ModelId).SingleOrDefault();


            // var command = this.GetQuery<IFCommand>(c => c.Model.Id == parentMap.IFModel.Id).SingleOrDefault();
            string name ="";
            if(command!=null)
            {

                name = parentMap.IFModel.Name;

                builder.AppendLine($"{name} {name} = new {name}();");
            }

           

            GenerateClassTree2(parent, builder,level,allMappers,command,name);

          var me =   new CSMethod("MapMe", "void", "public");
            me.Body = builder.ToString();
            cSClass.Methods.Add(me);


           fileSystem.FormatCode(cSClass.GenerateCode(), "cs");

            return builder.ToString();
        }

        private IFCommand FindModelRecursive(IFCommand parent,IFClassMapping mapper)
        {

            var list = parent.FlattenHierarchy(x => x.Childrens);

            var commands = list.Where(l => l.ModelId == mapper.IFClassMapper.IFModelId && !l.IsMultiCommand()).ToList();

            if(commands.Any())
            {
                if(commands.Count == 1)
                {
                    var command = commands.First();
                    if (command != null)
                    {
                        return command;
                    }
                }
                else
                {
                        return commands.First();
                }
            }


            //if (parent.ModelId == modelId)
            //{
            //    return parent;
            //}


            //foreach (var command in parent.Childrens)
            //{

            //    if(command.ModelId == modelId)
            //    {
            //        return command;
            //    }

            //    if (command.Childrens.Any())
            //    {

            //      var current =   FindModelRecursive(command,modelId);

            //        if(current!=null)
            //        {
            //            return current;

            //        }
            //    }
            //}

            throw new BusinessException("Model bulunamadı");
        }

        private void GenerateClassTree2(ClassControlTreeDto mainClass, StringBuilder builder, int level,List<IFClassMapper> mappers,IFCommand command,string namer)
        {

            level++;
            string indent = new String(' ', level * 4);


            foreach (var child in mainClass.Childs.OrderByDescending(c=>c.IsPrimitive))
            {

                string classPropertyName = child.GetPath();
                string rSide = "";


                //var mappersa = mappers.SelectMany(m => m.IFClassMappings.Where(c => c.FromProperty.Id == child.Id)).ToList();




                try
                {
                    if (child.IsPrimitive)
                    {

                        var mapper = mappers.SelectMany(m => m.IFClassMappings.Where(c => c.FromProperty.Id == child.Id)).SingleOrDefault();

                        var currentCommand = FindModelRecursive(command, mapper);

                        var path = namer + "." + currentCommand.GetModelPath();

                        // rSide = mapper.IFClassMapper.IFModel
                        builder.AppendLine($"{indent} {path}.{rSide} = {classPropertyName}.{child.Name}");
                    }
                    else
                    {

                        var mapper = mappers.SelectMany(m => m.IFClassMappings.Where(c => c.FromProperty.Id == child.Childs.First().Id)).SingleOrDefault();

                        var currentCommand = FindModelRecursive(command, mapper);

                        var path = currentCommand.GetModelPath();
                        rSide = currentCommand.Model.Name;


                        string multi = "";

                        if (currentCommand.IsMultiCommand())
                        {
                            multi = "Multi";
                        }

                        if (child.GenericType == "List")
                        {
                            builder.AppendLine($"{indent} {path}.{rSide}.DataModel.{multi} = new List<{child.Type}>()");
                        }
                        else
                        {
                            builder.AppendLine($"{indent} {path}.{rSide}.DataModel.{multi} = new {child.Type}();");
                        }



                        if (child.GenericType == "List")
                        {
                            var foreachName = child.GetPath() + "." + child.Name;

                            builder.AppendLine($" {indent} foreach (var item in {foreachName})");
                            builder.AppendLine();
                            builder.AppendLine($"{indent}{{");
                            builder.AppendLine(indent);
                        }

                        GenerateClassTree2(child, builder, level, mappers, command, namer);

                        if (child.GenericType == "List")
                        {
                            builder.AppendLine();
                            builder.AppendLine();
                            builder.AppendLine($"{indent}}}");
                        }

                    }
                }
                catch (Exception ex)
                {

                    throw;
                }


            }

        }

        public async Task<string> GenerateMapper2(int classMapperId)
        {



            var command = await this.GetQuery<IFClass>(p => p.Id == 702)
                .Include(c => c.Parent)
                .Include(s => s.Childrens)
                .SingleOrDefaultAsync();


            var process = await this.GetQuery<IFProcess>(p => p.Commands.Any(c => c.Id == 6))
                   .Include(s => s.Project.Solution)
                   .Include(s => s.Commands).ThenInclude(c => c.Parent)
                   .Include(s => s.Commands).ThenInclude(s => s.Childrens)
                   .Include(s => s.Commands).ThenInclude(s => s.Model.Properties).ThenInclude(s => s.EntityProperty)
                   .Include(s => s.Commands).ThenInclude(s => s.Model.Entity.Relations)
                   .ToListAsync();

            StringBuilder builder = new StringBuilder();

            var mapper = await this.GetClassMapper(classMapperId);
            
            int level = 0;
            //await Recursive("aaa", process.First().Commands.ToList(), builder,level);
          

            fileSystem.FormatCode(builder.ToString(), "cs", "mapper");

            return builder.ToString();
        }

        private async Task Recursive(string nameSpace, List<IFCommand> commmands, StringBuilder builder,int level)
        {
            foreach (var command in commmands)
            {

                var childs = command.Childrens.ToList();

                if (childs.Any())
                {
                    level++;
                    await Recursive(nameSpace, childs, builder,level);
                }               

                GenerateParentClass(command,builder, level);
                
            }


        }

        private void GenerateParentClass(IFCommand command,StringBuilder builder, int level)
        {
            string indent = new String(' ', level * 4);

            bool IsList = false;

            if (command.Parent != null)
            {
                IsList = command.IsList();
            }

            var path = command.GetModelPath();

            string modelPropertyName = path ;

            if (IsList)
            {
                builder.AppendLine($" {indent} foreach (var item in command.Data)");
                builder.AppendLine();
                builder.AppendLine($"{indent}{{");
                builder.AppendLine(indent);
            }

            bool isList = command.IsList();

            foreach (var property in command.Model.Properties)
            {
                string name = property.Model.Name;
                builder.AppendLine($"{indent} {modelPropertyName}.{command.Model.Name} = {property.EntityProperty.Name}");

            }


            if (IsList)
            {
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine($"{indent}}}");
            }
        }

        private async Task GenerateChildCommand(string nameSpace, IFCommand command,StringBuilder builder)
        {
            var entityTree = await entityService.GetEntityTree(command.Model.EntityId);

            //ModelGenerator modelGenerator = new ModelGenerator(fileSystem, command.Model, nameSpace, entityTree);

            //modelGenerator.Generate();

            //CqrsCommandClassGenerator commandClassGenerator = new CqrsCommandClassGenerator(command, process, entityTree, fileSystem);

            //commandClassGenerator.Generate();
        }

        public async Task GenerateClass(int classId)
        {

            var tree = await this.GetClassTreeList(classId);

            var parent = tree.First();

            List<CSClass> allClass = new List<CSClass>();


            CSClass csClass = new CSClass();

            csClass.Usings.Add("System");
            csClass.Usings.Add("System.Collections.Generic");

            GenerateClassTree(parent, csClass, allClass);


            StringBuilder code = new StringBuilder();

            foreach (var @class in allClass)
            {
                code.AppendLine(@class.GenerateCode().Template);
            }


            fileSystem.FormatCode(code.ToString(), "cs", parent.Name);
        }

        private static void GenerateClassTree(ClassControlTreeDto mainClass, CSClass csClass, List<CSClass> allClass)
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

                    GenerateClassTree(child, childClass, allClass);

                }

            }


        }

        public async Task<List<IFClassMapping>> GetClassMappings(int classMapId)
        {

            var list = await this.GetQuery<IFClassMapping>(c => c.IFClassMapperId == classMapId).ToListAsync();

            return list;
        }

        public async Task<List<IFClass>> GetClassTree(int classId)
        {
            var list = await this.GetQuery<IFClass>(t => t.Id == classId).ToListAsync();

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

        public async Task UpdateClassMapping(List<IFClassMapping> form, int classMapId)
        {
            try
            {
                var entity = await this.GetQuery<IFClassMapper>()
               .Include(e => e.IFClassMappings)
               .SingleOrDefaultAsync(k => k.Id == classMapId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                for (int i = 0; i < entity.IFClassMappings.Count; i++)
                {
                    if (!form.Any(d => d.Id == entity.IFClassMappings.ElementAt(i).Id))
                    {
                        this.Delete(entity.IFClassMappings.ElementAt(i));
                    }
                }

                foreach (var dto in form)
                {

                    if (dto.Id <= 0)
                    {
                        IFClassMapping property = new IFClassMapping();
                        //property.IsList = dto.IsList;
                        property.IFClassMapperId = classMapId;
                        property.FromPropertyId = dto.FromPropertyId;
                        property.ToPropertyId = dto.ToPropertyId;
                        entity.IFClassMappings.Add(property);
                    }
                    else
                    {
                        var property = entity.IFClassMappings.SingleOrDefault(p => p.Id == dto.Id);
                        //property.IsList = dto.IsList;
                        property.IFClassMapperId = classMapId;
                        property.FromPropertyId = dto.FromPropertyId;
                        property.ToPropertyId = dto.ToPropertyId;
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
