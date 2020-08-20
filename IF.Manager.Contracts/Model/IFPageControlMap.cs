using IF.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageControlMap:Entity
    {
        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }

        public IFPageControlMap Parent { get; set; }

        public ICollection<IFPageControlMap> Childrens { get; set; }

        public int IFPageControlId { get; set; }

        public IFPageControl IFPageControl { get; set; }

        public T ToControl<T>() where T : IFPageControl
        {
            return this.IFPageControl as T;
        }


        public string GetPageUrl()
        {
            var pagePath = this.GetPagePath();

            pagePath += "/" + this.IFPageControl.Name;
            return pagePath;
        }

        public string GetPagePath()
        {
            var pagePath = "";
            var parents = this.GetParentPages();

            foreach (var parent in parents)
            {
                if (parent is IFPage)
                {
                    pagePath += parent.Name + "/";
                }
            }

            if (this.IFPageControl is IFPage)
            {
                pagePath += this.IFPageControl.Name;
            }
            else
            {
                pagePath = pagePath.Remove(pagePath.Length - 1);
            }

            return pagePath;
        }

        public string GetPageNameSpace()
        {
            var pagePath = "";
            var parents = this.GetParentPages();

            foreach (var parent in parents)
            {
                if (parent is IFPage)
                {
                    pagePath += parent.Name + ".";
                }
            }

            if (this.IFPageControl is IFPage)
            {
                pagePath += this.IFPageControl.Name;
            }
            else
            {
                pagePath = pagePath.Remove(pagePath.Length - 1);

            }

            return pagePath;
        }
        public IFPage GetTopPage(bool GetFirstParent = false)
        {

            //if (this.IFPageControl is IFPage && (this.Parent == null || this.Parent.IFPageControl is IFPageNavigation)) return (IFPage)this.IFPageControl;

            var page = this;

            while (page != null)
            {
                if (page.Parent == null || page.Parent.IFPageControl is IFPageNavigation) break;               

                page = page.Parent;                

                if (page.IFPageControl is IFPage && GetFirstParent) return (IFPage)page.IFPageControl;
            }

            if (page != null && page.IFPageControl is IFPage)
            {
                return (IFPage)page.IFPageControl;
            }
            else
            {
                return null;
            }
        }

        public List<IFPage> GetParentPages()
        {

            List<IFPage> paths = new List<IFPage>();

            if (this.IFPageControl is IFPage && this.Parent == null)
            {
                paths.Add((IFPage)this.IFPageControl);
            }

            var page = this;

            while (page != null)
            {

                if (page.Parent == null) break;

                if (!(page.Parent.IFPageControl is IFPage))
                {
                    page = page.Parent;
                    continue;
                }
                else
                {
                    page = page.Parent;
                    paths.Add((IFPage)page.IFPageControl);
                }


            }

            paths.Reverse();

            return paths;
        }
    }
}
