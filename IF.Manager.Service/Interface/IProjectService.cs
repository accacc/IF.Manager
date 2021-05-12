using IF.Core.Persistence;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IProjectService: IRepository
    {

        Task<bool> ProcessIsExistByName(string name);
        Task AddProject(IFProject form);
        
        Task<IFProject> GetProject(int id);
        Task UpdateProject(IFProject form);
        Task<List<IFProject>> GetProjectList();
        Task AddSolution(IFSolution form);

        Task<IFSolution> GetSolution(int id);
        Task UpdateSolution(IFSolution form);
        Task<List<IFSolution>> GetSolutionList();

        Task AddProcess(ProcessDto form);

        Task<ProcessDto> GetProcess(int id);
        Task UpdateProcess(ProcessDto form);
        Task<List<ProcessDto>> GetProcessList();

   
    }
}
