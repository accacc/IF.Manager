using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;

namespace IF.Manager.Service
{
    public static class SolutionHelper
    {
        public static string GetProcessNamaspace(IFProcess process)
        {
            return $"{GetCoreNamespace(process.Project)}.{process.Name}";
        }

        public static string GetPageNamespace(IFPage page)
        {
            return $"{GetProjectNamespace(page.IFProject)}.{page.Name}";
        }

        public static string GetPageNamespace(IFPage page,string pageNamespace)
        {
            return $"{GetProjectNamespace(page.IFProject)}.{pageNamespace}";
        }

        public static string GetCoreNamespace(IFProject project)
        {
            return $"{project.Solution.SolutionName}.Core";
        }

        public static string GetCoreBaseNamespace(IFProject project)
        {
            return $"{project.Solution.SolutionName}.Core.Base";
        }


        public static string GetProjectNamespace(string solutionName,string projectName,ProjectType projectType)
        {
            return $"{solutionName}.{projectName}.{projectType}";
        }
        public static string GetProjectNamespace(IFProject project)
        {
            return $"{project.Solution.SolutionName}.{project.Name}.{project.ProjectType}";
        }

        public  static object GetProjectFullName(IFProject Project)
        {
            return $"{Project.Solution.SolutionName} {Project.Name}";
        }
    }
}
