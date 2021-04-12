using IF.Core.Persistence;
using IF.Manager.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IF.Manager.Contracts.Services
{
    public interface IPublishService : IRepository
    {
        //Task PublishPage(PublishDto form);
        Task PublishPageTree(PublishDto form);
        Task PublishMenu(PublishDto form);
        Task<List<PublishDto>> GetMenuPublishHistory();

        Task PublishProcess(PublishDto publish);

        Task<List<PublishDto>> GetPublishList(int processId);

        Task AddPublish(PublishDto form);
        Task PublishSolution(PublishDto form);

        Task PublishApiProject(int projectId);

        Task PublishProject(PublishDto publish);
    }
}
