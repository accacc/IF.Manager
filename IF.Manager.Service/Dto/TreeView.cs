using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IF.Manager.Contracts.Dto
{
    public class JsTreeViwItemState
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }

        public JsTreeViwItemState()
        {
        }
    }

    public class JsTreeViewItemModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }

        public JsTreeViwItemState state { get; set; }

        public JsTreeViewItemModel()
        {
            this.state = new JsTreeViwItemState();
        }

    }

    public static class JsTreeExtension
    {

        public static List<JsTreeViewItemModel> ListToJsTree(List<ClassTreeDto> list, List<int> selecteds)
        {
            list = list.OrderBy(p => p.SortOrder).ToList();

            List<JsTreeViewItemModel> nodes = new List<JsTreeViewItemModel>();

            foreach (var item in list)
            {
                JsTreeViewItemModel node = new JsTreeViewItemModel();

                node.id = item.Id.ToString();
                node.text = item.Name;

                if (!item.ParentId.HasValue)
                {
                    node.parent = "#";
                }
                else
                {
                    node.parent = item.ParentId.ToString();
                }

                if (selecteds != null && selecteds.Any() && selecteds.Contains(Convert.ToInt32(item.Id)))
                {
                    node.state.opened = true;
                    node.state.selected = true;
                }

                nodes.Add(node);
            }

            return nodes;
        }
    }
}
