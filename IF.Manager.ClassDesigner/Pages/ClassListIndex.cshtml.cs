using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.ClassDesigner.Pages
{
    public class ClassListIndexModel : PageModel
    {
        private readonly IClassService classService;

        public List<IFClass> ClassList { get; set; }

        public ClassListIndexModel(IClassService pageService)
        {
            this.classService = pageService;
        }
        public async Task OnGetAsync()
        {
           await SetModel();
        }


        public async Task<PartialViewResult> OnPostGenerateAsync(int ClassId)
        {
            try
            {
                await this.classService.GenerateClass(ClassId);
            }
            catch (System.Exception ex)
            {

                throw;
            }


            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_ClassListTable",
                ViewData = new ViewDataDictionary<List<IFClass>>(ViewData, this.ClassList)
            };


        }

        public async Task<PartialViewResult> OnGetPageListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_ClassListTable",
                ViewData = new ViewDataDictionary<List<IFClass>>(ViewData, this.ClassList)
            };

        }

        public async Task<PartialViewResult> OnGetClassTreeListAsync(int Id)
        {
            ClassMapModel model = new ClassMapModel();
            
            model.IsModal = true;

            var @class = await this.classService.GetClass(Id);

            if (@class != null)
            {
                var tree = await this.classService.GetClassTreeList(@class.Id);


                model.Tree = tree;
                model.ClassId = @class.Id;
            }
            else
            {
                model.Tree = new List<ClassControlTreeDto>();
            }


            return new PartialViewResult
            {
                ViewName = "_ClassTreeMain",
                ViewData = new ViewDataDictionary<ClassMapModel>(ViewData, model)
            };
        }

        private async Task SetModel()
        {
            this.ClassList = await this.classService.GetClassList();
        }
    }
}
