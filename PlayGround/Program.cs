using Flee.PublicTypes;
using IF.CodeGeneration.Core;
using IF.Manager.Contracts.Model;
using IF.Manager.Service;
using System;
using System.Linq.Expressions;

namespace PlayGround
{
    class Program
    {


        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int Weight { get; set; }
            public DateTime FavouriteDay { get; set; }
        }


        static void Main(string[] args)
        {

            //var context = new ExpressionContext();
            //const string exp = @"(Person.Age > 3 AND Person.Weight > 50) OR Person.Age < 3";
            //context.Variables.DefineVariable("Person", typeof(Person));
            //var e = context.CompileDynamic(exp);

            //var bob = new Person
            //{
            //    Name = "Bob",
            //    Age = 30,
            //    Weight = 213,
            //    FavouriteDay = new DateTime(2000, 1, 1)
            //};

            //context.Variables["Person"] = bob;
            //var result = e.Evaluate();
            //Console.WriteLine(result);
            //Console.ReadKey();
        }

        private static void Config()
        {
            string tempPath = @"C:\temp\generated\";
            string name = "Thos";


            FileSystemCodeFormatProvider fileSystem = new FileSystemCodeFormatProvider(tempPath);
            //GenerateConfigs(tempPath, name, fileSystem);
            IFProject project = new IFProject();
            project.Name = name;
            StartupClassWebApi startup = new StartupClassWebApi(project,entityService);
            await startup.Build();
            //fileSystem.FormatCode(startup.GenerateCode(), "cs");
        }

        private static void GenerateConfigs(string tempPath, string name, FileSystemCodeFormatProvider fileSystem)
        {
            //TemplateAppSettings settings = new TemplateAppSettings();
            //settings.Database = new IF.Core.Database.DatabaseSettings();
            //settings.Database.ConnectionString = "cnnstring";

            //settings.ApplicationName = name;
            //settings.Version = "1.0.0";
            //settings.RabbitMQConnection = new IF.Core.RabbitMQ.RabbitMQConnectionSettings();
            //settings.RabbitMQConnection.EventBusConnection = "aaaa";


            //ConfigGenerator configGenerator = new ConfigGenerator();
            //configGenerator.Generate(name, settings, tempPath);


            //ConfigInterface configInterface = new ConfigInterface(settings, name,"IF");
            //configInterface.Build();
            //fileSystem.FormatCode(configInterface.GenerateCode(), "cs");

            //ConfigClass configClass = new ConfigClass(settings, name,"IF");
            //configClass.Build();
            //fileSystem.FormatCode(configClass.GenerateCode(), "cs");
        }
    }
}
