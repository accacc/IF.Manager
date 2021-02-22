
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
                .Include(m => m.IFClassMappings)
                .Include(m => m.IFClass.Parent).ThenInclude(c => c.Childrens)
                .Include(m => m.IFModel.Properties)
                .Include(m => m.IFModel.Entity.Properties)
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
                .OrderBy(c => c.Name)
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
                var list = await this.GetQuery<IFClass>(c => c.Id == 702

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

        public async Task<string> GenerateMapper(IFProcess process,int classMapperId)
        {

            var fileSystem = new FileSystemCodeFormatProvider(DirectoryHelper.GetTempProcessDirectory(process));

            var tree = await this.GetClassTreeList(702);

            var parent = tree.First();

            string nameSpace = SolutionHelper.GetProcessNamaspace(process);

            CSClass cSClass = new CSClass();
            cSClass.Name = "Mapper";
            cSClass.Usings.Add(nameSpace);
            cSClass.Usings.Add("System.Collections.Generic");

            int level = 0;

            StringBuilder builder = new StringBuilder();

            List<IFCommand> commands = await this.GetQuery<IFCommand>()
                .Include(s => s.IFClassMapper.IFClassMappings).ThenInclude(m => m.FromProperty)
                .Include(s => s.IFClassMapper.IFClassMappings).ThenInclude(m => m.ToProperty.EntityProperty)
               .Include(c => c.Childrens).ThenInclude(p=>p.Parent)
                .Include(c => c.Parent)
                .Include(s => s.Model.Properties).ThenInclude(s => s.EntityProperty)
                .Include(s => s.Model.Entity.Relations)
                                                        .ToListAsync();

          var command = commands.Where(c => c.Id == 6).SingleOrDefault();

            command.Childrens = await GetCommandChildrensByParentId(6);

            string name = "";

            if (command != null)
            {

                name = command.Model.Name +"Multi";

                builder.AppendLine($"{name} {name} = new {name}();");
            }



            GenerateClassTree2(parent, builder, level, command, name);

            var me = new CSMethod("MapMe", "void", "public");
            me.Body = builder.ToString();
            me.Parameters.Add(new CsMethodParameter() { Name = parent.Name, Type = parent.Type });
            cSClass.Methods.Add(me);


            fileSystem.FormatCode(cSClass.GenerateCode(), "cs");

            return builder.ToString();
        }

        private IFCommand FindModelRecursive(IFCommand commands, ClassControlTreeDto classControlTree)
        {

            var list = commands.FlattenHierarchy(x => x.Childrens);
            IFCommand command = null;


            foreach (var item in list)
            {
                if (item.IsMultiCommand()) continue;

                foreach (var item2 in item.IFClassMapper.IFClassMappings)
                {
                    if(item2.FromPropertyId == classControlTree.Id)
                    {
                        command = item;
                    }
                }
            }

            if (command == null)
            {
                throw new BusinessException("Command da mapper bulunamadı:1122");
            }

            return command;
        }

        private void GenerateClassTree2(ClassControlTreeDto mainClass, StringBuilder builder, int level, IFCommand command, string namer)
        {

            level++;

            string indent = new String(' ', level * 4);


            foreach (var child in mainClass.Childs.OrderByDescending(c => c.IsPrimitive))
            {

              
                string modelName = "";
                string multiName = "";

                try
                {
                    if (child.IsPrimitive)
                    {

                        string classPropertyName = child.GetPath();

                        var currentCommand = FindModelRecursive(command,  child);

                        var mapper = currentCommand.IFClassMapper.IFClassMappings.Where(m=>m.FromProperty.Id == child.Id).SingleOrDefault();

                        var path = currentCommand.GetModelPath();

                        modelName = currentCommand.Model.Name;

                       //if(child.Parent.GenericType == "List")
                       // {
                       //     modelName += "item" + level;
                       // }

                        if (currentCommand.IsMultiCommand())
                        {
                            multiName = "Multi";
                        }

                        //if (child.GenericType == "List")
                        //{
                        //    builder.AppendLine($"{indent} {path}.{rSide}{multi} = new List<{rSide}{multi}>();");
                        //}
                        //else

                        if (child.Parent.GenericType == "List")
                        {
                            builder.AppendLine($"{indent} {modelName}{multiName}{level-1}.{mapper.ToProperty.EntityProperty.Name} = item{level - 1}.{mapper.FromProperty.Name};");
                        }
                        else
                        {
                            builder.AppendLine($"{indent} {modelName}{multiName}.{mapper.ToProperty.EntityProperty.Name} = {classPropertyName}.{mapper.FromProperty.Name};");
                        }

                      


                    }
                    else
                    {


                        builder.AppendLine();
                        builder.AppendLine();


                        var currentCommand = FindModelRecursive(command, child.Childs.First());

                        //currentCommand = currentCommand.Parent;

                        string path = "";

                        if(currentCommand.Parent == null)
                        {
                            path = currentCommand.GetModelPath();
                        }
                        else
                        {
                            path += currentCommand.GetModelPath();
                        }

                        //path = namer + "." + path;

                        modelName = currentCommand.Model.Name;

                        if (currentCommand.IsMultiCommand())
                        {
                            multiName = "Multi";
                        }

                        if(modelName== "AlacakliIcraDataModel")
                        {
                            continue;
                        }


                        builder.AppendLine();
                        builder.AppendLine();

                        bool IsMultiList = false;

                        if (currentCommand.Parent.IsMultiCommand() && currentCommand.Parent.Parent !=null)
                        {

                            if (currentCommand.Parent.Childrens.First().Id == currentCommand.Id)
                            {
                                if (currentCommand.Parent.IsList())
                                {
                                    builder.AppendLine($"{indent} {currentCommand.Parent.GetModelPath()}.{currentCommand.Parent.Model.Name}Multi = new List<{currentCommand.Parent.Model.Name}Multi>();");
                                    IsMultiList = true;
                                }
                                else
                                {
                                    builder.AppendLine($"{indent} {currentCommand.Parent.GetModelPath()}.{currentCommand.Parent.Model.Name}Multi = new {currentCommand.Parent.Model.Name}Multi();");
                                }
                            }

                        }


                        builder.AppendLine();
                        builder.AppendLine();

                        if (!IsMultiList)
                        {

                            if (child.GenericType == "List")
                            {

                                builder.AppendLine($"{indent} List<{modelName}{multiName}> {modelName}{multiName} = new List<{modelName}{multiName}>();");
                            }
                            else
                            {
                                builder.AppendLine($"{indent} {modelName}{multiName} {modelName}{multiName} = new {modelName}{multiName}();");
                            }

                        }


                        builder.AppendLine();
                        builder.AppendLine();




                        if (child.GenericType == "List")
                        {
                            var foreachName = child.GetPath() + "." + child.Name;
                            builder.AppendLine();
                            builder.AppendLine();
                            builder.AppendLine($" {indent} foreach (var item{level} in {foreachName})");
                            builder.AppendLine($"{indent}{{");
                            builder.AppendLine();
                            builder.AppendLine();


                            if(IsMultiList)
                            {
                                builder.AppendLine($"{indent}  {currentCommand.Parent.Model.Name}Multi {currentCommand.Parent.Model.Name}Multi = new {currentCommand.Parent.Model.Name}Multi();");
                            }


                            builder.AppendLine();
                            builder.AppendLine();

                            builder.AppendLine($"{indent} {modelName}{multiName} {modelName}{multiName}{level}= new {modelName}{multiName}();");
                            builder.AppendLine(indent);
                            builder.AppendLine();
                            builder.AppendLine();
                        }

                        GenerateClassTree2(child, builder, level, command, namer);

                        if (child.GenericType == "List")
                        {
                            builder.AppendLine();
                            builder.AppendLine();
                            builder.AppendLine($"{indent} {path}.{modelName}{multiName}.Add({modelName}{multiName}{level});");



                            builder.AppendLine($"{indent}}}");
                            builder.AppendLine();
                            builder.AppendLine();
                        }

                    }
                }
                catch (Exception ex)
                {

                    throw;
                }


            }

        }
       
        public async Task GenerateClass(IFProcess process, int classId)
        {

            var fileSystem = new FileSystemCodeFormatProvider(DirectoryHelper.GetTempProcessDirectory(process));

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

        private async Task<List<IFCommand>> GetCommandChildrensByParentId(int parentId)
        {
            var children = new List<IFCommand>();

            var threads = await this.GetQuery<IFCommand>(x => x.ParentId == parentId)
                 .Include(s => s.IFClassMapper.IFClassMappings).ThenInclude(m => m.FromProperty)
                .Include(s => s.IFClassMapper.IFClassMappings).ThenInclude(m => m.ToProperty.EntityProperty)
               .Include(c => c.Childrens).ThenInclude(c=>c.Parent)
                .Include(c => c.Parent.Model)
                .Include(s => s.Model.Properties).ThenInclude(s => s.EntityProperty)
                .Include(s => s.Model.Entity.Relations).ToListAsync();

            foreach (var t in threads)
            {
                t.Childrens = await GetCommandChildrensByParentId(t.Id);


                children.Add(t);
            }

            return children;
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
