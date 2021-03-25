using DatabaseSchemaReader.DataSchema;

using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service.CodeGen;
using IF.Manager.Service.Dto;
using IF.Manager.Service.Interface;
using IF.Web.Mvc.Extensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Entity.Pages.DbFirst
{
  

    public class FormModel : PageModel
    {

        private readonly IDbFirstService entityService;
        private readonly IProjectService projectService;


        [BindProperty, Required]
        public List<TableDbFirstDto> Form { get; set; }


        [BindProperty, Required]
        public bool SelectOperation { get; set; }

        [BindProperty, Required]
        public bool InsertOperation { get; set; }

        [BindProperty, Required]
        public bool UpdateOperation { get; set; }

        [BindProperty, Required]
        public bool DeleteOperation { get; set; }

        public FormModel(IDbFirstService entityService, IProjectService projectService)
        {
            this.entityService = entityService;
            this.projectService = projectService;
        }



        [BindProperty(SupportsGet = true), Required]
        public int IFProjectId { get; set; }

        [BindProperty(SupportsGet = true), Required]
        public string ConnectionString { get; set; }

        [BindProperty(SupportsGet = true), Required]
        public string ProcessName { get; set; }


        public async Task OnGet()
        {
            await this.SetFormDefaults();

        }

        public async Task<IActionResult> OnGetGetProjectCnnString()
        {
            IFProject project = await this.projectService.GetProject(this.IFProjectId);

            return Content(project.ConnectionString);

        }

        private async Task SetFormDefaults()
        {
            var projects = await this.projectService.GetProjectList();

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var project in projects)
            {
                SelectListItem item = new SelectListItem();

                item.Text = project.Name;
                item.Value = project.Id.ToString();
                items.Add(item);
            }

            ViewData["projects"] = items;
        }


        public PartialViewResult OnGetListTablesAsync()
        {
            try
            {
                var allTableSchemas = this.entityService.GetAllTableSchemas(this.ConnectionString);

                return new PartialViewResult
                {
                    ViewName = "_EntityListTable",
                    ViewData = new ViewDataDictionary<List<DatabaseTable>>(ViewData, allTableSchemas)
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [DisableRequestSizeLimit]

        public async Task<IActionResult> OnPostAddTablesAsync()
        {
            this.Form = this.Form.Where(f => f != null).ToList();

            var allTableSchemas = this.entityService.GetAllTableSchemas(this.ConnectionString);

            var tableSchemas = allTableSchemas.Where(c => this.Form.Select(s => s.Table).ToArray().Contains(c.Name)).ToList();

            var generateOptions = new GenerateOptions();
            generateOptions.DeleteOperation = this.DeleteOperation;
            generateOptions.SelectOperation = this.SelectOperation;
            generateOptions.UpdateOperation = this.UpdateOperation;
            generateOptions.InsertOperation = this.InsertOperation;
            generateOptions.ProjectId = this.IFProjectId;
            generateOptions.ProcessName = this.ProcessName;

            try
            {
                await this.entityService.AddDbFirst(tableSchemas, this.Form,generateOptions);
            }
            catch (Exception ex)
            {

                throw;
            }

            return this.OperationResult();

        }

      

    }
}
