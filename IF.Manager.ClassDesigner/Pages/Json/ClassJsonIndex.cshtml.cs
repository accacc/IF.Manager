using IF.CodeGeneration.Core;
using IF.Manager.Contracts.Services;
using IF.Manager.Service;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Threading.Tasks;

namespace IF.Manager.ClassDesigner.Pages.Json
{
    public class ClassIndexModel : PageModel
    {
        private IHostingEnvironment _environment;
        private readonly IClassService classService;
        private readonly FileSystemCodeFormatProvider fileSystem;
        public ClassIndexModel(IHostingEnvironment environment, IClassService classService)
        {
            _environment = environment;
            this.classService = classService;
            this.fileSystem = new FileSystemCodeFormatProvider(DirectoryHelper.GetTempGeneratedDirectoryName());
        }

        [BindProperty]
        public string  jsondata { get; set; }

        [BindProperty]
        public string name { get; set; }

        public async Task OnPostAsync()
        {
            try
            {
                await this.classService.JsonToClass(name, jsondata);
            }
            catch (System.Exception ex)
            {

            }
        }

      
    }
}
