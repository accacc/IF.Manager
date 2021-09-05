using IF.CodeGeneration.Language.CSharp;
using IF.Core.Exception;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IF.Manager.Service.Web.Page.Form
{

    public class PageFormControl : PageControlBase
    {
        IFPageForm form;
        public PageFormControl(IFPageControlMap map) : base(map)
        {
            this.form = this.PageControlMap.ToControl<IFPageForm>();
            this.Name = $"{this.form.Name}Model";
        }
        public override void Generate()
        {
            GenerateFormClass();
            GenerateView();           
            
        }

        private void GenerateView()
        {
            StringBuilder builder = new StringBuilder();

            string coreNameSpace = SolutionHelper.GetPageNamespace(page);
            builder.AppendLine("@page");
            builder.AppendLine("@{");
            builder.AppendLine("Layout = \"_ModalLayout\";");
            builder.AppendLine("}");

            builder.AppendLine($"@model {coreNameSpace}.{this.form.Name}Model");
            builder.AppendLine();

            builder.AppendLine("<form>");
            builder.AppendLine("@Html.AntiForgeryToken()");

            foreach (var item in form.IFPageFormItemModelProperties.OrderBy(p=>p.Sequence))
            {

                string required = "required";

                if (item.IFModelProperty.EntityProperty.IsNullable)
                {
                    required = "";
                }

                builder.AppendLine("<div class=\"row\">");
                builder.AppendLine("<div class=\"col-md-6\">");
                builder.AppendLine("<div class=\"form-group\">");
                builder.AppendLine($"<label for=\"{item.IFModelProperty.EntityProperty.Name}\">{item.IFModelProperty.EntityProperty.Name}</label>");


                

                builder.AppendLine($"<input type=\"text\" name=\"Form.{item.IFModelProperty.EntityProperty.Name}\" class=\"form-control\" value=\"@Model.Form.{item.IFModelProperty.EntityProperty.Name}\" {required} />");
                
                
                
                
                builder.AppendLine($"</div>");
                builder.AppendLine($"</div>");
                builder.AppendLine($"</div>");
            }

            foreach (var child in this.PageControlMap.Childrens)
            {


                if (!(child.IFPageControl is IFPageAction)) continue;

                var action = child.IFPageControl as IFPageAction;
                


                builder.AppendLine("<div class=\"row\">");
                builder.AppendLine("<div class=\"col-md-6\">");
                builder.AppendLine("<button type=\"submit\" class=\"btn btn-primary\"");
                builder.AppendLine($"if-ajax-action=\"@Url.Page(\"/{page.Name}/{form.Name}\",\"{action.Name}\")\"");
                builder.AppendLine("if-ajax-form-submit=\"true\"");
                builder.AppendLine("if-ajax-method=\"post\"");
                builder.AppendLine("if-ajax-close-modal-on-success=\"true\"");
                builder.AppendLine($"if-ajax-update-id=\"{action.IFPageControl.Name}Div\"");
                builder.AppendLine(">");
                builder.AppendLine("Kaydet");
                builder.AppendLine("</button>");
                builder.AppendLine($"</div>");
                builder.AppendLine($"</div>");
            }

            builder.AppendLine($"@Html.HiddenFor(model => model.Form.Id)");


            builder.AppendLine("</form>");

            fileSystem.FormatCode(builder.ToString(), "cshtml", this.form.Name,"");
        }

        private void GenerateFormClass()
        {
            base.AddPageNameSpaces();
            base.AddConstructor();
            base.AddApiClientProperty();
            AddFormModelProperty();
            AddGetMethod();
            AddSetFormDefaultsMethod();

            foreach (var child in this.PageControlMap.Childrens)
            {
                if (child.IFPageControl is IFPageAction)
                {
                    //TODO:Caglar
                    AddPostMethod(child.IFPageControl as IFPageAction);
                }
            }

            fileSystem.FormatCode(this.GenerateCode().Template, "cshtml.cs", this.PageControlMap.IFPageControl.Name,"");
        }

        private void AddSetFormDefaultsMethod()
        {
            CSMethod method = new CSMethod("SetFormDefaults", "void", "public");
            method.IsAsync = true;

            StringBuilder methodBody = new StringBuilder();

            foreach (var formProperty in this.form.IFPageFormItemModelProperties)
            {
                //formProperty.IFPageControlItemModelProperties
            }
            

            method.Body = methodBody.ToString();

            this.Methods.Add(method);
        }

        private void AddPostMethod(IFPageAction action)
        {

            CSMethod method = new CSMethod($"OnPost{action.Name}", "PartialViewResult", "public");
            method.IsAsync = true;

            StringBuilder methodBody = new StringBuilder();

            if (action.Command != null)
            {                

                CsMethodParameter parameter = new CsMethodParameter();
                parameter.Name = "form";
                parameter.Type = $"{action.Command.Model.Name}";
                method.Parameters.Add(parameter);
                method.IsAsync = true;

             

                methodBody.AppendLine($"{action.Command.Name}Command command = new {action.Command.Name}Command();");

                methodBody.AppendLine($"command.Data = form;");

                methodBody.AppendLine($@"await this.apiClient.PostAsync<{action.Command.Name}Command>(""{topPage.Process.Name}Controller/{action.Command.Name}"",command);");                
            }

            if (action.IFPageControl != null)
            {

                var query = action.IFPageControl.GetQuery();

                methodBody.AppendLine($@"var response = await this.apiClient.GetAsync<{query.Name}Response>(""{topPage.Process.Name}Controller/{query.Name}"",new {query.Name}Request());");

                methodBody.AppendLine($"var list = response.Data;");

                methodBody.AppendLine($"return new PartialViewResult");
                methodBody.AppendLine($"{{");
                methodBody.AppendLine($@"ViewName = ""_{action.IFPageControl.Name}"",");
                methodBody.AppendLine($@"ViewData = new ViewDataDictionary<List<{query.Model.Name}>>(ViewData, list)");
            
            }

            methodBody.AppendLine($"}};");

            method.Body = methodBody.ToString();

            this.Methods.Add(method);
        }


        private void AddGetMethod()
        {
            CSMethod method = null;

            //getMethod.IsAsync = true;

            StringBuilder methodBody = new StringBuilder();


            if (this.form.IFQuery!=null)
            {
                method = new CSMethod($"OnGet{this.form.IFQuery.Name}", "void", "public");
                method.IsAsync = true;

                var filterParameter = new CsMethodParameter();
                filterParameter.Name = "filter";
                filterParameter.Type = $"{this.form.IFQuery.Name}Filter";
                method.Parameters.Add(filterParameter);


                methodBody.AppendLine($@"var response = await this.apiClient.GetAsync<{this.form.IFQuery.Name}Response>(""{topPage.Process.Name}Controller/{this.form.IFQuery.Name}"",new {this.form.IFQuery.Name}Request(){{ Data = filter }});");
                methodBody.AppendLine($@"this.Form = response.Data;");
            }
            else if(this.form.IFModel !=null)
            {
                method = new CSMethod($"OnGet{this.PageControlMap.Parent.IFPageControl.Name}", "void", "public");
                methodBody.AppendLine($@"this.Form = new {this.form.IFModel.Name}();");
            }
            else
            {
                throw new BusinessException("Form için model ya da bir query tanimlanmamış.");
            }


            method.Body = methodBody.ToString();


            this.Methods.Add(method);
        }

        private void AddFormModelProperty()
        {
            IFModel model = this.form.IFModel;

            if(model == null)
            {
                model = this.form.IFQuery.Model;
            }

            if (model == null)
            {
                throw new BusinessException("Bu form için herhangi bir model ya da query tanımı yapılmamış.");
            }

                var formProprety = new CSProperty("public", "Form", false);
            formProprety.PropertyTypeString = $"{model.Name}";
            formProprety.Attirubites.Add("BindProperty");
            formProprety.Attirubites.Add("Required");
            this.Properties.Add(formProprety);
        }


    }


}
