using IF.CodeGeneration.Core;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IF.Manager.Service.Web.Page.Panel
{
    public class PagePanelControl : PageControlBase
    {


        IFPagePanel panel;
        FileSystemCodeFormatProvider fileSystem;


        public PagePanelControl(IFPageControlMap map) : base(map)
        {
            this.panel = this.PageControlMap.ToControl<IFPagePanel>();
            //string Path = DirectoryHelper.GetPagePath("", this.pagePath);
            string Path = this.pagePath;
            string generatedBasePath = DirectoryHelper.GetTempPageDirectory(Path, this.page.IFProject.Solution.SolutionName, this.page.IFProject.Name, this.page.IFProject.ProjectType);
            this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
        }



        public override void Generate()
        {
            string pageNameSpace = base.GetPageNamespace();

            StringBuilder builder = new StringBuilder();
            builder.AppendDivStart(this.panel.CssClass,this.panel.Name);

            GeneratePartialViews(1, pageNameSpace, builder);

            builder.AppendDivEnd();
            this.fileSystem.FormatCode(builder.ToString(), "cshtml", $"{panel.Name}");
        }

        //TODO:base classa al 112
        private void GeneratePartialViews(int columnCount, string pageNameSpace, StringBuilder builder)
        {
            //builder.AppendLine("@page");
            //builder.AppendLine($"@model {pageNameSpace}.Model");
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
                    builder.AppendLine($@"@{{await Html.RenderPartialAsync(""{name}"");}}");
                    builder.AppendDivEnd();

                }
                else
                {

                    builder.AppendDivStart("col", $@"{name}Div");
                    builder.AppendLine($@"@{{await Html.RenderPartialAsync(""{name}"", Model.{modelName});}}");
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

    }
}