using IF.Core.Persistence;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IPageFormService: IRepository
    {

        Task<List<ModelPropertyDto>> GetFormModelProperties(int Id);
        Task<List<IFPageForm>> GetFormList();
        Task AddForm(IFPageForm form);
        Task UpdateForm(IFPageForm form);
        Task<IFPageForm> GetForm(int id);
        Task<List<IFPageFormLayout>> GetFormLayouts();
        Task<List<IFPageFormItem>> GetFormItems();
        Task<List<IFPageFormItemModelProperty>> GetPageFormItemModelProperties(int id);
        Task UpdateFormItemModelProperties(List<IFPageFormItemModelProperty> form, int formId);
        Task AddPageControlItemModelProperty(IFPageFormItemModelProperty form);
        Task UpdatePageControlItemModelProperty(IFPageFormItemModelProperty form);
        Task<IFPageFormItemModelProperty> GetPageControlItemModelProperty(int iFPageFormItemModelPropertyId);
        void MoveModelItemUp(int ıd);
        void MoveModelItemDown(int ıd);
    }
}
