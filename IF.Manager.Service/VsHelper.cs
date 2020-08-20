using IF.Manager.Contracts.Enum;
using IF.Manager.Contracts.Model;
using System.Collections.Generic;
using System.IO;

namespace IF.Manager.Service
{
    public class VsHelper
    {
        
        private readonly string generatedBasePath;
        public VsHelper(string generatedBasePath)
        {
            this.generatedBasePath = generatedBasePath;
        }

        public void GenerateSolution(string solutionName)
        {
            if (Directory.Exists(DirectoryHelper.GetBaseTempDirectoryName()))
            {
                Directory.Delete(DirectoryHelper.GetBaseTempDirectoryName(), true);
            }
            
            List<string> projects = new List<string>();
            projects.Add($"{DirectoryHelper.GetTemplateProjectBaseName()}.Core");

            SolutionCloner cloner = new SolutionCloner(DirectoryHelper.GetTemplateProjectBaseName(), $"{solutionName}",projects);
            cloner.Clone();



        }


        public void GenerateProject(string projectName,ProjectType projectType ,string solutionName,string solutionPath)
        {
            List<string> projects = new List<string>();            
            projects.Add($"{DirectoryHelper.GetTemplateProjectBaseName()}.{projectType}");
            ProjectCloner cloner = new ProjectCloner(DirectoryHelper.GetTemplateProjectBaseName(), $"{solutionName}", projectName,projectType,solutionPath);
            cloner.Clone();

        }


        public void AddConfigJsonFileToApiProject(ProjectType projectType, string projectName, string solutionName)
        {
            string toPath = DirectoryHelper.GetTempProjectDirectory(projectType,projectName ,solutionName);
            File.Copy($"{generatedBasePath}/appsettings.Development.json", $@"{toPath}/appsettings.Development.json", true);
            File.Copy($"{generatedBasePath}/appsettings.json", $@"{toPath}/appsettings.json", true);
            File.Delete($"{generatedBasePath}/appsettings.json");
            File.Delete($"{generatedBasePath}/appsettings.Development.json");
        }

        public void AddMenuJsonFileToApiProject(IFProject project)
        {
            string toPath = DirectoryHelper.GetNewProjectDirectory(project);
            File.Copy($"{generatedBasePath}/menu.json", $@"{toPath}/menu.json", true);            
            File.Delete($"{generatedBasePath}/menu.json");
            
        }


        public void AddClassToProject(ProjectType projectType, string projectName, string className, string solutionName)
        {
            var extension = "cs";
            string fromPath = $"{generatedBasePath}";
            string toPath = DirectoryHelper.GetTempProjectDirectory(projectType, projectName,solutionName);
            string itemName = className;            
            File.Copy($"{fromPath}/{itemName}.{extension}", $@"{toPath}/{itemName}.{extension}", true);
            File.Delete($"{fromPath}/{className}.cs");
        }

        public void AddDllReferenceToApiProject(string dllName, string solutionName,string solutionPath)
        {

            var itemName = dllName;
            var extension = "dll";

            var fromPath = DirectoryHelper.GetTempGeneratedDirectoryName();
            var toPath = $"{DirectoryHelper.GetNewFrameworkDirectory(solutionName,solutionPath)}";
            File.Copy($"{fromPath}/{itemName}.{extension}", $@"{toPath}/{itemName}.{extension}", true);

        }

        public void AddDllReferenceToCoreProject(string dll, string solutionName, string solutionPath)
        {
            var itemName = dll;
            var extension = "dll";            
            var fromPath = DirectoryHelper.GetTempGeneratedDirectoryName();
            var toPath = $"{DirectoryHelper.GetNewFrameworkDirectory(solutionName,solutionPath)}";
            File.Copy($"{fromPath}/{itemName}.{extension}", $@"{toPath}/{itemName}.{extension}", true);
            
        }

        public void ExploreSolution(string slnPath, string solutionName)
        {
            ExploreFile($@"{slnPath}\{solutionName}\{solutionName}.sln");
        }

        private bool ExploreFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return false;
            }

            filePath = System.IO.Path.GetFullPath(filePath);
            System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", filePath));
            return true;
        }
    }
}
