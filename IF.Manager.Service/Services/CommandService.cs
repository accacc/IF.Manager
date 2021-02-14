using IF.Core.Data;
using IF.Core.Exception;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.Model;
using IF.Persistence.EF;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Service.Services
{
    public class CommandService : GenericRepository, ICommandService
    {

        public CommandService(ManagerDbContext dbContext) : base(dbContext)
        {

        }


        public async Task<List<CommandControlTreeDto>> GetCommandTreeList(int ParentId)
        {
            List<CommandControlTreeDto> tree = null;

            try
            {
                var list = await this.GetQuery<IFCommand>().Select

               (map => new CommandControlTreeDto
               {

                   Name = map.Name,
                   Id = map.Id,
                   ParentId = map.ParentId,

               }).ToListAsync();

                var parents = list.Where(c => c.ParentId == ParentId).ToList();


                tree = list.ToTree(parents);
            }
            catch (Exception ex)
            {

                throw;
            }

            return tree;
        }

        public async Task AddCommand(IFCommand form)
        {
            IFCommand entity = new IFCommand();
            entity.Id = form.Id;
            string name = DirectoryHelper.AddAsLastWord(form.Name, "DataCommand");
            entity.Name = name;
            entity.ModelId = form.ModelId;
            entity.ProcessId = form.ProcessId;
            entity.CommandGetType = form.CommandGetType;
            entity.Description = form.Description;

            this.Add(entity);

            await this.UnitOfWork.SaveChangesAsync();

            form.Id = entity.Id;
        }

        public async Task UpdateCommand(IFCommand form)
        {

            try
            {
                var entity = await this.GetQuery<IFCommand>()
            .SingleOrDefaultAsync(k => k.Id == form.Id);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                entity.Name = form.Name;
                entity.Description = form.Description;
                entity.ModelId = form.ModelId;
                entity.CommandGetType = form.CommandGetType;
                entity.ProcessId = form.ProcessId;

                this.Update(entity);
                await this.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IFCommand> GetCommand(int id)
        {
            var entity = await this.GetQuery<IFCommand>()

            .Select(x => new IFCommand
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ModelId = x.ModelId,
                ProcessId = x.ProcessId,
                CommandGetType = x.CommandGetType,

            }).SingleOrDefaultAsync(k => k.Id == id);

            if (entity == null) { throw new BusinessException("Command : No such entity exists"); }

            return entity;
        }


        public async Task<List<IFCommand>> GetCommandList()
        {
            var data = await this.GetQuery<IFCommand>()
                                .Select(x => new IFCommand
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Description = x.Description,
                                }).ToListAsync();

            return data;
        }


        public async Task<List<IFCommandFilterItem>> GetCommandFilterItems(int CommandId)
        {

            var filters = await this.GetQuery<IFCommandFilterItem>(f => f.CommandId == CommandId).ToListAsync();
            return filters;
        }

        public async Task UpdateCommandFilters(List<IFCommandFilterItem> form, int commandId)
        {
            var dtos = form;

            try
            {
                var entity = this.GetQuery<IFCommand>()
               .Include(e => e.CommandFilterItems)
               .SingleOrDefaultAsync(q => q.Id == commandId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                foreach (var dto in dtos)
                {

                    if (dto.Id <= 0)
                    {
                        IFCommandFilterItem filter = new IFCommandFilterItem();

                        if (dto.FormModelPropertyId.HasValue)
                        {
                            filter.FormModelPropertyId = dto.FormModelPropertyId;
                        }
                        else
                        {
                            filter.Value = dto.Value;
                        }

                        filter.CommandId = commandId;
                        filter.ConditionOperator = form.First().ConditionOperator;
                        filter.EntityPropertyId = dto.EntityPropertyId;
                        filter.FilterOperator = dto.FilterOperator;

                        this.Add(filter);
                    }
                    else
                    {
                        var filter = await this.GetQuery<IFCommandFilterItem>(p => p.Id == dto.Id).SingleOrDefaultAsync();
                        if (filter.FormModelPropertyId.HasValue)
                        {
                            filter.FormModelPropertyId = dto.FormModelPropertyId;
                        }
                        else
                        {
                            filter.Value = dto.Value;
                        }

                        filter.CommandId = commandId;
                        filter.ConditionOperator = form.First().ConditionOperator;
                        filter.EntityPropertyId = dto.EntityPropertyId;
                        filter.FilterOperator = dto.FilterOperator;

                        this.Update(filter);
                    }
                }

                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }


        }



        public async Task<List<IFCommand>> GetCommandMultiItems(int CommandId)
        {

            var items = await this.GetQuery<IFCommand>(f => f.Id == CommandId)
                .Include(c=>c.Childrens)
                .ToListAsync();
            return items;
        }


        public async Task UpdateCommandMulties(List<IFCommand> form, int commandId)
        {
            try
            {
                var entity = await this.GetQuery<IFCommand>()
               .Include(e => e.Childrens)
               .SingleOrDefaultAsync(k => k.Id == commandId);

                if (entity == null) { throw new BusinessException(" No such entity exists"); }

                for (int i = 0; i < entity.Childrens.Count; i++)
                {
                    if (!form.Any(d => d.Id == entity.Childrens.ElementAt(i).Id))
                    {
                        entity.Childrens.ElementAt(i).ParentId = null;
                    }
                }

                foreach (var item in form)
                {

                    if (item.Id <= 0)
                    {
                        throw new BusinessException("Command'in id parametresi eksik.");
                    }
                    else
                    {
                        var command = this.GetQuery<IFCommand>().SingleOrDefault(p => p.Id == item.Id);
                        command.ParentId = commandId;
                        command.IFClassMapperId = item.IFClassMapperId;
                        this.Update(command);
                    }
                }

                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }

}
