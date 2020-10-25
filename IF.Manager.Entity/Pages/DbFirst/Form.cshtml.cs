using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;

using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Services;
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

        public FormModel(IEntityService entityService)
        {
            this.entityService = entityService;
            this.ConnectionString = @"Data Source=88.249.221.31\SQLEXPRESS;Initial Catalog=THOS;Persist Security Info=false;User Id=sa;Password=Thos2014@;MultipleActiveResultSets=true;";
        }

        [BindProperty(SupportsGet =true), Required]
        public string ConnectionString { get; set; }
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

      

        public async Task<IActionResult> OnPostAddTablesAsync(List<string> tables)
        {

            var tablesa = GetAllTables();

            var mytables = tablesa.Where(c => tables.Contains(c.Name)).ToList();

            try
            {
                await this.entityService.AddDbFirst(mytables);
            }
            catch (Exception ex)
            {

                throw;
            }

            return RedirectToPage("EntityManagerIndex");

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
