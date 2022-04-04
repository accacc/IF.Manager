using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;


namespace IF.Manager.Service
{
    public class CoreDllGenerator
    {
        private readonly string fwDllFilePathPattern = @"C:/Projects/IF.Manager/packages/InFramework/{0}.dll";
        public  void GenerateCoreBaseDll(string solutionName,string generatedBasePath)
        {
            var files = Directory.GetFiles(generatedBasePath, "*.cs", SearchOption.AllDirectories).Where(s => s.EndsWith(".cs") || s.EndsWith(".cs")).ToArray();

            var codes = new List<SyntaxTree>();

            foreach (string file in files)
            {

                var source = File.ReadAllText(file);
                codes.Add(Parse(source, "", CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Default)));
            }


            

            IEnumerable<MetadataReference> defaultReferences = new[]
                {
                    MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(string.Format(fwDllFilePathPattern, "IF.Core")),
                    MetadataReference.CreateFromFile(string.Format(fwDllFilePathPattern, "IF.Configuration")),
                    MetadataReference.CreateFromFile(string.Format(fwDllFilePathPattern, "IF.Cqrs")),
                    MetadataReference.CreateFromFile(string.Format(fwDllFilePathPattern, "IF.Dependency.AutoFac")),
                    MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location),"System.Runtime.dll")),
                    MetadataReference.CreateFromFile(typeof(DbContext).Assembly.Location),
                   MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location),"System.Collections.dll")),
                    MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location),"netstandard.dll")),
                    MetadataReference.CreateFromFile(typeof(DisplayAttribute).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(RelationalEntityTypeBuilderExtensions).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Expression).Assembly.Location),
                    //MetadataReference.CreateFromFile(typeof(IServiceCollection).Assembly.Location),
                    //MetadataReference.CreateFromFile(typeof(IConfiguration).Assembly.Location),
                    //MetadataReference.CreateFromFile(typeof(ContainerBuilder).Assembly.Location),
                    //MetadataReference.CreateFromFile(typeof(Expression).Assembly.Location)
                };


            var compilation = CSharpCompilation.Create(DirectoryHelper.GetCoreBaseDllName(solutionName).Replace(".dll",""))
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
              .AddReferences(defaultReferences)
              .AddSyntaxTrees(codes);

            string path = Path.Combine(generatedBasePath, DirectoryHelper.GetCoreBaseDllName(solutionName));


            EmitResult compilationResult = compilation.Emit(path);


            if (!compilationResult.Success)
            {
                string issue = "";

                foreach (Diagnostic codeIssue in compilationResult.Diagnostics)
                {
                    issue += $"ID: {codeIssue.Id}, Message: {codeIssue.GetMessage()},  Location: { codeIssue.Location.GetLineSpan()}, Severity: { codeIssue.Severity}";
                    issue += Environment.NewLine;

                }

                throw new Exception(issue);
            }
        }

        public static SyntaxTree Parse(string text, string filename = "", CSharpParseOptions options = null)
        {
            var stringText = SourceText.From(text, Encoding.UTF8);
            return SyntaxFactory.ParseSyntaxTree(stringText, options, filename);
        }
    }
}
