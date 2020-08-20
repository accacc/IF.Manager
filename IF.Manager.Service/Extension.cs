using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Service
{
    public static class Extension
    {
        public static void AppendDivStart(this StringBuilder builder,string className="",string id = "")
        {
            builder.AppendLine($@"<div id=""{id}"" class=""{className}"">");

        }

        public static void AppendDivRowStart(this StringBuilder builder, string id = "")
        {
            builder.AppendLine($@"<div id=""{id}"" class=""row"">");

        }

        public static void AppendDivColumnMd6(this StringBuilder builder,string id = "")
        {
            builder.AppendLine($@"<div id=""{id}"" class=""col-md-6"">");

        }

        public static void AppendDivEnd(this StringBuilder builder)
        {
            builder.AppendLine($"</div>");

        }
    }
}
