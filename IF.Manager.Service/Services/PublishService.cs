﻿using IF.Core.Data;
using IF.Core.Navigation;
using IF.Manager.Contracts.Dto;
using IF.Manager.Contracts.Model;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service.Web.Page;
using IF.Manager.Service.Web.Page.Form;
using IF.Manager.Service.Web.Page.ListView;
using IF.Manager.Service.Web.Page.Panel;
using IF.Persistence.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IF.Manager.Service.Services
{
    public class PublishService : GenericRepository, IPublishService
    {

        private readonly IPageService pageService;
        private readonly VsHelper vsHelper;
        private readonly IPageGridService pageGridService;
        private readonly IProjectService projectService;
        public PublishService(ManagerDbContext dbContext,  IPageService pageService, IPageGridService pageGridService, IProjectService projectService) : base(dbContext)
        {
            this.pageService = pageService;
            this.pageGridService = pageGridService;
            this.projectService = projectService;
            this.vsHelper = new VsHelper(DirectoryHelper.GetTempGeneratedDirectoryName());
        }

        public Task<List<PublishDto>> GetMenuPublishHistory()
        {
            throw new NotImplementedException();
        }


        public async Task PublishPageTree(PublishDto form)
        {
            var page = await this.pageService.GetPageTree(form.PageId);            

            PageControl pageControl = new PageControl(page);
         
            pageControl.Generate();

            Render(page.Childrens);

            DirectoryHelper.MoveDirectory(DirectoryHelper.GetTempPageDirectory(pageControl.PageControlMap), DirectoryHelper.GetNewPageDirectory(pageControl.PageControlMap));

        }



        public void Render(ICollection<IFPageControlMap> pageControlMaps)
        {
            foreach (var pageControlMap in pageControlMaps)
            {
                if (pageControlMap.IFPageControl is IFPage)
                {
                    PageControl pageControl = new PageControl(pageControlMap);
                    pageControl.Generate();
                    //DirectoryHelper.MoveDirectory(DirectoryHelper.GetTempPageDirectory(pageControl), DirectoryHelper.GetNewPageDirectory(pageControl));
                }

                else if (pageControlMap.IFPageControl is IFPageGrid)
                {
                    PageGridViewControl gridControl = new PageGridViewControl(pageControlMap);
                    gridControl.Generate();
                    
                }
                else if (pageControlMap.IFPageControl is IFPageAction)
                {
                    //IFPageAction actionControl = new IFPageAction(pageControlMap);
                    //actionControl.Render();
                }
                else if (pageControlMap.IFPageControl is IFPageForm)
                {
                    PageFormControl formControl = new PageFormControl(pageControlMap);
                    formControl.Generate();
                }
                else if (pageControlMap.IFPageControl is IFPageListView)
                {
                    PageListViewControl listViewControl = new PageListViewControl(pageControlMap);
                    listViewControl.Generate();
                }
                else if (pageControlMap.IFPageControl is IFPagePanel)
                {
                    PagePanelControl panel = new PagePanelControl(pageControlMap);
                    panel.Generate();
                }

                if (pageControlMap.Childrens!=null && pageControlMap.Childrens.Any())
                {
                    Render(pageControlMap.Childrens);
                }
            }
        }


        public async Task PublishMenu(PublishDto form)
        {
            var tree = await this.pageService.GetPageControlMapMenuList(form.ProjectId);            

            List<NavigationDto> list = new List<NavigationDto>();

            foreach (var item in tree)
            {
                NavigationDto dto = new NavigationDto();
                dto.Id = item.Id;
                dto.ControlType = item.PageControl.ControlType;
                dto.Name = item.PageControl.Name;
                dto.PageControlId = item.PageControlId;
                dto.ParentId = item.ParentId;
                dto.SortOrder = item.SortOrder;
                if (item.PageControl is IFPage)
                {
                    string path = item.PageControl.IFPageControlMap.GetPagePath();
                    dto.Description = $"{path}/{item.Name}";
                }
                list.Add(dto);
            }

            list = list.ToTree();

            var config = JsonConvert.SerializeObject(list, 
                new JsonSerializerSettings { 
                    NullValueHandling = NullValueHandling.Ignore, 
                    Formatting = Formatting.Indented ,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            File.WriteAllText($"{DirectoryHelper.GetTempGeneratedDirectoryName()}/menu.json", config);

            IFProject project = await this.projectService.GetProject(form.ProjectId);

            this.vsHelper.AddMenuJsonFileToApiProject(project);



        }
    }
}
