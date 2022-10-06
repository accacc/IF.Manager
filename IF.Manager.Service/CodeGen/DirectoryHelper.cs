using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;
using System.IO;

namespace IF.Manager.Service
{
    public static class DirectoryHelper
    {
        public static void MoveDirectory(string from, string to, bool deleteExist = true)
        {


            if (Directory.Exists(to))
            {
                Directory.Delete(to, deleteExist);
            }

            Directory.Move(from, to);
        }

        public static string GetTempPageDirectory(IFPageControlMap pageControl)
        {
            var pagePath = pageControl.GetPagePath();

            var page = (IFPage)pageControl.IFPageControl;

            string path = pagePath;

            return $@"{GetTempGeneratedDirectoryName()}/{SolutionHelper.GetProjectNamespace(page.IFProject).Replace(".", "/")}/Pages/{path}";
        }
        public  static string GetTempPageDirectory(IFPage page)
        {
            return $@"{GetTempGeneratedDirectoryName()}/{SolutionHelper.GetProjectNamespace(page.IFProject).Replace(".", "/")}/Pages/{page.Name}";
        }

        public static string GetTempPageDirectory(string pageName, string solutionName, string projectName, ProjectType projectType)
        {
            return $@"{GetTempGeneratedDirectoryName()}/{SolutionHelper.GetProjectNamespace(solutionName,projectName,projectType).Replace(".", "/")}/Pages/{pageName}";
        }

        public static string GetTempGeneratedDirectoryName()
        {
            return @"C:/temp/generated";
        }

        public static string GetBaseTempDirectoryName()
        {
            return @"C:/temp/templateproject";
        }


        public static string GetTemplateProjectBaseName()
        {
            return "IF.Template";
        }

        public static string GetTemplateProjectBasePath()
        {
            return $@"C:\Projects\{GetTemplateProjectBaseName()}\";
        }


        public static string GetCoreBaseDllName(string solutionName)
        {
            return $"{solutionName}.Core.Base.dll";
        }


        public static void CopyProject(string solutionName, string solutionPath)
        {
            var from = $@"{GetBaseTempDirectoryName()}\{solutionName}";

            var to = solutionPath + "/" + solutionName;

            CopyAll(new DirectoryInfo(from), new DirectoryInfo(to));

            
        }

        public static void CreateDirectory(string directory, bool ifExistDelete)
        {
            if (System.IO.Directory.Exists($@"{directory}") && ifExistDelete)
            {
                System.IO.Directory.Delete($@"{directory}", true);
            }

            System.IO.Directory.CreateDirectory($@"{directory}");
        }

        public static void CopyAll(DirectoryInfo oOriginal, DirectoryInfo oFinal)
        {
            foreach (DirectoryInfo oFolder in oOriginal.GetDirectories())
                CopyAll(oFolder, oFinal.CreateSubdirectory(oFolder.Name));

            foreach (FileInfo oFile in oOriginal.GetFiles())
                oFile.CopyTo(oFinal.FullName + @"\" + oFile.Name, true);
        }

        public static string GetTempWebApiControllerDirectory(string processName)
        {
            return $@"{GetTempGeneratedDirectoryName()}/ApiControllers/{processName}";
        }

      

        public static string GetTemplateProjectPath(ProjectType type)
        {
            return $@"{GetTemplateProjectBasePath()}\{GetTemplateProjectBaseName()}.{type}";
        }

        public static string GetTempProjectFilePath(ProjectType projectType, string projectName, string solutionName)
        {
            return $"{GetTempProjectDirectory(projectType, projectName, solutionName)}/{solutionName}.{projectType}.csproj";
        }



        public static string GetTempProjectDirectory(ProjectType projectType,string projectName ,string solutionName)
        {
            return $"{GetBaseTempDirectoryName()}/{solutionName}/{solutionName}.{projectName}.{projectType}";
        }


        public static string GetProjectFullName(string solutionName,string projectName,ProjectType projectType)
        {
            return $"{solutionName}.{projectName}.{projectType}";
        }


        public static string GetTempProcessDirectory(IFProcess process)
        {
            return $@"{DirectoryHelper.GetTempGeneratedDirectoryName()}/{SolutionHelper.GetProcessNamaspace(process).Replace(".", "/")}";
        }
        public static string GetTempSolutionDirectory(string solutionName)
        {
            return $"{GetBaseTempDirectoryName()}/{solutionName}";
        }

        public static string GetTempSolutionFile(string solutionName)
        {
            return $"{GetTempSolutionDirectory(solutionName)}/{solutionName}.sln";
        }

        public static string GetNewSolutionFilePath(string solutionName, string solutionPath)
        {
            return $@"{GetNewSolutionPath(solutionPath,solutionName)}{solutionName}.sln";
        }

        public static string GetNewFrameworkDirectory(string solutionName, string solutionPath)
        {
            return $@"{GetNewSolutionPath(solutionPath, solutionName)}/packages/InFramework";
        }

        

        public static string GetNewSolutionPath(string solutionPath,string solutionName)
        {
            return $@"{solutionPath}/{solutionName}/";
        }

        public  static string GetNewApiControllerDirectory(IFProcess process)
        {
            return $@"{GetNewSolutionPath(process.Project.Solution.Path, process.Project.Solution.SolutionName)}{GetProjectFullName(process.Project.Solution.SolutionName, process.Project.Name,process.Project.ProjectType)}/Controllers/{process.Name}";
        }

        public static string GetNewProjectDirectory(IFProject project)
        {
            return $@"{GetNewSolutionPath(project.Solution.Path, project.Solution.SolutionName)}{GetProjectFullName(project.Solution.SolutionName, project.Name, project.ProjectType)}/";
        }

        public static string GetNewProcessDirectory(IFProcess process)
        {
            return $@"{GetNewSolutionPath(process.Project.Solution.Path,process.Project.Solution.SolutionName)}{GetNewCoreProjectName(process.Project.Solution.SolutionName)}{process.Name}";
        }

        private static string GetNewCoreProjectName(string solutionName)
        {
            return $"{solutionName}.Core/";
        }

        public static string GetNewCoreProjectDirectory(IFProject project)
        {
            return $@"{GetNewSolutionPath(project.Solution.Path, project.Solution.SolutionName)}{GetNewCoreProjectName(project.Solution.SolutionName)}";
        }

        public static string GetNewPageDirectory(IFPage page)
        {
            return $@"{GetNewSolutionPath(page.IFProject.Solution.Path, page.IFProject.Solution.SolutionName)}{GetProjectFullName(page.IFProject.Solution.SolutionName, page.IFProject.Name, page.IFProject.ProjectType)}/Pages/{page.Name}";
        }

        public static string GetNewPageDirectory(IFPageControlMap pageControl)
        {
            var pagePath = pageControl.GetPagePath();

            var page = (IFPage)pageControl.IFPageControl;

            string path = pagePath;

            return $@"{GetNewSolutionPath(page.IFProject.Solution.Path, page.IFProject.Solution.SolutionName)}{GetProjectFullName(page.IFProject.Solution.SolutionName, page.IFProject.Name, page.IFProject.ProjectType)}/Pages/{path}";
        }


    }
}
