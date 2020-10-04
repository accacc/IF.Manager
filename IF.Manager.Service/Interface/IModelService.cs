using IF.Core.Persistence;
using IF.Manager.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IModelService: IRepository
    {
        Task SaveModel(ModelUpdateDto createModel,int ModelId);

        Task AddModel(ModelDto form);
        Task<ModelDto> GetModel(int id);
        Task UpdateModel(ModelDto form);
        Task<List<ModelDto>> GetModelList();
        Task<List<ModelPropertyDto>> GetModelPropertyList(int modelId);
    }
}
