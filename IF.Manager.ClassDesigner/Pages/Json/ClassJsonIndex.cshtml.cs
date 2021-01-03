using IF.CodeGeneration.Core;
using IF.CodeGeneration.CSharp;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using IF.Manager.Service;
using IF.Manager.Service.Model;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;

namespace IF.Manager.ClassDesigner.Pages.Json
{
    public class ClassIndexModel : PageModel
    {
        private IHostingEnvironment _environment;
        private readonly IClassService classService;
        private readonly FileSystemCodeFormatProvider fileSystem;
        public ClassIndexModel(IHostingEnvironment environment, IClassService classService)
        {
            _environment = environment;
            this.classService = classService;
            this.fileSystem = new FileSystemCodeFormatProvider(DirectoryHelper.GetTempGeneratedDirectoryName());
        }

        [BindProperty]
        public string  jsondata { get; set; }

        [BindProperty]
        public string name { get; set; }

         public async Task OnPostAsync()
        //public void OnPostAsync()
        {
            JsonClassGenerator gen = GetTypes();

            var types = gen.Types;

            IFClass mainClass = new IFClass();
            mainClass.Name = name;
            mainClass.Type = name;
            mainClass.IsPrimitive = true;

            List<IFClass> list = new List<IFClass>();

            mainClass.Childrens = list;
            var rootObject = types.Where(t => t.IsRoot).SingleOrDefault();
            var rchilds = rootObject.Fields.Select(s => s.MemberName).ToList();
            var rootChilds = types.Where(t => rchilds.Contains(t.AssignedName)).ToList();
            GenerateClasses(list, rootChilds,types);

            //await this.classService.AddClass(mainClass);
        }

        private void GenerateClasses(List<IFClass> list, List<JsonType> rootChilds,IList<JsonType> types)
        {
            foreach (var type in rootChilds)
            {
                IFClass @class = new IFClass();

                HandleType(type, type.AssignedName, @class);

                if (type.Type == JsonTypeEnum.Object)
                {
                    @class.IsPrimitive = false;

                    foreach (var field in type.Fields)
                    {
                        IFClass property = new IFClass();

                        HandleType(field.Type, field.MemberName, property);

                        @class.Childrens.Add(property);

                        if(field.Type.Type == JsonTypeEnum.Object ||(field.Type.InternalType!=null && field.Type.InternalType.Type == JsonTypeEnum.Object ))
                        {
                            GenerateClasses(property.Childrens.ToList(),types.Where(t=>t.AssignedName == field.MemberName).ToList(),types);

                        }

                    }

                    
                }

                list.Add(@class);


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




        private JsonClassGenerator GetTypes()
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

        public bool IsChild(IList<JsonType> types,string name)
        {



            return false;
        }
    }
}
