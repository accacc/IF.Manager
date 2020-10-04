using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IF.Manager.Service
{
    public class ConfigGenerator
    {
        public void Generate(string name, TemplateAppSettings settings, string tempDir)
        {

            var config = JsonConvert.SerializeObject(settings, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented });

            config = $@"{{""{name}App"" : {config}}}";


            File.WriteAllText($"{tempDir}/appsettings.Development.json", config);
            File.WriteAllText($"{tempDir}/appsettings.json", config);


        }
    }
}
