using System.Collections.Generic;
using System.IO;

namespace IF.Manager.Service
{
    public class SolutionCloner
    {
        string templateSolutionName;
        string newSolutionName;
        List<string> projects;

        public SolutionCloner(string templateSolutionName, string newSolutionName, List<string> projects)
        {
            this.templateSolutionName = templateSolutionName;
            this.newSolutionName = newSolutionName;
            this.projects = projects;

             
        }
        public void Clone()
        {
            var source = new DirectoryInfo(DirectoryHelper.GetTemplateProjectBasePath());
            var target = new DirectoryInfo(DirectoryHelper.GetTempSolutionDirectory(newSolutionName));

            CopyFilesRecursively(source, target, newSolutionName);


            var newSolutionNamePath = DirectoryHelper.GetBaseTempDirectoryName() + @"\" + templateSolutionName.Replace(templateSolutionName, newSolutionName) + @"\" + templateSolutionName.Replace(templateSolutionName, newSolutionName) + ".sln";

            CreateSolutionFile(newSolutionName, newSolutionNamePath);


        }

        private void CreateSolutionFile(string newSolutionName, string newSolutionNamePath)
        {

            var oldSolutionPath = DirectoryHelper.GetTemplateProjectBasePath() + templateSolutionName + ".sln";
            File.Copy(oldSolutionPath, newSolutionNamePath, true);
            string text = File.ReadAllText(newSolutionNamePath);
            text = text.Replace(templateSolutionName, templateSolutionName);
            text = text.Replace(templateSolutionName, newSolutionName);
            File.WriteAllText(newSolutionNamePath, text);

        }



        public void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, string newSolutionName)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {

                if (source.Name == DirectoryHelper.GetTemplateProjectBaseName() && dir.Name != "packages" && !projects.Contains(dir.Name))
                {
                    continue;
                }

                if (dir.Name == ".git")
                {
                    continue;
                }

                if (dir.Name == ".vs")
                {
                    continue;
                }


                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name.Replace(templateSolutionName, newSolutionName)), newSolutionName);
            }

            foreach (FileInfo file in source.GetFiles())
            {


                if (file.Extension == ".dll")
                {
                    var newFileName = file.Name.Replace(templateSolutionName, newSolutionName);
                    file.CopyTo(Path.Combine(target.FullName, newFileName));
                }
                else if (file.Extension == ".csproj")
                {
                    HandleProjects(target, file, newSolutionName);
                }
                else
                {
                    HandleOthers(target, newSolutionName, file);
                }
            }

        }
        private void HandleOthers(DirectoryInfo target, string newSolutionName, FileInfo file)
        {
            var newFileName = file.Name.Replace(templateSolutionName, newSolutionName);
            file.CopyTo(Path.Combine(target.FullName, newFileName));
            string text = File.ReadAllText(Path.Combine(target.FullName, newFileName));
            text = text.Replace(templateSolutionName, newSolutionName);
            File.WriteAllText(Path.Combine(target.FullName, newFileName), text);
        }

        private void HandleProjects(DirectoryInfo target, FileInfo file, string newSolutionName)
        {

            var newFileName = file.Name.Replace(templateSolutionName, newSolutionName);
            file.CopyTo(Path.Combine(target.FullName, newFileName));
            string text = File.ReadAllText(Path.Combine(target.FullName, newFileName));
            text = text.Replace(templateSolutionName, newSolutionName);
            File.WriteAllText(Path.Combine(target.FullName, newFileName), text);


        }
    }
}
