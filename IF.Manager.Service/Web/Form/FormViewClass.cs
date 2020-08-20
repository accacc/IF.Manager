//using IF.CodeGeneration.Core;
//using IF.Manager.Contracts.Model;
//using System.Collections.Generic;
//using System.Text;

//namespace IF.Manager.Service.Web.Form
//{



//    public class FormViewClass
//    {


//        FileSystemCodeFormatProvider fileSystem;
//        string generatedBasePath;
//        IFPage page;
//        IFPageGrid grid;
//        IFPageForm form;
//        IFPageAction action;
//        List<IFPageAction> actions;
//        public FormViewClass(IFPage page, IFPageGrid grid, IFPageAction action, IFPageForm form, List<IFPageAction> actions)
//        {
//            this.page = page;
//            generatedBasePath = DirectoryHelper.GetTempPageDirectory(page);
//            this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
//            this.form = form;
//            this.actions = actions;
//            this.action = action;
//            this.grid = grid;
//        }


//        public void Generate()
//        {
//            StringBuilder builder = new StringBuilder();

//            string coreNameSpace = SolutionHelper.GetPageNamespace(page);
//            builder.AppendLine("@page");
//            builder.AppendLine("@{");
//            builder.AppendLine("Layout = \"_ModalLayout\";");
//            builder.AppendLine("}");

//            builder.AppendLine($"@model {coreNameSpace}.{this.form.Name}Model");
//            builder.AppendLine();

//            builder.AppendLine("<form>");
//            builder.AppendLine("@Html.AntiForgeryToken()");

//            foreach (var item in form.IFPageFormItemModelProperties)
//            {


//                string required = "required";

//                if (item.IFModelProperty.EntityProperty.IsNullable)
//                {
//                    required = "";
//                }

//                builder.AppendLine("<div class=\"row\">");
//                builder.AppendLine("<div class=\"col-md-6\">");
//                builder.AppendLine("<div class=\"form-group\">");
//                builder.AppendLine($"<label for=\"{item.IFModelProperty.EntityProperty.Name}\">{item.IFModelProperty.EntityProperty.Name}</label>");
//                builder.AppendLine($"<input type=\"text\" name=\"Form.{item.IFModelProperty.EntityProperty.Name}\" class=\"form-control\" value=\"@Model.Form.{item.IFModelProperty.EntityProperty.Name}\" {required} />");
//                builder.AppendLine($"</div>");
//                builder.AppendLine($"</div>");
//                builder.AppendLine($"</div>");
//            }

//            foreach (var action in actions)
//            {

//                builder.AppendLine("<div class=\"row\">");
//                builder.AppendLine("<div class=\"col-md-6\">");
//                builder.AppendLine("<button type=\"submit\" class=\"btn btn-primary\"");
//                builder.AppendLine($"if-ajax-action=\"@Url.Page(\"/{page.Name}/{form.Name}\",\"{action.Name}\")\"");
//                builder.AppendLine("if-ajax-form-submit=\"true\"");
//                builder.AppendLine("if-ajax-method=\"post\"");
//                builder.AppendLine("if-ajax-close-modal-on-success=\"true\"");
//                builder.AppendLine($"if-ajax-update-id=\"{grid.Name}Div\"");
//                builder.AppendLine(">");
//                builder.AppendLine("Kaydet");
//                builder.AppendLine("</button>");
//                builder.AppendLine($"</div>");
//                builder.AppendLine($"</div>");
//            }

//            builder.AppendLine($"@Html.HiddenFor(model => model.Form.Id)");


//            builder.AppendLine("</form>");

//            fileSystem.FormatCode(builder.ToString(), "cshtml", this.form.Name);
//        }
//    }

//}
