//using IF.CodeGeneration.Language.CSharp;
//using IF.Manager.Contracts.Model;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace IF.Manager.Service.Web.Form
//{

//    public class FormCsClass : PageClassBase
//    {

//        //IFPage page;
//        IFPageAction action;
//        IFPageGrid grid;
//        IList<IFPageAction> actions;
//        public FormCsClass(IFPage page, IFPageGrid grid, IFPageAction action, IList<IFPageAction> actions, string name, string pageNameSpace) : base(page, pageNameSpace, page.Name)
//        {
//            this.action = action;
//            this.actions = actions;
//            this.Name = name;
//            this.grid = grid;

//            //this.IsAsync = true;
//        }



//        public void Generate()
//        {

//            base.AddNameSpaces();
//            base.AddConstructor();
//            base.AddApiClientProperty();
//            AddFormModelProperty();
//            AddGetMethod();

//            foreach (var action in actions)
//            {
//                AddPostMethod(action);
//            }
//        }

//        private void AddPostMethod(IFPageAction action)
//        {
//            CSMethod method = new CSMethod($"OnPost{action.Name}", "PartialViewResult", "public");
//            method.IsAsync = true;

//            CsMethodParameter parameter = new CsMethodParameter();
//            parameter.Name = "form";
//            parameter.Type = $"{action.Command.Model.Name}";
//            method.Parameters.Add(parameter);
//            method.IsAsync = true;

//            StringBuilder methodBody = new StringBuilder();

//            methodBody.AppendLine($"{action.Command.Name}Command command = new {action.Command.Name}Command();");

//            methodBody.AppendLine($"command.Data = form;");

//            methodBody.AppendLine($@"await this.apiClient.PostAsync<{action.Command.Name}Command>(""{topPage.Process.Name}Controller/{action.Command.Name}"",command);");

//            methodBody.AppendLine($@"var response = await this.apiClient.GetAsync<{this.grid.Query.Name}Response>(""{topPage.Process.Name}Controller/{grid.Query.Name}"",new {this.grid.Query.Name}Request());");

//            methodBody.AppendLine($"var list = response.Data;");

//            methodBody.AppendLine($"return new PartialViewResult");
//            methodBody.AppendLine($"{{");
//            methodBody.AppendLine($@"ViewName = ""_{this.grid.Name}Table"",");
//            methodBody.AppendLine($@"ViewData = new ViewDataDictionary<List<{this.grid.Query.Model.Name}>>(ViewData, list)");
//            methodBody.AppendLine($"}};");

//            method.Body = methodBody.ToString();

//            this.Methods.Add(method);
//        }


//        private void AddGetMethod()
//        {
//            CSMethod method = null;

//            //getMethod.IsAsync = true;

//            StringBuilder methodBody = new StringBuilder();


//            if (action.Query != null)
//            {
//                method = new CSMethod($"OnGet{action.Name}", "void", "public");
//                method.IsAsync = true;

//                var filterParameter = new CsMethodParameter();
//                filterParameter.Name = "filter";
//                filterParameter.Type = $"{this.action.Query.Name}Filter";
//                method.Parameters.Add(filterParameter);


//                methodBody.AppendLine($@"var response = await this.apiClient.GetAsync<{this.action.Query.Name}Response>(""{topPage.Process.Name}Controller/{action.Query.Name}"",new {this.action.Query.Name}Request(){{ Data = filter }});");
//                methodBody.AppendLine($@"this.Form = response.Data;");
//            }
//            else
//            {


//                method = new CSMethod($"OnGet{action.Name}", "void", "public");
//                methodBody.AppendLine($@"this.Form = new {action.IFModel.Name}();");
//            }


//            method.Body = methodBody.ToString();


//            this.Methods.Add(method);
//        }

//        private void AddFormModelProperty()
//        {
//            var formProprety = new CSProperty("public", "Form", false);
//            formProprety.PropertyTypeString = $"{this.action.IFModel.Name}";
//            formProprety.Attirubites.Add("BindProperty");
//            formProprety.Attirubites.Add("Required");
//            this.Properties.Add(formProprety);
//        }


//    }
//}
