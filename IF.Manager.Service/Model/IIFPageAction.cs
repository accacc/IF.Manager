using IF.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Model
{
    public interface IIFPageAction
    {
        string Url { get; set; }

        string Text { get; set; }

        string Style { get; set; }


        ActionWidgetType     WidgetType { get; set; }

        ActionWidgetRenderType WidgetRenderType { get; set; }


    }
}
