using IF.Core.Data;
using IF.Core.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public class IFPageAction: IFPageControl
    {

        public IFPageAction()
        {
            this.IFPageActionRouteValues = new List<IFPageActionRouteValue>();
        }

        public string Url { get; set; }

        public string Text { get; set; }

        public string Style { get; set; }

        public int SortOrder { get; set; }

        public int? QueryId { get; set; }

        public int? CommandId { get; set; }

        public int? IFModelId { get; set; }

        public IFCommand Command { get; set; }

        public IFModel IFModel { get; set; }

        public IFPageControl IFPageControl { get; set; }

        public int? IFPageControlId  { get; set; }

        public IFQuery Query { get; set; }
        public ActionWidgetType WidgetType { get; set; }

        public ActionType ActionType { get; set; }
        public ActionWidgetRenderType WidgetRenderType { get; set; }

        public ICollection<IFPageActionRouteValue> IFPageActionRouteValues { get; set; }

    }
}
