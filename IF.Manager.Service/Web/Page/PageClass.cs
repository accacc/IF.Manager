//using IF.CodeGeneration.Core;
//using IF.CodeGeneration.CSharp;
//using IF.Core.Exception;
//using IF.Manager.Contracts.Model;
//using Microsoft.EntityFrameworkCore.Internal;
//using System.Collections.Generic;
//using System.Text;

//namespace IF.Manager.Service.Web.Page
//{
//    public class PageClass : PageClassBase
//    {

//        FileSystemCodeFormatProvider fileSystem;
//        IFPageControlMap PageControlMap;
//        string pagePath;
//        IFPage page;
//        public PageClass(IFPage topPage, string pagePath, IFPageControlMap pageControlMap, string pageNamespace, string pageName) : base(topPage, pageNamespace, pageName)
//        {

//            this.PageControlMap = pageControlMap;
//            this.page = this.PageControlMap.ToControl<IFPage>();
//            this.Name = $"{this.page.Name}Model";
//            this.topPage = topPage;
//            this.pagePath = pagePath;
//            string path = pagePath;// DirectoryHelper.GetPagePath(this.PageControlMap.IFPageControl.Name, pagePath);
//            string generatedBasePath = DirectoryHelper.GetTempPageDirectory(path, this.topPage.IFProject.Solution.SolutionName, this.topPage.IFProject.Name, this.topPage.IFProject.ProjectType);
//            this.fileSystem = new FileSystemCodeFormatProvider(generatedBasePath);
//        }


    
//    }
//}
