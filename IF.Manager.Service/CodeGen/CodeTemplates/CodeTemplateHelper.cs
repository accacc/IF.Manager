using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace IF.Manager.Service.CodeGen.CodeTemplates
{
    public class CodeTemplateHelper
    {
        public static string GetResourceTextFile(string filename)
        {
            //using Stream stream = typeof(CodeTemplateHelper).Assembly.GetManifestResourceStream($"IF.Manager.Service.{filename}");
            //using StreamReader sr = new StreamReader(stream);

            //return sr.ReadToEnd();

            var assembly = typeof(CodeTemplateHelper).Assembly;
            var a = assembly.GetManifestResourceNames();
            var resourceStream = assembly.GetManifestResourceStream($"IF.Manager.Service.CodeGen.CodeTemplates.{filename}");
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }

            
        }
    }
}
