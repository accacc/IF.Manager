using IF.Manager.Contracts.Services;
using IF.Manager.Service.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IF.Manager.ClassDesigner.Pages.Mapper
{


    public class ClassMapperIndexModel : PageModel
    {
        private readonly IClassService classService;

        public List<IFClassMapper> ClassMapperList { get; set; }

        public ClassMapperIndexModel(IClassService pageService)
        {
            this.classService = pageService;
        }
        public async Task OnGetAsync()
        {
            this.ClassMapperList = await this.classService.GetClassMapperList();
        }




        public async Task<PartialViewResult> OnGetPageListPartialAsync()
        {
            this.ClassMapperList = await this.classService.GetClassMapperList();

            return new PartialViewResult
            {
                ViewName = "_ClassMapperListTable",
                ViewData = new ViewDataDictionary<List<IFClassMapper>>(ViewData, this.ClassMapperList)
            };

        }

        public async Task<PartialViewResult> OnPostGenerateAsync(int ClassMapId)
        {
            try
            {
               // var result = await this.classService.GenerateMapper(ClassMapId);
            }
            catch (System.Exception ex)
            {

                throw;
            }

            this.ClassMapperList = await this.classService.GetClassMapperList();


            return new PartialViewResult
            {
                ViewName = "_ClassMapperListTable",
                ViewData = new ViewDataDictionary<List<IFClassMapper>>(ViewData, this.ClassMapperList)
            };


        }


    }
}

