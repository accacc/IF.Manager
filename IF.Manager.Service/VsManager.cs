using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IF.Manager.Service
{
    public class VsManager
    {
        //private readonly string solutionName;
        //private readonly string solutionPath;
        //private readonly string basePath;
        public VsManager()
        {
            //this.solutionName = solutionName;
            //this.solutionPath = solutionPath;
            //this.basePath = path;
        }



        //type cs:Compile
        public void AddFile(string type, string from, string to, string projectFile, string itemName, string fileExtension,string sdk)
        {


            RemoveSdk(projectFile,sdk);

            var project = new Microsoft.Build.Evaluation.Project(projectFile);

            if (type != "Reference")
            {
                if (!Directory.Exists(to))
                {
                    project.AddItem("Folder", to);

                    Directory.CreateDirectory(to);
                }
            }


            File.Copy($"{from}/{itemName}.{fileExtension}", $@"{to}/{itemName}.{fileExtension}", true);


            var projectReferences = project.GetItems("ProjectReference");

            //if(projectReferences.Where(r=>r.($"{itemName}.{fileExtension}"))
            //{ 

            //if (!File.Exists($@"{to}/{itemName}.{fileExtension}"))
            //{
            var result =  project.AddItem(type, $@"{to}/{itemName}.{fileExtension}");
                
            //}

            project.Save();



            project.ProjectCollection.UnloadProject(project);

            this.AddSdk(projectFile,sdk);
        }

        private void RemoveSdk(string projectFile,string sdk)
        {
            string text = File.ReadAllText(projectFile);
            text = text.Replace($@"<Project Sdk=""{sdk}"">", "<Project>");           
            File.WriteAllText(projectFile, text);
        }

        private void AddSdk(string projectFile, string sdk)
        {
            string text = File.ReadAllText(projectFile);
            text = text.Replace("<Project>", $@"<Project Sdk=""{sdk}"">");
            File.WriteAllText(projectFile, text);
        }


    }
}
