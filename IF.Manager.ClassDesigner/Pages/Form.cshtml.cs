using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.Model;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.ClassDesigner.Pages
{
    public class FormModel : PageModel
    {

        private readonly IClassService classService;
        private readonly IHostingEnvironment environment;


        public FormModel(IClassService classService, IHostingEnvironment environment)
        {
            this.classService = classService;
            this.environment = environment;
        }


        [BindProperty]
        public IFormFile Upload { get; set; }

        public PartialViewResult OnGetLoadJsonAsync()
        {


            return new PartialViewResult
            {
                ViewName = "_JsonForm",
                ViewData = new ViewDataDictionary<FormModel>(ViewData, this)
            };
        }

        public void OnPostLoadJson()
        {
            //var file = Path.Combine(this.environment.ContentRootPath, "uploads", Upload.FileName);
            //using (var fileStream = new FileStream(file, FileMode.Create))
            //{
            //    await Upload.CopyToAsync(fileStream);
            //}
            string result = string.Empty;

            try

            {

                long size = 0;

                var file = Request.Form.Files;

                var filename = ContentDispositionHeaderValue

                                .Parse(file[0].ContentDisposition)

                                .FileName

                                .Trim('"');

                string FilePath = environment.WebRootPath + $@"\{ filename}";

                size += file[0].Length;

                using (FileStream fs = System.IO.File.Create(FilePath))

                {

                    file[0].CopyTo(fs);

                    fs.Flush();

                }



                result = FilePath;

            }

            catch (Exception ex)

            {

                result = ex.Message;

            }




        
    }


    [BindProperty, Required]
    public IFCustomClass Form { get; set; }
    public void OnGetAddAsync()
    {
        this.Form = new IFCustomClass();
        // await this.SetFromDefaults();

    }

    public async Task OnGetUpdateAsync(int Id)
    {
        this.Form = await this.classService.GetClass(Id);
        // await this.SetFromDefaults();
    }

    public async Task<PartialViewResult> OnPostAddAsync()
    {

        await this.classService.AddClass(this.Form);


        var list = await this.classService.GetClassList();

        return new PartialViewResult
        {
            ViewName = "_ClassListTable",
            ViewData = new ViewDataDictionary<List<IFCustomClass>>(ViewData, list)
        };
    }

    public async Task<PartialViewResult> OnPostUpdateAsync()
    {

        await this.classService.UpdateClass(this.Form);


        var list = await this.classService.GetClassList();

        return new PartialViewResult
        {
            ViewName = "_ClassListTable",
            ViewData = new ViewDataDictionary<List<IFCustomClass>>(ViewData, list)
        };
    }

}
}
