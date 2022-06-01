using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IF.Manager.Command.Pages.Filter
{
    

    public class CommandFilterModel : PageModel
    {

        private readonly IEntityService entityService;
        private readonly ICommandService commandService;
        private readonly IModelService modelService;

        [BindProperty, Required]
        public List<IFCommandFilterItem> Form { get; set; }


        [BindProperty(SupportsGet = true), Required]
        public int CommandId { get; set; }


        public CommandFilterModel(IEntityService entityService, ICommandService commandService, IModelService modelService)
        {
            this.entityService = entityService;
            this.commandService = commandService;
            this.modelService = modelService;
        }


        public async Task OnGet(int Id)
        {

            this.Form = await this.commandService.GetCommandFilterItems(Id);

            if (!this.Form.Any())
            {
                SetEmptyForm(Id);
            }

            await SetFromDefaults(Id);

        }



        public async Task<PartialViewResult> OnGetEmptyFormItemPartialAsync(int Id)
        {
            await SetFromDefaults(Id);

            var emptyFormItem = new IFCommandFilterItem();

            return new PartialViewResult
            {
                ViewName = "_CommandFilterFormItem",
                ViewData = new ViewDataDictionary<IFCommandFilterItem>(ViewData, emptyFormItem)
            };
        }

        public async Task<PartialViewResult> OnPost()
        {
            await this.commandService.UpdateCommandFilters(this.Form,this.CommandId);


            var CommandList = await this.commandService.GetCommandList();
            

            return new PartialViewResult
            {
                ViewName = "_CommandListTable",
                ViewData = new ViewDataDictionary<List<IFCommand>>(ViewData, CommandList)
            };

        }

        private void SetEmptyForm(int Id)
        {
            this.Form = new List<IFCommandFilterItem>();
            this.CommandId = Id;
            var item = new IFCommandFilterItem();
            
            this.Form.Add(item);            

        }

        private async Task SetFromDefaults(int Id)
        {
            var Command = await this.commandService.GetCommand(Id);

            var model = await this.modelService.GetModel(Command.ModelId.Value);

            var entities = await this.entityService.GetEntityAllRelations(model.EntityId);

          
            SetEntities(entities);

        }

    
       
        private void SetEntities(List<EntityDto> entities)
        {
            List<SelectListItem> items = new List<SelectListItem>();


            foreach (var entity in entities)
            {
                foreach (var property in entity.Properties)
                {
                    SelectListItem item = new SelectListItem();

                    //if (this.Form.ModelId == group.Id)
                    //{
                    //    item.Selected = true;
                    //}

                    item.Text = entity.Name + " - " + property.Name;
                    item.Value = property.Id.ToString();
                    items.Add(item);
                }


            }

            ViewData["entities"] = items;
        }
    }
}
