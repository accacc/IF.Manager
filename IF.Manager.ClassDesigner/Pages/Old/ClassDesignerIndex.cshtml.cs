using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.ClassDesigner.Pages
{
    public class ClassDesignerIndexModel : PageModel
    {
        private readonly IClassService classService;

        public List<IFCustomClass> ClassList { get; set; }

        public ClassDesignerIndexModel(IClassService ClassService)
        {
            this.classService = ClassService;
        }
        public async Task OnGetAsync()
        {
            await SetModel();
        }

        public async Task<PartialViewResult> OnGetClassListPartialAsync()
        {
            await SetModel();

            return new PartialViewResult
            {
                ViewName = "_ClassListTable",
                ViewData = new ViewDataDictionary<List<IFCustomClass>>(ViewData, this.ClassList)
            };

            //return Partial("",this.ClassList);
        }

        private async Task SetModel()
        {
            var list = await this.classService.GetClassList();

            this.ClassList = list;
        }
    }


}
