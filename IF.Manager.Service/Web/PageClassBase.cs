//using IF.CodeGeneration.CSharp;
//using IF.Manager.Contracts.Model;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace IF.Manager.Service.Web
//{
//    public abstract class PageClassBase:CSClass
//    {

//        public IFPage topPage;
//        public string PageNameSpace { get; set; }
//        public PageClassBase(IFPage topPage,string PageNameSpace,string pageName)
//        {
//            this.BaseClass = "PageModel";
//            this.PageNameSpace = PageNameSpace;
//            this.NameSpace = SolutionHelper.GetPageNamespace(topPage,this.PageNameSpace);
//            this.topPage = topPage;
            
//        }

//        public void AddNameSpaces()
//        {
//            //this.Usings.Add("IF.Rest.Client");
//            this.Usings.Add("System.Collections.Generic");
//            this.Usings.Add("System.Threading.Tasks");
//            this.Usings.Add("Microsoft.AspNetCore.Mvc");
//            this.Usings.Add("IF.Core.Rest");
//            this.Usings.Add("Microsoft.AspNetCore.Mvc.RazorPages");
//            this.Usings.Add("Microsoft.AspNetCore.Mvc.ViewFeatures");
//            this.Usings.Add("System.ComponentModel.DataAnnotations");
//            this.Usings.Add(SolutionHelper.GetProcessNamaspace(topPage.Process));
//        }


//        public void AddApiClientProperty()
//        {
//            var apiClientProperty = new CSProperty("private", "apiClient", false);
//            apiClientProperty.PropertyTypeString = "IIFWebApiClient";
//            apiClientProperty.IsReadOnly = true;
//            this.Properties.Add(apiClientProperty);
//        }


//        public void AddConstructor()
//        {
//            CSMethod constructorMethod = new CSMethod(this.Name, "", "public");
//            constructorMethod.Parameters.Add(new CsMethodParameter() { Name = "apiClient", Type = "IIFWebApiClient" });

//            StringBuilder constructorMethodBody = new StringBuilder();
//            constructorMethodBody.AppendFormat("this.apiClient = apiClient;");
//            constructorMethodBody.AppendLine();
//            constructorMethod.Body = constructorMethodBody.ToString();
//            this.Methods.Add(constructorMethod);
//        }
//    }
//}
