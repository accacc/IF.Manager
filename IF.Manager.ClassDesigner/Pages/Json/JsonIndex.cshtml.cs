using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.Model;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;

namespace IF.Manager.ClassDesigner.Pages.Json
{

    

    public class JsonIndexModel : PageModel
    {
        private IHostingEnvironment _environment;
        private readonly IClassService classService;
        public JsonIndexModel(IHostingEnvironment environment, IClassService classService)
        {
            _environment = environment;
            this.classService = classService;
        }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        [BindProperty]
        public string  jsondata { get; set; }
        public async Task OnPostAsync()
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
            gen.MainClass ="Example";
            gen.UsePascalCase = true;
            gen.UseNestedClasses = false;
            gen.ApplyObfuscationAttributes = false;
            gen.SingleFile = true;
            gen.ExamplesInDocumentation = false;

            gen.GenerateClasses();

            var types = gen.Types;

            IFKClass mainClass = new IFKClass();
            mainClass.Name = "aaa";
            mainClass.Type = "aaa";
            mainClass.IsPrimitive = true;

           

            List<IFKClass> list = new List<IFKClass>();

            mainClass.Childrens = list;

            foreach (var item in types)
            {
                IFKClass cls = new IFKClass();

                if (item.Type == JsonTypeEnum.Object)
                {
                    cls.IsPrimitive = true;
                    

                    foreach (var field in item.Fields)
                    {
                        IFKClass prop = new IFKClass();
                        prop.Name = field.MemberName;
                        prop.Type = field.Type.Type.ToString();
                        prop.IsPrimitive = false;
                        prop.Description = field.MemberName;

                        if(field.Type.Type == JsonTypeEnum.Array)
                        {

                            prop.GenericType = "List";
                        }

                        if(field.Type.Type == JsonTypeEnum.Object)
                        {
                            prop.Parent = cls;
                        }

                        if(field.Type.Type == JsonTypeEnum.NullableSomething)
                        {
                            prop.Type = "string";
                        }

                        cls.Childrens.Add(prop);

                    }
                }



                cls.Name = item.AssignedName;
                cls.Type = item.AssignedName;

                if (item.Type == JsonTypeEnum.Array)
                {

                    cls.GenericType = "List";
                }


                if (item.Type == JsonTypeEnum.NullableSomething)
                {
                    cls.Type = "string";
                }

                if (item.Type == JsonTypeEnum.Array)
                {

                    cls.GenericType = "List";
                }

                list.Add(cls);


            }


            await this.classService.AddClass(mainClass);
        }


        public bool IsChild(IList<JsonType> types,string name)
        {



            return false;
        }
    }
}
