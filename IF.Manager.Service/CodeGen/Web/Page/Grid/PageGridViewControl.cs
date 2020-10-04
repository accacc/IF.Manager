using IF.Core.Control;
using IF.Core.Exception;
using IF.Manager.Contracts.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IF.Manager.Service.Web.Page
{
    public class PageGridViewControl : PageControlBase
    {
        IFPageGrid grid;
        public PageGridViewControl(IFPageControlMap map) : base(map)
        {
            this.grid = this.PageControlMap.ToControl<IFPageGrid>();
        }
        public override void Generate()
        {

            StringBuilder builder = new StringBuilder();

            string nameSpace = SolutionHelper.GetProcessNamaspace(page.Process);

            builder.AppendLine($"@model List<{nameSpace}.{grid.Query.Model.Name}>");

            builder.AppendLine("@{");
            builder.AppendLine($@"Layout = null;");
            builder.AppendLine("}");

            builder.AppendLine();

            builder.AppendDivRowStart();
            builder.AppendDivColumnMd6();
            builder.AppendLine($"<h4>{grid.Description}</h4>");
            builder.AppendDivEnd();
            builder.AppendDivEnd();


            builder.AppendLine("<table>");
            builder.AppendLine("<tr>");

            List<IFPageControlMap> actionControls = this.grid.IFPageControlMap.Childrens.Where(g => g.IFPageControl.ControlType == PageControlType.Action).ToList();

            foreach (var actionControl in actionControls.Where(a => a.ToControl<IFPageAction>().WidgetType == Core.Mvc.ActionWidgetType.GridButton))
            {
                var action = actionControl.ToControl<IFPageAction>();

                var name = "";

                if (actionControl.Childrens.First().IFPageControl is IFPageForm)
                {
                    IFPageForm form = (IFPageForm)actionControl.Childrens.First().IFPageControl;
                    name = form.Name;
                }
                else if (actionControl.Childrens.First().IFPageControl is IFPageForm)
                {
                    IFPageForm page = (IFPageForm)actionControl.Childrens.First().IFPageControl;
                    name = page.Name;
                }

                builder.AppendLine("<td>");
                builder.AppendLine("<a class=\"btn btn-primary\"");
                builder.AppendLine($"href=\"@Url.Page(\"/{page.Name}/{name}\",\"{actionControl.IFPageControl.Name}\")\"");
                builder.AppendLine("if-ajax=\"true\"");
                builder.AppendLine("if-ajax-method=\"get\"");
                builder.AppendLine("if-ajax-mode=\"replace\"");
                builder.AppendLine("if-ajax-show-dialog=\"true\"");
                builder.AppendLine("if-ajax-modal-id=\"@Guid.NewGuid()\">");
                builder.AppendLine(action.Text);
                builder.AppendLine("</a>");
                builder.AppendLine("</td>");

            }



            builder.AppendLine("</tr>");
            builder.AppendLine("</table>");


            builder.AppendLine("<table class=\"table table-striped table-sm\">");
            builder.AppendLine("<tr>");



            foreach (var item in grid.Query.Model.Properties)
            {
                builder.AppendLine("<th>");
                builder.AppendLine(item.EntityProperty.Name);
                builder.AppendLine("</th>");
            }


            foreach (var actionControl in actionControls.Where(a => a.ToControl<IFPageAction>().WidgetType == Core.Mvc.ActionWidgetType.GridRowButton))
            {
                var action = actionControl.ToControl<IFPageAction>();
                builder.AppendLine("<th>");
                builder.AppendLine(action.Text);
                builder.AppendLine("</th>");
            }

            builder.AppendLine("</tr>");

            builder.AppendLine("@if(Model != null && Model.Any())");
            builder.AppendLine("{");
            builder.AppendLine("@foreach (var item in Model)");
            builder.AppendLine("{");
            builder.AppendLine("<tr>");

            foreach (var item in grid.Query.Model.Properties)
            {
                builder.AppendLine("<td>");
                if (grid.Query.Model.EntityId != item.EntityId)
                {
                    builder.AppendLine($"@Html.DisplayFor(modelItem => item.{item.Entity.Name}.{item.EntityProperty.Name})");
                }
                else
                {
                    builder.AppendLine($"@Html.DisplayFor(modelItem => item.{item.EntityProperty.Name})");
                }
                builder.AppendLine("</td>");
            }



            foreach (var actionControl in actionControls.Where(a => a.ToControl<IFPageAction>().WidgetType == Core.Mvc.ActionWidgetType.GridRowButton))
            {
                var action = actionControl.ToControl<IFPageAction>();

                var name = "";
                var queryString = "";
                var path = "";

                if (actionControl.Childrens.First().IFPageControl is IFPageForm)
                {
                    IFPageForm form = (IFPageForm)actionControl.Childrens.First().IFPageControl;
                    name = form.Name;

                    builder.AppendLine("<td>");
                    builder.AppendLine("<a class=\"btn btn-primary\"");
                    builder.AppendLine($"href=\"@Url.Page(\"/{page.Name}/{name}\",\"{action.Name}\",{queryString})\"");
                    builder.AppendLine("if-ajax=\"true\"");
                    builder.AppendLine("if-ajax-method=\"get\"");
                    builder.AppendLine("if-ajax-mode=\"replace\"");
                    builder.AppendLine("if-ajax-show-dialog=\"true\"");
                    builder.AppendLine("if-ajax-modal-id=\"@Guid.NewGuid()\">");
                    builder.AppendLine(action.Text);
                    builder.AppendLine("</a>");
                    builder.AppendLine("</td>");
                }
                else if (actionControl.Childrens.First().IFPageControl is IFPage)
                {
                    IFPage page = (IFPage)actionControl.Childrens.First().IFPageControl;

                    name = page.Name;
                    path = page.IFPageControlMap.GetPageUrl();

                    if (action.ActionType == Core.Mvc.ActionType.RedirectToAction)
                    {

                        foreach (var item in action.IFPageActionRouteValues)
                        {
                            queryString += $"{item.IFPageParameter.Name} = item.{item.IFModelProperty.EntityProperty.Name}";
                        }

                        if (queryString != "")
                        {
                            queryString = $"new {{{queryString}}}";
                        }

                        builder.AppendLine("<td>");
                        builder.AppendLine("<a class=\"btn btn-primary\"");
                        builder.AppendLine($"href=\"@Url.Page(\"/{path}\",{queryString})\"");
                        builder.AppendLine(">");
                        builder.AppendLine(action.Text);
                        builder.AppendLine("</a>");
                        builder.AppendLine("</td>");
                    }
                }
                else if (actionControl.Childrens.First().IFPageControl is IFPageListView)
                {
                    IFPageListView listView = actionControl.Childrens.First().ToControl<IFPageListView>();
                    name = listView.Name;
                    path = page.IFPageControlMap.GetPageUrl();

                    if (action.ActionType == Core.Mvc.ActionType.DirectActionOnButton)
                    {
                        foreach (var item in action.IFPageActionRouteValues)
                        {
                            if (item.IFModelProperty.EntityId != grid.Query.Model.EntityId)
                            {
                                queryString += $"{item.IFPageParameter.Name} = item.{item.IFModelProperty.Entity.Name}.{item.IFModelProperty.EntityProperty.Name}";
                            }
                            else
                            {
                                queryString += $"{item.IFPageParameter.Name} = item.{item.IFModelProperty.EntityProperty.Name}";
                            }

                        }

                        if (queryString != "")
                        {
                            queryString = $"new {{{queryString}}}";
                        }

                        path = $"{pagePath}/{listView.Name}";

                        builder.AppendLine("<td>");
                        builder.AppendLine("<a class=\"btn btn-primary\"");
                        builder.AppendLine($"href=\"@Url.Page(\"/{path}\",\"{listView.IFQuery.Name}\",{queryString})\"");
                        builder.AppendLine("if-ajax=\"true\"");
                        builder.AppendLine("if-ajax-method=\"get\"");
                        builder.AppendLine("if-ajax-mode=\"replace\"");
                        builder.AppendLine($"if-ajax-update-id=\"{action.IFPageControl.Name}\">");
                        builder.AppendLine(action.Text);
                        builder.AppendLine("</a>");
                        builder.AppendLine("</td>");
                    }
                }
                else
                {
                    throw new BusinessException($"{actionControl.Childrens.First().GetType()} unknow page control type");
                }
            }

            builder.AppendLine("</tr>");

            builder.AppendLine("}");
            builder.AppendLine("}");
            builder.AppendLine("else");
            builder.AppendLine("{");
            builder.AppendLine("@:Veri bulunamadı, Lütfen Kriter seçiniz");
            builder.AppendLine("}");
            builder.AppendLine("</table>");

            this.fileSystem.FormatCode(builder.ToString(), "cshtml", $"_{grid.Name}");
        }
    }
}
