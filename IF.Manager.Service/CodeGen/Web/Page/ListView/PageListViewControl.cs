using IF.CodeGeneration.Core;
using IF.CodeGeneration.Language.CSharp;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service.Web.Page.ListView
{
    public class PageListViewControl : PageControlBase

    {

        IFPageListView listView;
        FileSystemCodeFormatProvider fileSystem;


        public PageListViewControl(IFPageControlMap map) : base(map)
        {
            this.listView = this.PageControlMap.ToControl<IFPageListView>();
            //string Path = DirectoryHelper.GetPagePath("", this.pagePath);
            string Path = this.pagePath;
            string generatedBasePath = DirectoryHelper.GetTempPageDirectory(Path, this.page.IFProject.Solution.SolutionName, this.page.IFProject.Name, this.page.IFProject.ProjectType);
            this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
        }



        public override void Generate()
        {
            
            RenderPartialView();

            if (!(this.listView.IFPageControlMap.Parent.IFPageControl is IFPage))
            {
                RenderClass();
                RenderView();
            }
        }

        private void RenderClass()
        {
            this.Name = $"{this.listView.Name}Model";
            this.BaseClass = "PageModel";
            this.NameSpace = base.GetPageNamespace();

            base.AddPageNameSpaces();

            base.AddApiClientProperty();

            foreach (var parameter in listView.IFPageParameters)
            {
                // [BindProperty(SupportsGet = true), Required]
                //public int CommandId { get; set; }

                var pageParameterProperty = new CSProperty("public", $"{parameter.Name}", false);
                pageParameterProperty.PropertyTypeString = parameter.Type;
                pageParameterProperty.Attirubites.Add("BindProperty(SupportsGet = true)");
                pageParameterProperty.Attirubites.Add("Required");
                this.Properties.Add(pageParameterProperty);
            }

            AddChildControlModelProperties();

            AddConstructor();

            SetModelMethod();

            AddPartialViewMethod();

            AddOnGetModelMethod();


            fileSystem.FormatCode(this.GenerateCode().Template, "cshtml.cs", this.listView.Name,"");
        }

        private void AddPartialViewMethod()
        {
            CSMethod PartialViewMethod = new CSMethod($"OnGet{this.listView.IFQuery.Name}", "PartialViewResult", "public");
            PartialViewMethod.IsAsync = true;

            StringBuilder partialMethodBody = new StringBuilder();

            partialMethodBody.AppendLine($@"await SetModel();");

            partialMethodBody.AppendLine($"return new PartialViewResult");
            partialMethodBody.AppendLine($"{{");
            partialMethodBody.AppendLine($@"ViewName = ""_{this.listView.Name}"",");
            partialMethodBody.AppendLine($@"ViewData = new ViewDataDictionary<{this.listView.IFQuery.Model.Name}>(ViewData, {this.listView.IFQuery.Model.Name})");
            partialMethodBody.AppendLine($"}};");

            PartialViewMethod.Body = partialMethodBody.ToString();

            this.Methods.Add(PartialViewMethod);
        }

        private void AddChildControlModelProperties()
        {
            string modelName = "";
            string propertyType = "";


            IFPageListView control = this.listView;
            modelName = control.IFQuery.Model.Name;

            switch (control.IFQuery.QueryGetType)
            {
                case Contracts.Enum.QueryGetType.Single:
                    propertyType = $"{modelName}";
                    break;
                case Contracts.Enum.QueryGetType.List:
                    propertyType = $"List<{modelName}>";
                    break;
                default:
                    break;
            }

            var listProprety = new CSProperty("public", $"{modelName}", false);
            listProprety.PropertyTypeString = propertyType;
            this.Properties.Add(listProprety);

        }


        private void SetModelMethod()
        {
            CSMethod setModelMethod = new CSMethod("SetModel", "void", "private");
            setModelMethod.IsAsync = true;

            StringBuilder setModelMethodBody = new StringBuilder();


            string modelName = "";
            string queryName = "";


            IFPageListView control = this.listView;
            modelName = control.IFQuery.Model.Name;
            queryName = control.IFQuery.Name;




            var requestParameter = "";

            //Data = new LawCommonInfoFilter { LawId = this.LawId }

            foreach (var parameter in listView.IFPageParameters)
            {

                requestParameter = $"{parameter.Name} = this.{parameter.Name}";
            }

            if (requestParameter != "")
            {
                requestParameter = $"{{ Data = new {queryName}Filter {{ {requestParameter}}}}}";

            }
            else
            {
                requestParameter = "()";
            }

            setModelMethodBody.AppendLine($@"var {modelName}Response = await this.apiClient.GetAsync<{queryName}Response>(""{this.topPage.Process.Name}Controller/{queryName}"",new {queryName}Request {requestParameter});");
            setModelMethodBody.AppendLine($"this.{modelName} = {modelName}Response.Data;");
            setModelMethod.Body = setModelMethodBody.ToString();
            this.Methods.Add(setModelMethod);
        }

        private void AddOnGetModelMethod()
        {
            CSMethod setModelMethod = new CSMethod("OnGet", "void", "public");

            setModelMethod.IsAsync = true;

            StringBuilder setModelMethodBody = new StringBuilder();

            setModelMethodBody.AppendLine($@"await SetModel();");

            setModelMethod.Body = setModelMethodBody.ToString();

            this.Methods.Add(setModelMethod);
        }


        private void RenderView()
        {
            StringBuilder builder = new StringBuilder();

            string nameSpace = base.GetPageNamespace();
            string layout = "";
            builder.AppendLine("@page");
            builder.AppendLine("@{");
            builder.AppendLine($"Layout = \"{layout}\";");
            builder.AppendLine("}");
            builder.AppendLine($"@model {nameSpace}.{this.listView.Name}Model");





            builder.AppendLine($@"@{{await Html.RenderPartialAsync(""{listView.Name}"", Model.{listView.IFQuery.Model.Name});}}");
            

                

            fileSystem.FormatCode(builder.ToString(), "cshtml","", $"{this.listView.Name}");
        }

            private void RenderPartialView()
        {
            StringBuilder builder = new StringBuilder();

            string nameSpace = SolutionHelper.GetProcessNamaspace(page.Process);
            string layout = "";

            if (this.listView.IFPageControlMap.Parent.IFPageControl is IFPage)
            {
                builder.AppendLine("@page");
            }
            builder.AppendLine("@{");
            builder.AppendLine($"Layout = \"{layout}\";");
            builder.AppendLine("}");

            switch (this.listView.IFQuery.QueryGetType)
            {
                case Contracts.Enum.QueryGetType.Single:
                    builder.AppendLine($"@model {nameSpace}.{this.listView.IFQuery.Model.Name}");
                    break;
                case Contracts.Enum.QueryGetType.List:
                    builder.AppendLine($"@model List<{nameSpace}.{this.listView.IFQuery.Model.Name}>");
                    break;
                default:
                    throw new BusinessException($"{this.listView.IFQuery.QueryGetType.GetType()} unknow query get type");
            }


            builder.AppendLine();

            builder.AppendDivRowStart();
            builder.AppendDivColumnMd6();
            builder.AppendLine($"<h4>{listView.Description}</h4>");
            builder.AppendDivEnd();
            builder.AppendDivEnd();



            foreach (var item in listView.IFQuery.Model.Properties)
            {

                builder.AppendLine("<div class=\"row\">");
                builder.AppendLine("<div class=\"col-md-6\">");
                builder.AppendLine("<div class=\"form-group\">");

                if (listView.IFQuery.Model.EntityId != item.EntityId)
                {
                    builder.AppendLine($"{item.EntityProperty.Name}");
                    builder.AppendLine(":");
                    builder.AppendLine($"@Html.DisplayFor(m => m.{item.Entity.Name}.{item.EntityProperty.Name})");
                }
                else
                {
                    builder.AppendLine($"{item.EntityProperty.Name}");
                    builder.AppendLine(":");
                    builder.AppendLine($"@Html.DisplayFor(m => m.{item.EntityProperty.Name})");
                }




                builder.AppendLine($"</div>");
                builder.AppendLine($"</div>");
                builder.AppendLine($"</div>");
            }

            if (!(this.listView.IFPageControlMap.Parent.IFPageControl is IFPage))
            {

                fileSystem.FormatCode(builder.ToString(), "cshtml","", $"_{this.listView.Name}");
            }
            else
            {
                fileSystem.FormatCode(builder.ToString(), "cshtml", "", $"{this.listView.Name}");
            }
        }
    }
}
