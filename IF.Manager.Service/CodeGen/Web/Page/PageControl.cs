using IF.CodeGeneration.CSharp;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IF.Manager.Service.Web.Page
{
    public class PageControl : PageControlBase
    {
        public PageControl(IFPageControlMap control):base(control)
        {                      
        }

        public override void Generate()
        {
            this.GeneratePageClass();
            this.GeneratePageView();
        }


        public void GeneratePageView()
        {
            var columnCount = this.page.PageLayout.ColumSize;

            string pageNameSpace = SolutionHelper.GetPageNamespace(topPage, this.pageNameSpace);

            StringBuilder builder = new StringBuilder();

            GeneratePartialViews(columnCount, pageNameSpace, builder);

            this.fileSystem.FormatCode(builder.ToString(), "cshtml", $"{page.Name}");
        }

        //TODO:base classa al 112
        private void GeneratePartialViews(int columnCount, string pageNameSpace, StringBuilder builder)
        {
            builder.AppendLine("@page");
            builder.AppendLine($"@model {pageNameSpace}.{this.page.Name}Model");

            builder.AppendDivStart("container");
            builder.AppendDivStart("row");

            var modelName = "";
            var name = "";

            for (int i = 0; i < this.PageControlMap.Childrens.Count; i++)
            {

                var child = this.PageControlMap.Childrens.ElementAt(i);


                if (child.IFPageControl is IFPageGrid)
                {
                    IFPageGrid grid = child.ToControl<IFPageGrid>();
                    modelName = grid.Query.Model.Name;
                    name = grid.Name;
                }
                else if (child.IFPageControl is IFPageListView)
                {
                    IFPageListView listView = child.ToControl<IFPageListView>();
                    modelName = listView.IFQuery.Model.Name;
                    name = listView.Name;
                }
                else if (child.IFPageControl is IFPagePanel)
                {
                    IFPagePanel panel = child.ToControl<IFPagePanel>();
                    name = panel.Name;
                }
                else
                {
                    throw new BusinessException($"{child.GetType()} unknow page control type");
                }

                if (child.IFPageControl is IFPagePanel)
                {
                    builder.AppendDivStart("col", $@"{name}Div");
                    builder.AppendLine($@"@{{await Html.RenderPartialAsync(""_{name}"");}}");
                    builder.AppendDivEnd();

                }
                else
                {

                    builder.AppendDivStart("col", $@"{name}Div");
                    builder.AppendLine($@"@{{await Html.RenderPartialAsync(""_{name}"", Model.{modelName});}}");
                    builder.AppendDivEnd();

                }


                if ((i + 1) % columnCount == 0)
                {
                    builder.AppendDivStart("w-100");
                    builder.AppendDivEnd();
                }

            }

            builder.AppendDivEnd();
            builder.AppendDivEnd();
        }

        public void GeneratePageClass()
        {

            this.Name = $"{this.page.Name}Model";

            base.AddPageNameSpaces();

            base.AddApiClientProperty();

            foreach (var parameter in page.IFPageParameters)
            {
                // [BindProperty(SupportsGet = true), Required]
                //public int CommandId { get; set; }

                var pageParameterProperty = new CSProperty("public", $"{parameter.Name}", false);
                pageParameterProperty.PropertyTypeString = parameter.Type;
                pageParameterProperty.Attirubites.Add("BindProperty(SupportsGet = true)");
                pageParameterProperty.Attirubites.Add("Required");
                this.Properties.Add(pageParameterProperty);
            }

            AddChildControlModelProperties(this.PageControlMap.Childrens);

            AddConstructor();

            SetModelMethod();

            //AddPartialViewMethod();

            AddGetModelMethod();


            fileSystem.FormatCode(this.GenerateCode().Template, "cshtml.cs", this.PageControlMap.IFPageControl.Name);

        }

        private void AddChildControlModelProperties(ICollection<IFPageControlMap> childrens)
        {
            string modelName = "";
            string propertyType = "";

            foreach (var child in childrens)
            {
                if (child.IFPageControl is IFPageGrid)
                {
                    IFPageGrid control = child.ToControl<IFPageGrid>();
                    modelName = control.Query.Model.Name;

                    switch (control.Query.QueryGetType)
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
                }
                else if (child.IFPageControl is IFPageListView)
                {
                    IFPageListView control = child.ToControl<IFPageListView>();
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
                }
                else if (child.IFPageControl is IFPagePanel)
                {
                    IFPagePanel panel = child.ToControl<IFPagePanel>();
                    this.AddChildControlModelProperties(panel.IFPageControlMap.Childrens);
                    continue;
                }
                else
                {
                    throw new BusinessException($"{child.GetType()} unknow page control type");
                }


                //if (!(child.IFPageControl is IFPagePanel))
                {
                    var listProprety = new CSProperty("public", $"{modelName}", false);
                    listProprety.PropertyTypeString = propertyType;
                    this.Properties.Add(listProprety);
                }
            }
        }

        private void AddGetModelMethod()
        {
            CSMethod setModelMethod = new CSMethod("OnGet", "void", "public");

            setModelMethod.IsAsync = true;

            StringBuilder setModelMethodBody = new StringBuilder();

            setModelMethodBody.AppendLine($@"await SetModel();");

            setModelMethod.Body = setModelMethodBody.ToString();

            this.Methods.Add(setModelMethod);
        }

        //private void AddPartialViewMethod()
        //{
        //    CSMethod PartialViewMethod = new CSMethod($"OnGet{this.grid.Query.Model.Name}Partial", "PartialViewResult", "public");
        //    PartialViewMethod.IsAsync = true;

        //    StringBuilder partialMethodBody = new StringBuilder();

        //    partialMethodBody.AppendLine($@"await SetModel();");

        //    partialMethodBody.AppendLine($"return new PartialViewResult");
        //    partialMethodBody.AppendLine($"{{");
        //    partialMethodBody.AppendLine($@"ViewName = ""_{this.grid.Query.Model.Name}Table"",");
        //    partialMethodBody.AppendLine($@"ViewData = new ViewDataDictionary<List<{this.grid.Query.Model.Name}>>(ViewData, {this.grid.Query.Model.Name})");
        //    partialMethodBody.AppendLine($"}};");

        //    PartialViewMethod.Body = partialMethodBody.ToString();

        //    this.Methods.Add(PartialViewMethod);
        //}

        private void SetModelMethod()
        {
            CSMethod setModelMethod = new CSMethod("SetModel", "void", "private");
            setModelMethod.IsAsync = true;

            StringBuilder setModelMethodBody = new StringBuilder();

            GenerateSetModelMethods(setModelMethodBody, this.PageControlMap.Childrens);

            setModelMethod.Body = setModelMethodBody.ToString();


            this.Methods.Add(setModelMethod);
        }

        private void GenerateSetModelMethods(StringBuilder setModelMethodBody, ICollection<IFPageControlMap> Childrens)
        {
            foreach (var child in Childrens)
            {
                string modelName = "";
                string queryName = "";

                if (child.IFPageControl is IFPageGrid)
                {
                    //PageGridControl gridControl = (PageGridControl)child;
                    IFPageGrid grid = (IFPageGrid)child.IFPageControl;
                    modelName = grid.Query.Model.Name;
                    queryName = grid.Query.Name;
                }
                else if (child.IFPageControl is IFPageListView)
                {
                    IFPageListView control = child.ToControl<IFPageListView>();
                    modelName = control.IFQuery.Model.Name;
                    queryName = control.IFQuery.Name;
                }
                else if (child.IFPageControl is IFPagePanel)
                {
                    IFPagePanel panel = child.ToControl<IFPagePanel>();
                    this.GenerateSetModelMethods(setModelMethodBody, panel.IFPageControlMap.Childrens);
                    continue;
                }
                else
                {
                    throw new BusinessException($"{child.GetType()} unknow page control type");
                }

                //if (!(child.IFPageControl is IFPagePanel))
                {

                    var requestParameter = "";

                    //Data = new LawCommonInfoFilter { LawId = this.LawId }

                    foreach (var parameter in page.IFPageParameters)
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

                    setModelMethodBody.AppendLine($@"var {modelName}Response = await this.apiClient.GetAsync<{queryName}Response>(""{this.PageControlMap.ToControl<IFPage>().Process.Name}Controller/{queryName}"",new {queryName}Request {requestParameter});");

                    setModelMethodBody.AppendLine($"this.{modelName} = {modelName}Response.Data;");
                }
            }
        }



    }
}
