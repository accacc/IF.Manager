using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;

using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Service;
using IF.Web.Mvc.Extensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;

namespace IF.Manager.Entity.Pages.DbFirst
{
    
    public class FormModel : PageModel
    {

        private readonly IEntityService entityService;
        private readonly IProjectService projectService;
     
        
        [BindProperty, Required]
        public List<TableDbFirstDto> Form { get; set; }
        
        public FormModel(IEntityService entityService,IProjectService projectService)
        {
            this.entityService = entityService;
            this.projectService = projectService;
            //this.ConnectionString = @"Data Source=88.249.221.31;Initial Catalog=THOS;Persist Security Info=false;User Id=sa;Password=Thos2014@;MultipleActiveResultSets=true;";
        }

      

        [BindProperty(SupportsGet =true), Required]
        public int IFProjectId { get; set; }

        [BindProperty(SupportsGet = true), Required]
        public string ConnectionString { get; set; }


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
            var entities = await this.projectService.GetProjectList();

            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var entity in entities)
            {
                SelectListItem item = new SelectListItem();

                item.Text = entity.Name;
                item.Value = entity.Id.ToString();
                items.Add(item);
            }

            ViewData["projects"] = items;
        }


        public PartialViewResult OnGetListTablesAsync()
        {
            try
            {
                List<DatabaseTable> list = GetAllTables();

                return new PartialViewResult
                {
                    ViewName = "_EntityListTable",
                    ViewData = new ViewDataDictionary<List<DatabaseTable>>(ViewData, list)
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

            var tablesa = GetAllTables();

            var mytables = tablesa.Where(c => this.Form.Select(s=>s.Table).ToArray().Contains(c.Name)).ToList();

            try
            {
                await this.entityService.AddDbFirst(mytables,this.Form);
            }
            catch (Exception ex)
            {

                throw;
            }

            return this.OperationResult();

        }

        private List<DatabaseTable> GetAllTables()
        {
            List<DatabaseTable> list = new List<DatabaseTable>();

            using (var connection = new SqlConnection(this.ConnectionString))
            {
                var dr = new DatabaseSchemaReader.DatabaseReader(connection);
                //Then load the schema (this will take a little time on moderate to large database structures)
                var schema = dr.ReadAll();

                //The structure is identical for all providers (and the full framework).
                foreach (var table in schema.Tables)
                {
                    list.Add(table);
                }
            }

            return list;
        }

    }
}
