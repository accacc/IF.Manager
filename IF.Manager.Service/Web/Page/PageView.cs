//using IF.CodeGeneration.Core;
//using IF.Core.Exception;
//using IF.Manager.Contracts.Dto;
//using IF.Manager.Contracts.Model;
//using Microsoft.CodeAnalysis;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace IF.Manager.Service.Web.Page
//{
//    public class PageView
//    {

//        IFPage page;
//        IFPageControlMap PageControlMap;
//        FileSystemCodeFormatProvider fileSystem;
//        string pagePath;
//        IFPage topPage;
//        string pageNameSpace;
//        public PageView(IFPage topPage, string pagePath, string pageNameSpace, IFPageControlMap PageControlMap)
//        {
//            this.pageNameSpace = pageNameSpace;
//            this.PageControlMap = PageControlMap;
//            this.page = PageControlMap.IFPageControl as IFPage;

//            this.topPage = topPage;
//            this.pagePath = pagePath;

//            string path = pagePath;//DirectoryHelper.GetPagePath(page.Name, pagePath);

//            string generatedBasePath = DirectoryHelper.GetTempPageDirectory(path, this.topPage.IFProject.Solution.SolutionName, this.topPage.IFProject.Name, this.topPage.IFProject.ProjectType);

//            //string generatedBasePath = DirectoryHelper.GetTempPageDirectory(this.page.Name, this.page.IFProject.Solution.SolutionName, this.page.IFProject.Name, this.page.IFProject.ProjectType);
//            this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
//        }



//        public void RenderPageView()
//        {
//            var columnCount = this.page.PageLayout.ColumSize;

//            string pageNameSpace = SolutionHelper.GetPageNamespace(topPage, this.pageNameSpace);

//            StringBuilder builder = new StringBuilder();

//            GeneratePartialViews(columnCount, pageNameSpace, builder);

//            this.fileSystem.FormatCode(builder.ToString(), "cshtml", $"{page.Name}");
//        }

//        //TODO:base classa al 112
//        private void GeneratePartialViews(int columnCount, string pageNameSpace, StringBuilder builder)
//        {
//            builder.AppendLine("@page");
//            builder.AppendLine($"@model {pageNameSpace}.{this.page.Name}Model");
//            builder.AppendDivStart("container");
//            builder.AppendDivStart("row");

//            var modelName = "";
//            var name = "";

//            for (int i = 0; i < this.PageControlMap.Childrens.Count; i++)
//            {

//                var child = this.PageControlMap.Childrens.ElementAt(i);


//                if (child.IFPageControl is IFPageGrid)
//                {
//                    IFPageGrid grid = child.ToControl<IFPageGrid>();
//                    modelName = grid.Query.Model.Name;
//                    name = grid.Name;
//                }
//                else if (child.IFPageControl is IFPageListView)
//                {
//                    IFPageListView listView = child.ToControl<IFPageListView>();
//                    modelName = listView.IFQuery.Model.Name;
//                    name = listView.Name;
//                }
//                else if (child.IFPageControl is IFPagePanel)
//                {
//                    IFPagePanel panel = child.ToControl<IFPagePanel>();
//                    name = panel.Name;
//                }
//                else
//                {
//                    throw new BusinessException($"{child.GetType()} unknow page control type");
//                }

//                if (child.IFPageControl is IFPagePanel)
//                {
//                    builder.AppendDivStart("col", $@"{name}Div");
//                    builder.AppendLine($@"@{{await Html.RenderPartialAsync(""{name}"");}}");
//                    builder.AppendDivEnd();

//                }
//                else
//                {

//                    builder.AppendDivStart("col", $@"{name}Div");
//                    builder.AppendLine($@"@{{await Html.RenderPartialAsync(""{name}"", Model.{modelName});}}");
//                    builder.AppendDivEnd();

//                }


//                if ((i + 1) % columnCount == 0)
//                {
//                    builder.AppendDivStart("w-100");
//                    builder.AppendDivEnd();
//                }

//            }

//            builder.AppendDivEnd();
//            builder.AppendDivEnd();
//        }
//    }
//}
