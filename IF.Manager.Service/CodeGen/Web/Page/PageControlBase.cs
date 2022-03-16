using IF.CodeGeneration.Core;
using IF.CodeGeneration.Language.CSharp;
using IF.Manager.Contracts.Model;

using System.Text;

namespace IF.Manager.Service.Web.Page
{
    public abstract class PageControlBase : CSClass
    {
        public IFPageControlMap PageControlMap;
        public IFPage page;
        public IFPage topPage;
        public string pagePath;
        public string pageNameSpace { get; set; }
        protected FileSystemCodeFormatProvider fileSystem;

        public PageControlBase(IFPageControlMap map)
        {
            try
            {
                this.PageControlMap = map;
                this.pageNameSpace = this.PageControlMap.GetPageNameSpace();
                this.topPage = this.PageControlMap.GetTopPage();
                this.page = this.PageControlMap.GetTopPage(true);
                this.pagePath = PageControlMap.GetPagePath();
                this.BaseClass = "PageModel";
                this.NameSpace = SolutionHelper.GetPageNamespace(topPage, this.pageNameSpace);
                string generatedBasePath = DirectoryHelper.GetTempPageDirectory(this.pagePath, this.page.IFProject.Solution.SolutionName, this.page.IFProject.Name, this.page.IFProject.ProjectType);
                this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }


        public string GetPageNamespace()
        {
            string nameSpace = SolutionHelper.GetPageNamespace(topPage, this.pageNameSpace);
            return nameSpace;

        }

        public void AddPageNameSpaces()
        {
            //this.Usings.Add("IF.Rest.Client");
            this.Usings.Add("System.Collections.Generic");
            this.Usings.Add("System.Threading.Tasks");
            this.Usings.Add("Microsoft.AspNetCore.Mvc");
            this.Usings.Add("IF.Core.Rest");
            this.Usings.Add("Microsoft.AspNetCore.Mvc.RazorPages");
            this.Usings.Add("Microsoft.AspNetCore.Mvc.ViewFeatures");
            this.Usings.Add("System.ComponentModel.DataAnnotations");
            this.Usings.Add(SolutionHelper.GetProcessNamaspace(topPage.Process));
        }


        public void AddApiClientProperty()
        {
            var apiClientProperty = new CSProperty("private", "apiClient", false);
            apiClientProperty.PropertyTypeString = "IIFWebApiClient";
            apiClientProperty.IsReadOnly = true;
            this.Properties.Add(apiClientProperty);
        }


        public void AddConstructor()
        {
            CSMethod constructorMethod = new CSMethod(this.Name, "", "public");
            constructorMethod.Parameters.Add(new CsMethodParameter() { Name = "apiClient", Type = "IIFWebApiClient" });

            StringBuilder constructorMethodBody = new StringBuilder();
            constructorMethodBody.AppendFormat("this.apiClient = apiClient;");
            constructorMethodBody.AppendLine();
            constructorMethod.Body = constructorMethodBody.ToString();
            this.Methods.Add(constructorMethod);
        }



        public abstract void Generate();





    }
}
