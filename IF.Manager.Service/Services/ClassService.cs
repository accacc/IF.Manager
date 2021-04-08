
using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Core.Data;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.CodeGen;
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
        private readonly IEntityService entityService;
        public ClassService(ManagerDbContext dbContext, IEntityService entityService) : base(dbContext)
        {
            this.entityService = entityService;
        }

        public async Task JsonToClass(string name, string json)
        {
            JsonClassGenerator jsonClassGenerator = GetJsonGenerator(name, json);

            var jsonTypes = jsonClassGenerator.Types;

            IFClass wrapperClass = new IFClass();

            wrapperClass.Name = name;
            wrapperClass.Type = name;
            wrapperClass.IsPrimitive = true;

            IFClass rootClass = new IFClass();

            wrapperClass.Childrens.Add(rootClass);

            var rootJsonObject = jsonTypes.Where(t => t.IsRoot).SingleOrDefault();

            var rootJsonObjectChildNames = rootJsonObject.Fields.Select(s => s.MemberName).ToList();

            var rootJsonObjectChilds = jsonTypes.Where(t => rootJsonObjectChildNames.Contains(t.AssignedName)).ToList();

            GenerateJsonToClass(rootClass, rootJsonObjectChilds, jsonTypes);

            await this.AddClass(wrapperClass);
        }

        private void GenerateJsonToClass(IFClass @class, List<JsonType> rootChilds, IList<JsonType> types)
        {
            foreach (var type in rootChilds)
            {
                HandleJsonType(type, type.AssignedName, @class);

                if (type.Type == JsonTypeEnum.Object)
                {
                    @class.IsPrimitive = false;

                    foreach (var field in type.Fields)
                    {
                        IFClass property = new IFClass();

                        HandleJsonType(field.Type, field.MemberName, property);

                        @class.Childrens.Add(property);

                        if (field.Type.Type == JsonTypeEnum.Object || (field.Type.InternalType != null && field.Type.InternalType.Type == JsonTypeEnum.Object))
                        {
                            GenerateJsonToClass(property, types.Where(t => t.AssignedName == field.MemberName).ToList(), types);

                        }
                    }
                }
            }
        }

        private void HandleJsonType(JsonType item, string name, IFClass @class)
        {
            @class.Name = name;
            @class.Type = item.Type.ToString();
            @class.IsPrimitive = true;
            @class.Description = name;

            if (item.Type == JsonTypeEnum.Array)
            {
                @class.GenericType = "List";

                if (item.InternalType.Type == JsonTypeEnum.Object)
                {
                    @class.Type = name;
                }
            }
            else if (item.Type == JsonTypeEnum.Object)
            {
                @class.IsPrimitive = false;
                @class.Type = name;
            }
            else if (item.Type == JsonTypeEnum.NullableSomething)
            {
                @class.Type = "string";
            }

            else if (item.Type == JsonTypeEnum.Integer)
            {
                @class.Type = "int";
            }
            else if (item.Type == JsonTypeEnum.Boolean)
            {
                @class.Type = "bool";
            }
        }

        private JsonClassGenerator GetJsonGenerator(string name, string jsondata)
        {
            var generator = new JsonClassGenerator();
            generator.Example = jsondata;
            generator.InternalVisibility = false;
            generator.CodeWriter = new CSharpCodeWriter();
            generator.ExplicitDeserialization = false;
            generator.Namespace = "IF.Json";
            generator.NoHelperClass = false;
            generator.SecondaryNamespace = null;
            generator.TargetFolder = @"C:\temp\generated\json";
            generator.UseProperties = true;
            generator.MainClass = name;
            generator.UsePascalCase = true;
            generator.UseNestedClasses = false;
            generator.ApplyObfuscationAttributes = false;
            generator.SingleFile = true;
            generator.ExamplesInDocumentation = false;
            generator.GenerateClasses();
            return generator;
        }

        public async Task<List<IFClassMapper>> GetClassMapperList()
        {
            var data = await this.GetQuery<IFClassMapper>().ToListAsync();
            return data;
        }


        public async Task AddClassMapper(IFClassMapper form)
        {
            IFClassMapper entity = new IFClassMapper();
            string name = ObjectNamerHelper.AddAsLastWord(form.Name, "IFClassMapper");
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

                string name = ObjectNamerHelper.AddAsLastWord(form.Name, "IFClassMapper");
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
                string type = ObjectNamerHelper.AddAsLastWord(form.Type, "CustomClass");
                entity.Type = type;
                entity.Name = form.Name;
                entity.GenericType = form.GenericType;
                // entity.IsPrimitive = false;
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

                string type = ObjectNamerHelper.AddAsLastWord(form.Type, "CustomClass");
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
                        //property.GenericType = dto.GenericType;
                        //property.IsPrimitive = true;
                        property.ParentId = classId;
                        property.IsNullable = dto.IsNullable;
                        property.Description = dto.Description;
                        this.Add(property);
                    }
                    else
                    {
                        var property = await this.GetQuery<IFClass>(p => p.Id == dto.Id && p.ParentId == classId).SingleOrDefaultAsync();
                        property.Name = dto.Name;
                        //property.IsPrimitive = true;
                        property.ParentId = classId;
                        property.Description = dto.Description;
                        property.Type = dto.Type;
                        // property.GenericType = dto.GenericType;
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

        public async Task<string> GenerateClassToModelMapper(IFProcess process, int classId, int commandId)
        {

            var fileSystem = new FileSystemCodeFormatProvider(DirectoryHelper.GetTempProcessDirectory(process));

            var classTree = await this.GetClassTreeList(classId);

            var parentClass = classTree.First();

            string nameSpace = SolutionHelper.GetProcessNamaspace(process);

            int level = 0;

            StringBuilder builder = new StringBuilder();

            List<IFCommand> commands = await this.GetQuery<IFCommand>()
                .Include(c => c.Childrens).ThenInclude(p => p.Parent)
                .Include(c => c.Parent)
                .ToListAsync();


            List<IFCommand> commands2 = await this.GetQuery<IFCommand>()
                .Include(s => s.IFClassMapper.IFClassMappings).ThenInclude(m => m.FromProperty)
                .Include(s => s.IFClassMapper.IFClassMappings).ThenInclude(m => m.ToProperty.EntityProperty)
                .Include(s => s.Model.Properties).ThenInclude(s => s.EntityProperty)
                .Include(s => s.Model.Entity.Relations)
                .ToListAsync();

            var command = commands.Where(c => c.Id == commandId).SingleOrDefault();

            command.Childrens = await GetCommandChildrensByParentId(commandId);
            command.Childrens.OrderBy(c => c.Sequence);

            CSClass cSClass = new CSClass();
            cSClass.Name = command.IFClassMapper.Name;
            cSClass.Usings.Add(nameSpace);
            cSClass.Usings.Add("System.Collections.Generic");

            string name = "";

            if (command != null)
            {

                name = command.Model.Name + "Multi";

                builder.AppendLine($"{name} {name} = new {name}();");
            }



            GenerateMapperMethodBody(parentClass, builder, level, command);

            builder.AppendLine($"return {name};");

            var mapperMethod = new CSMethod(command.IFClassMapper.Name + "Map", command.Model.Name + "Multi", "public");

            mapperMethod.Body = builder.ToString();
            mapperMethod.Parameters.Add(new CsMethodParameter() { Name = parentClass.Name, Type = parentClass.Type });

            cSClass.Methods.Add(mapperMethod);


            fileSystem.FormatCode(cSClass.GenerateCode(), "cs");

            return builder.ToString();
        }

        private IFCommand FindCommandByClassMapping(IFCommand commands, ClassControlTreeDto classControlTree)
        {

            var allCommands = commands.FlattenHierarchy(x => x.Childrens).ToList();

            IFCommand currentCommand = null;

            foreach (var command in allCommands)
            {
                if (command.IsMultiCommand()) continue;

                foreach (var classMapping in command.IFClassMapper.IFClassMappings)
                {
                    if (classControlTree.IsPrimitive)
                    {
                        if (classMapping.FromPropertyId == classControlTree.Id)
                        {
                            currentCommand = command;
                        }
                    }
                    else
                    {
                        if (classControlTree.Childs.Any(c => c.Id == classMapping.FromPropertyId))
                        {
                            currentCommand = command;
                        }
                    }
                }
            }

            //if (command == null)
            //{
            //    throw new BusinessException("Command da mapper bulunamadı:1122");
            //}

            return currentCommand;
        }

        private void GenerateMapperMethodBody(ClassControlTreeDto mainClass, StringBuilder builder, int level, IFCommand command)
        {

            level++;

            string indent = new String(' ', level * 4);

            foreach (var child in mainClass.Childs.OrderByDescending(c => c.IsPrimitive))
            {


                string modelName = "";
                string modelNameWithParent = "";
                string multiName = "Multi";
                string modelType = "";
                string parentModelName = "";

                try
                {
                    if (child.IsPrimitive)
                    {

                        string classPropertyName = child.GetPath();

                        var currentCommand = FindCommandByClassMapping(command, child);

                        if (currentCommand == null) continue;

                        var mapperList = currentCommand.IFClassMapper.IFClassMappings.Where(m => m.FromProperty.Id == child.Id).ToList();


                        if (!mapperList.Any())
                        {

                            throw new BusinessException($"Command:{currentCommand.Name} Mapper:{currentCommand.IFClassMapper.Name} Property:{child.Name} PropertyMapping Is Not Found : ErroCode(C1534)");
                        }

                        var mapperCount = mapperList.Count();

                        if (mapperCount > 1)
                        {
                            throw new BusinessException($"Command:{currentCommand.Name} Mapper:{currentCommand.IFClassMapper.Name} Property:{child.Name} Too Many PropertyMapping : ErroCode(C1533)");
                        }

                        var mapper = mapperList.Where(m => m.FromProperty.Id == child.Id).SingleOrDefault();

                        var path = currentCommand.GetModelPath();

                        modelName = currentCommand.Model.Name;
                        modelType = currentCommand.Model.Name;

                        if (currentCommand.Parent != null && currentCommand.Parent.Model != null)
                        {
                            parentModelName = currentCommand.Parent.Model.Name;
                            modelNameWithParent = $"{parentModelName}_{modelName}";
                        }

                        if (currentCommand.IsMultiCommand())
                        {
                            modelName = modelName + multiName;
                        }

                        if (child.Parent.GenericType == "List")
                        {
                            builder.AppendLine($"{indent} {modelName}{level - 1}.{mapper.ToProperty.EntityProperty.Name} = item{level - 1}.{mapper.FromProperty.Name};");
                        }
                        else
                        {
                            builder.AppendLine($"{indent} {modelName}.{mapper.ToProperty.EntityProperty.Name} = {classPropertyName}.{mapper.FromProperty.Name};");
                        }
                    }
                    else
                    {


                        builder.AppendLine();
                        builder.AppendLine();

                        if (child.Type == "BorcluTelefon")
                        {

                        }

                        var currentCommand = FindCommandByClassMapping(command, child);

                        if (currentCommand == null)
                        {

                            continue;
                        }

                        string path = "";

                        if (currentCommand.Parent == null)
                        {
                            path = currentCommand.GetModelPath();
                        }
                        else
                        {
                            path += currentCommand.GetModelPath();
                        }

                        modelName = currentCommand.Model.Name;
                        modelType = currentCommand.Model.Name;

                        if (currentCommand.Parent != null && currentCommand.Parent.Model != null)
                        {
                            parentModelName = currentCommand.Parent.Model.Name;
                            modelNameWithParent = $"{parentModelName}_{modelName}";
                        }


                        if (child.Name == "Borclular")
                        {
                        }


                        builder.AppendLine();
                        builder.AppendLine();

                        bool IsMultiList = false;

                        if (currentCommand.Parent.IsMultiCommand() && currentCommand.Parent.Parent != null)
                        {

                            if (currentCommand.Parent.Childrens.First().Id == currentCommand.Id)
                            {
                                if (currentCommand.Parent.IsList)
                                {
                                    builder.AppendLine($"{indent} var {parentModelName}{multiName}s = new List<{parentModelName}{multiName}>();");
                                    IsMultiList = true;
                                }
                                else
                                {
                                    builder.AppendLine($"{indent} {parentModelName}{multiName} {parentModelName}{multiName} = new {parentModelName}{multiName}();");
                                }

                                if (currentCommand.Parent.Parent != null)
                                {
                                    string lastName = $"{multiName}";

                                    if (currentCommand.Parent.IsList) lastName = $"{multiName}s";

                                    builder.AppendLine($"{currentCommand.Parent.Parent.Model.Name}{multiName}.{parentModelName}{multiName} = {parentModelName}{lastName};");
                                }
                            }

                        }


                        builder.AppendLine();
                        builder.AppendLine();

                        if (!IsMultiList)
                        {

                            if (child.GenericType == "List")
                            {
                                builder.AppendLine($"{indent} List<{modelName}> {modelName} = new List<{modelName}>();");
                            }
                            else
                            {
                                builder.AppendLine($"{indent} {modelName} {modelName} = new {modelName}();");
                            }

                            builder.AppendLine($"{indent} {parentModelName}{multiName}.{modelName} = {modelName};");

                        }

                        builder.AppendLine();
                        builder.AppendLine();


                        if (child.GenericType == "List")
                        {
                            bool IsFirstLoop = true;

                            var parents = child.GetParentPath();

                            foreach (var parent in parents)
                            {
                                if (parent.GenericType == "List")
                                {
                                    IsFirstLoop = false;
                                }
                            }

                            string foreachName = "";

                            if (!IsFirstLoop)
                            {
                                foreachName = "item" + (level - 1) + "." + child.Name;
                            }
                            else
                            {
                                foreachName = child.GetPath() + "." + child.Name;
                            }

                            builder.AppendLine();
                            builder.AppendLine();
                            builder.AppendLine($" {indent} foreach (var item{level} in {foreachName})");
                            builder.AppendLine($"{indent}{{");
                            builder.AppendLine();
                            builder.AppendLine();


                            if (IsMultiList)
                            {
                                builder.AppendLine($"{indent}  {parentModelName}{multiName} {parentModelName}{multiName} = new {parentModelName}{multiName}();");
                            }


                            builder.AppendLine();
                            builder.AppendLine();

                            builder.AppendLine($"{indent} {modelName} {modelName}{level}= new {modelName}();");

                            if (IsMultiList && child.GenericType == "List")
                            {
                                builder.AppendLine($"{indent} {parentModelName}{multiName}.{modelName}= {modelName}{level};");
                            }

                            builder.AppendLine(indent);
                            builder.AppendLine();
                            builder.AppendLine();
                        }

                        GenerateMapperMethodBody(child, builder, level, command);

                        if (child.GenericType == "List")
                        {
                            builder.AppendLine();
                            builder.AppendLine();


                            if (currentCommand.Parent.IsMultiCommand() && currentCommand.Parent.Parent != null && currentCommand.Parent.Childrens.First().Id == currentCommand.Id)
                            {

                                builder.AppendLine($"{indent} {parentModelName}{multiName}s.Add({parentModelName}{multiName});");
                            }
                            else
                            {

                                builder.AppendLine($"{indent} {modelName}.Add({modelName}{level});");
                            }


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
               .Include(c => c.Childrens).ThenInclude(c => c.Parent)
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

            var items = await this.GetQuery<IFClass>(x => x.ParentId == parentId).ToListAsync();

            foreach (var item in items)
            {
                item.Childrens = await GetChildrenByParentId(item.Id);
                children.Add(item);
            }

            return children;
        }

        public async Task<List<IFClass>> GetClassFlattenList(int classId)
        {
            var list = await GetClassTree(classId);

            var flat = MakeClassFlatten(list.First());

            return flat.ToList();

        }
        private IEnumerable<IFClass> MakeClassFlatten(IFClass @class)
        {
            yield return @class;

            foreach (var node in @class.Childrens.SelectMany(child => MakeClassFlatten(child)))
            {
                yield return node;
            }
        }

        public async Task DeleteClass(int id)
        {
            var list = await GetClassFlattenList(id);

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
                        property.IFClassMapperId = classMapId;
                        property.FromPropertyId = dto.FromPropertyId;
                        property.ToPropertyId = dto.ToPropertyId;
                        entity.IFClassMappings.Add(property);
                    }
                    else
                    {
                        var property = entity.IFClassMappings.SingleOrDefault(p => p.Id == dto.Id);
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
