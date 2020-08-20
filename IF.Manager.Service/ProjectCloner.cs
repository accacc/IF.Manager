using IF.Manager.Contracts.Enum;
using System.IO;

namespace IF.Manager.Service
{
    public class ProjectCloner
    {
        ProjectType projectType;
        string templateSolutionName;
        string newSolutionName;
        string projectName;
        string newProjectName;
        string solutionPath;

        public ProjectCloner(string templateSolutionName, string newSolutionName, string projectName, ProjectType projectType,string solutionPath)
        {
            this.templateSolutionName = templateSolutionName;
            this.newSolutionName = newSolutionName;
            this.projectName = projectName;
            this.projectType = projectType;
            this.newProjectName = newSolutionName + "." + projectName + "." + projectType;
            this.solutionPath = solutionPath;

        }
        public void Clone()
        {
            var source = new DirectoryInfo(DirectoryHelper.GetTemplateProjectPath(projectType));
            var target = new DirectoryInfo(DirectoryHelper.GetTempProjectDirectory(projectType, projectName, newSolutionName));

            CopyFilesRecursively(source, target, newSolutionName);

            var newSlnPath = DirectoryHelper.GetNewSolutionFilePath(newSolutionName, solutionPath);
            //string text = File.ReadAllText(DirectoryHelper.GetTempSolutionFile(newSolutionName));
            string text = File.ReadAllText(newSlnPath);
            text = text.Replace(newSolutionName + "." + projectType, this.newProjectName);

            switch (projectType)
            {
                case ProjectType.Web:
                    text = text.Replace("#web_project","");
                    break;
                case ProjectType.Api:
                    text = text.Replace("#web_api_project", "");
                    text = text.Replace("#core_project", "");
                    break;
                default:
                    break;
            }

            File.WriteAllText(newSlnPath, text);
        }
        public void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, string newSolutionName)
        {

            var dirs = source.GetDirectories();

            foreach (DirectoryInfo dir in dirs)
            {
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name.Replace(templateSolutionName, this.newProjectName)), newSolutionName);
            }

            foreach (FileInfo file in source.GetFiles())
            {

                try
                {
                    if (file.Extension == ".dll")
                    {
                        var newFileName = file.Name.Replace(templateSolutionName, newSolutionName);
                        file.CopyTo(Path.Combine(target.FullName, newFileName));
                    }
                    else
                    {
                        var newFileName = file.Name.Replace(templateSolutionName, newSolutionName + "." + projectName);
                        file.CopyTo(Path.Combine(target.FullName, newFileName));
                        string text = File.ReadAllText(Path.Combine(target.FullName, newFileName));
                        text = text.Replace(templateSolutionName, newSolutionName);
                        File.WriteAllText(Path.Combine(target.FullName, newFileName), text);
                    }
                }
                catch (System.Exception ex)
                {

                    throw;
                }

            }

        }

    }
}
